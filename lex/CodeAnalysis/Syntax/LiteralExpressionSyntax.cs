using System.Collections.Generic;

namespace lex.CodeAnalysis.Syntax
{
    sealed class LiteralExpressionSyntax : BoundExpression
    {
        public LiteralExpressionSyntax(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public SyntaxToken NumberToken { get; }

        public override IEnumerable<SyntaxNode> getChildern()
        {
            yield return NumberToken;
        }
    }
}


