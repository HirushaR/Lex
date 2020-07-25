using System.Collections.Generic;

namespace lex.CodeAnalysis.Syntax
{
    sealed class UnaryExpressionSyntax : BoundExpression
    { 
        public UnaryExpressionSyntax(SyntaxToken operatorToken, BoundExpression operand)
        {
    
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

        public SyntaxToken OperatorToken { get; }
        public BoundExpression Operand { get; }

        public override IEnumerable<SyntaxNode> getChildern()
        {
            yield return OperatorToken;
            yield return Operand;

        }
    }
}


