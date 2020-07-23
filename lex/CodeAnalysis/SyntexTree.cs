using System.Collections.Generic;
using System.Linq;

namespace lex.CodeAnalysis
{
     sealed class SyntexTree
    {
        public SyntexTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntexTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}


