using System;
using Lex.CodeAnalysis.Syntax;
using Lex.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lex.Tests.CodeAnalysis.Syntax
{
    public class LexerTests
    {

        [Fact]
        public void Lexer_Lexes_UnterminatedString()
        {
            var text = "\"text";
            var tokens = SyntaxTree.ParseTokens(text, out var diagnostics);
            var token = Assert.Single(tokens);
            Assert.Equal(SyntaxKind.StringToken, token.Kind);
            Assert.Equal(text, token.Text);

            var diagnostic = Assert.Single(diagnostics);
            Assert.Equal(new TextSpan(0,1), diagnostic.Span);
            Assert.Equal("Unterminated string Literal.", diagnostic.Message);
        }


        [Fact]
        public void Lexer_Tests_AllToken()
        {
            var tokendKinds = Enum.GetValues(typeof(SyntaxKind))
                                  .Cast<SyntaxKind>()
                                  .Where(k => k.ToString().EndsWith("Keyword")||
                                              k.ToString().EndsWith("Token"))
                                   .ToList();

            var testedTokenKinds = GetTokens().Concat(GetSeparators()).Select(t => t.kind);
    
            var untestedTokenKind = new SortedSet<SyntaxKind>(tokendKinds);
            untestedTokenKind.Remove(SyntaxKind.BadToken);
            untestedTokenKind.Remove(SyntaxKind.EndOfFileToken);
            untestedTokenKind.ExceptWith(testedTokenKinds);

            Assert.Empty(untestedTokenKind);

        }

        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Lexes_Token(SyntaxKind kind, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text);

            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_Lexes_TokenPairs(SyntaxKind t1Kind, string t1Text,
                                           SyntaxKind t2Kind, string t2Text)
        {
            var text = t1Text + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(2, tokens.Length);
            Assert.Equal(t1Kind, tokens[0].Kind);
            Assert.Equal(t1Text, tokens[0].Text);
            Assert.Equal(t2Kind, tokens[1].Kind);
            Assert.Equal(t2Text, tokens[1].Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsWithSeparatorData))]
        public void Lexer_Lexes_TokenPairs_WithSeparators(SyntaxKind t1Kind, string t1Text,
                                                          SyntaxKind separatorKind, string separatorText,
                                                          SyntaxKind t2Kind, string t2Text)
        {
            var text = t1Text + separatorText + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(3, tokens.Length);
            Assert.Equal(t1Kind, tokens[0].Kind);
            Assert.Equal(t1Text, tokens[0].Text);
            Assert.Equal(separatorKind, tokens[1].Kind);
            Assert.Equal(separatorText, tokens[1].Text);
            Assert.Equal(t2Kind, tokens[2].Kind);
            Assert.Equal(t2Text,tokens[2].Text);
        }

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var t in GetTokens().Concat(GetSeparators()))
                yield return new object[] { t.kind, t.text };
        }

        public static IEnumerable<object[]> GetTokenPairsData()
        {
            foreach (var t in GetTokenPairs())
                yield return new object[] { t.t1Kind, t.t1Text, t.t2Kind, t.t2Text };
        }

        public static IEnumerable<object[]> GetTokenPairsWithSeparatorData()
        {
            foreach (var t in GetTokenPairsWithSeparator())
                yield return new object[] { t.t1Kind, t.t1Text, t.separatorKind, t.separatorText, t.t2Kind, t.t2Text };
        }

        private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
        {
            var fixedTokens = Enum.GetValues(typeof(SyntaxKind))
                                  .Cast<SyntaxKind>()
                                  .Select(k => (kind: k, text: SyntaxFacts.GetText(k)))
                                  .Where(t => t.text != null);


            var dynamicTokens = new[]
            {
                (SyntaxKind.NumberToken, "1"),
                (SyntaxKind.NumberToken, "123"),
                (SyntaxKind.IdentifierToken, "a"),
                (SyntaxKind.StringToken, "\"Test\""),
                (SyntaxKind.StringToken, "\"Te \"\"st\""),
            };

            return fixedTokens.Concat(dynamicTokens);
        }

        private static IEnumerable<(SyntaxKind kind, string text)> GetSeparators()
        {
            return new[]
            {
                (SyntaxKind.WhitespaceToken, " "),
                (SyntaxKind.WhitespaceToken, "  "),
                (SyntaxKind.WhitespaceToken, "\r"),
                (SyntaxKind.WhitespaceToken, "\n"),
                (SyntaxKind.WhitespaceToken, "\r\n")
            };
        }

        private static bool RequiresSeparator(SyntaxKind t1Kind, SyntaxKind t2Kind)
        {
            var t1IsKeyword = t1Kind.ToString().EndsWith("Keyword");
            var t2IsKeyword = t2Kind.ToString().EndsWith("Keyword");

            if (t1Kind == SyntaxKind.IdentifierToken && t2Kind == SyntaxKind.IdentifierToken)
                return true;

            if (t1IsKeyword && t2IsKeyword)
                return true;

            if (t1IsKeyword && t2Kind == SyntaxKind.IdentifierToken)
                return true;

            if (t1Kind == SyntaxKind.IdentifierToken && t2IsKeyword)
                return true;

            if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.NumberToken)
                return true;

            if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EaqlesToken)
                return true;

            if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EaqulesEaqlesToken)
                return true;

            if (t1Kind == SyntaxKind.EaqlesToken && t2Kind == SyntaxKind.EaqlesToken)
                return true;

            if (t1Kind == SyntaxKind.EaqlesToken && t2Kind == SyntaxKind.EaqulesEaqlesToken)
                return true;
            if (t1Kind == SyntaxKind.LessThanToken && t2Kind == SyntaxKind.EaqulesEaqlesToken)
                return true;
            if (t1Kind == SyntaxKind.LessThanToken && t2Kind == SyntaxKind.EaqlesToken)
                return true;
            if (t1Kind == SyntaxKind.GreaterThanToken && t2Kind == SyntaxKind.EaqulesEaqlesToken)
                return true;
            if (t1Kind == SyntaxKind.GreaterThanToken && t2Kind == SyntaxKind.EaqlesToken)
                return true;
            if (t1Kind == SyntaxKind.StarToken && t2Kind == SyntaxKind.StarStarToken)
                return true;
            if (t1Kind == SyntaxKind.StarToken && t2Kind == SyntaxKind.StarToken)
                return true;
            if (t1Kind == SyntaxKind.StringToken && t2Kind == SyntaxKind.StringToken)
                return true;
            if (t1Kind == SyntaxKind.PipeToken && t2Kind == SyntaxKind.PipeToken)
                return true;
            if (t1Kind == SyntaxKind.AmpersandToken && t2Kind == SyntaxKind.AmpersandToken)
                return true;
            if (t1Kind == SyntaxKind.AmpersandToken && t2Kind == SyntaxKind.AmpersandAmpersandToken)
                return true;
            if (t1Kind == SyntaxKind.PipeToken && t2Kind == SyntaxKind.PipePieToken)
                return true;
            if (t1Kind == SyntaxKind.WhileKeyword  && t2Kind == SyntaxKind.ByKeyWord)
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.WhileKeyword)
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.ToKeyword )
                return true;
            if (t1Kind == SyntaxKind.ToKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.IfKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.TrueKeyword )
                return true;
            if (t1Kind == SyntaxKind.TrueKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ForKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ElseKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.ForKeyword )
                return true;
             if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.ElseKeyword )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.FalseKeyword )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.IfKeyword )
                return true;
            if (t1Kind == SyntaxKind.FalseKeyword && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.IdentifierToken )
                return true;
            if (t1Kind == SyntaxKind.IdentifierToken  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.FunctionKeyword )
                return true;
            if (t1Kind == SyntaxKind.FunctionKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.BreakKeyword )
                return true;
            if (t1Kind == SyntaxKind.ContinueKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.BreakKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.ContinueKeyword )
                return true;
            if (t1Kind == SyntaxKind.ByKeyWord  && t2Kind == SyntaxKind.ReturnKeyword )
                return true;
            if (t1Kind == SyntaxKind.ReturnKeyword  && t2Kind == SyntaxKind.ByKeyWord )
                return true;

            return false;
        }

        private static IEnumerable<(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text)> GetTokenPairs()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (!RequiresSeparator(t1.kind, t2.kind))
                        yield return (t1.kind, t1.text, t2.kind, t2.text);
                }
            }
        }

        private static IEnumerable<(SyntaxKind t1Kind, string t1Text,
                                    SyntaxKind separatorKind, string separatorText,
                                    SyntaxKind t2Kind, string t2Text)> GetTokenPairsWithSeparator()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (RequiresSeparator(t1.kind, t2.kind))
                    {
                        foreach (var s in GetSeparators())
                            yield return (t1.kind, t1.text, s.kind, s.text, t2.kind, t2.text);
                    }
                }
            }
        }
    }
}
