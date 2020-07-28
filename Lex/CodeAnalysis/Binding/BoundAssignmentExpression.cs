using System;


namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {


        public BoundAssignmentExpression(string name, BoundExpression expression)
        {
            Name = name;
            Expression = expression;
        }

        public override Type Type => Expression.Type;

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;

        public string Name { get; }
        public BoundExpression Expression { get; }
    }
}
