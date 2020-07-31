using Lex.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lex.Tests.CodeAnalysis.Syntax
{

    internal sealed class AssertingEnumerator: IDisposable
    {

        private IEnumerator<SyntaxNode> _enumerator;
        private bool _hasError;
        public AssertingEnumerator(SyntaxNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
        }
        public void Dispose()
        {
            if(_hasError)
                Assert.False(_enumerator.MoveNext());
            _enumerator.Dispose();
        }

        private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(node);

            while(stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                foreach (var child in n.GetChildren().Reverse())
                    stack.Push(child);
            }
        }

        public void AssertNode(SyntaxKind kind)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.IsNotType<SyntaxToken>(_enumerator.Current);
                Assert.Equal(kind, _enumerator.Current.Kind);
            }
            catch(Exception)
            {
                _hasError = true;
                throw;
            }

        }
        public void AssertToken(SyntaxKind kind, string text)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                var token = Assert.IsType<SyntaxToken>(_enumerator.Current);
                Assert.Equal(kind, token.Kind);
                Assert.Equal(text, token.Text);
            }
            catch (Exception)
            {
                _hasError = true;
                throw;
            }
            
        }

        


    }


    public class ParserTests
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void Parser_BinaryExpression_honorsPrecedences(SyntaxKind op1, SyntaxKind op2)
        {
            var op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            var op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            var op1Text = SyntaxFacts.GetText(op1);
            var op2Text = SyntaxFacts.GetText(op2);
            var text = $"a {op1Text} b {op2Text} c";
            var expression = SyntaxTree.Parse(text).Root;

            if (op1Precedence >= op2Precedence)
            {
               using(var e = new AssertingEnumerator(expression))
                {
                    e.AssertNode(SyntaxKind.BinaryExpression);
                    e.AssertNode(SyntaxKind.BinaryExpression);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "a");
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "b");
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
            else
            {
                using (var e = new AssertingEnumerator(expression))
                {
                    e.AssertNode(SyntaxKind.BinaryExpression);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "a");
                    e.AssertNode(SyntaxKind.BinaryExpression);
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "b");
                    e.AssertNode(SyntaxKind.NameExpression);
                    e.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach(var op1 in SyntaxFacts.GetBinaryOperatorsKinds())
            {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorsKinds())
                {
                    yield return new object[] { op1, op2 };
                }
            }
        }



    }
}