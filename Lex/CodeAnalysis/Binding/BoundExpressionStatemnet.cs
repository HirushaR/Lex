namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundExpressionStatemnet : BoundStatement
    {
        public BoundExpressionStatemnet(BoundExpression expression)
        {
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;
        public BoundExpression Expression { get; }

       
    }
}
