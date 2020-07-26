using System;

namespace lex.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator op, BoundExpression operand)
        {
            
            this.op = op;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => op.ResultType;
        public BoundUnaryOperator op { get; }
        public BoundExpression Operand { get; }
    }
}
