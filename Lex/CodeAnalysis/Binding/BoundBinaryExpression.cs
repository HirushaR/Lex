using System;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{

    internal sealed partial class BoundBinaryExpression : BoundExpression
    {

        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator op, BoundExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;

        public override TypeSymbol Type => Op.Type;
        public BoundExpression Left { get; }
        public BoundBinaryOperator Op { get; }
        public BoundExpression Right { get; }

        
    }
}
