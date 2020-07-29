using Lex.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
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

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var t in GetTokens())
                yield return new object[] { t.kind, t.text };
        }

        private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
        {
            return new[]
            {
                (SyntaxKind.IdentifierToken, "a"),
                (SyntaxKind.IdentifierToken, "abc"),
            };

        }
    }
}
