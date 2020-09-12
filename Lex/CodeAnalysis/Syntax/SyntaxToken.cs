using System.Collections.Generic;
using System.Linq;
using Lex.CodeAnalysis.Text;

namespace Lex.CodeAnalysis.Syntax
{
    public sealed class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override TextSpan Span => new TextSpan(Position, Text?.Length ?? 0);

        public bool isMissing => Text == null;

      
    }
}