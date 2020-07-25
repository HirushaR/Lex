using System.Collections.Generic;

namespace lex.CodeAnalysis.Syntax
{
    sealed class BinaryExpressionSyntax : BoundExpression
    {
        public BinaryExpressionSyntax(BoundExpression left, SyntaxToken operatorToken, BoundExpression right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public BoundExpression Left { get; }
        public SyntaxToken OperatorToken { get; }
        public BoundExpression Right { get; }

        public override IEnumerable<SyntaxNode> getChildern()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}


