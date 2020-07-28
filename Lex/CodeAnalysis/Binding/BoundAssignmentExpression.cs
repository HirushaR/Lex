using System;


namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {


        public BoundAssignmentExpression(VariableSymble variable, BoundExpression expression)
        {

            Variable = variable;
            Expression = expression;
        }

        public override Type Type => Expression.Type;
        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public VariableSymble Variable { get; }
        public BoundExpression Expression { get; }
    }
}
