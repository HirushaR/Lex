using System;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator op, BoundExpression operand)
        {
            
            this.Op = op;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;

        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }

        public override TypeSymbol Type =>Op.Type ;
    }
}
