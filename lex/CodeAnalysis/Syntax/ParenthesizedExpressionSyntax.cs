using System.Collections.Generic;

namespace lex.CodeAnalysis.Syntax
{
    sealed class ParenthesizedExpressionSyntax : BoundExpression
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParanthesizedToken, BoundExpression expression, SyntaxToken closeParanthesizedToken)
        {
            OpenParanthesizedToken = openParanthesizedToken;
            Expression = expression;
            CloseParanthesizedToken = closeParanthesizedToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
        public SyntaxToken OpenParanthesizedToken { get; }
        public BoundExpression Expression { get; }
        public SyntaxToken CloseParanthesizedToken { get; }



        public override IEnumerable<SyntaxNode> getChildern()
        {
            yield return OpenParanthesizedToken;
            yield return Expression;
            yield return CloseParanthesizedToken;
        }
    }
}


