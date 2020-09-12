using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lex.CodeAnalysis.Text;

namespace Lex.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        //, ImmutableArray<Diagnostic> diagnostics, CompilationUnitSyntax root
        private SyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            var root = parser.ParseCompilationUnit();
            var diagnostics = parser.Diagnostics.ToImmutableArray();
            
            Text = text;
            Diagnostics = diagnostics;
            Root = root;

        }

        public SourceText Text { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public CompilationUnitSyntax Root { get; }

        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
           return new SyntaxTree(text);
        }

        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
           var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);           
        }

        public static IEnumerable<SyntaxToken> ParseTokens(SourceText text)
        {

            return ParseTokens(text, out _);
        }

        public static IEnumerable<SyntaxToken> ParseTokens(SourceText text,out ImmutableArray<Diagnostic> diagnostics)
        {
            IEnumerable<SyntaxToken> LexTokens(Lexer lexer)
            {
                while(true)
                {
                    var token = lexer.Lex();
                    if (token.Kind == SyntaxKind.EndOfFileToken)
                        break;


                    yield return token;
                }
            }
            var l = new Lexer(text);
            var result = LexTokens(l);
            diagnostics = l.Diagnostics.ToImmutableArray();
            return result;
            
        }
    }
}