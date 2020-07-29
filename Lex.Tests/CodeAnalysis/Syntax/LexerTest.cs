using Lex.CodeAnalysis.Syntax;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lex.Tests.CodeAnalysis.Syntax
{
    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_lexes_token(SyntaxKind kind, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_lexes_token_Pairs(SyntaxKind t1kind, string t1text, SyntaxKind t2kind, string t2text)
        {
            var text = t1text + t2text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(2, tokens.Length);

            Assert.Equal(tokens[0].Kind, t1kind);
            Assert.Equal(tokens[0].Text, t1text);

            Assert.Equal(tokens[1].Kind, t2kind);
            Assert.Equal(tokens[1].Text, t2text);
        }

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var t in GetTokens())
                yield return new object[] { t.kind, t.text };
        }

        public static IEnumerable<object[]> GetTokenPairsData()
        {
            foreach (var t in GetTokenPairs()) 
                yield return new object[] { t.t1Kind, t.t1Text, t.t2Kind, t.t2Text};
        }

        private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
        {
            return new[]
            {
                

                (SyntaxKind.PlusToken, "+"),
                (SyntaxKind.MinusToken, "-"),
                (SyntaxKind.StarToken, "*"),
                (SyntaxKind.SlashToken, "/"),
                (SyntaxKind.EaqlesToken, "="),
                (SyntaxKind.BangToken, "!"),
                (SyntaxKind.EaqulesEaqlesToken, "=="),
                (SyntaxKind.BangEaqlesToken, "!="),
                (SyntaxKind.AmpersandAmpersandToken, "&&"),
                (SyntaxKind.PipePieToken, "||"),
                (SyntaxKind.OpenParenthesisToken, "("),
                (SyntaxKind.CloseParenthesisToken, ")"),
                (SyntaxKind.TrueKeyword, "true"),
                (SyntaxKind.FalseKeyword, "false"),
                //(SyntaxKind.WhitespaceToken, " "),
                //(SyntaxKind.WhitespaceToken, "  "),
                //(SyntaxKind.WhitespaceToken, "\r"),
                //(SyntaxKind.WhitespaceToken, "\n"),
                //(SyntaxKind.WhitespaceToken, "\r\n"),
                (SyntaxKind.NumberToken, "1"),
                (SyntaxKind.NumberToken, "123"),
                (SyntaxKind.IdentifierToken, "a"),
                (SyntaxKind.IdentifierToken, "abc"),
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
            return false;
        }

        private static IEnumerable<(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text)> GetTokenPairs()
        {
            foreach( var t1 in GetTokens())
            {
                foreach(var t2 in GetTokens())
                {

                    if(!RequiresSeparator(t1.kind,t2.kind))
                        yield return (t1.kind, t1.text, t2.kind, t2.text);
                }
            }
        }

    }
}
