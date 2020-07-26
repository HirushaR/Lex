using lex.CodeAnalysis.Syntax;
using System;

namespace lex.CodeAnalysis.Binding
{

    internal sealed class BoundUnaryOperator
    {


        public BoundUnaryOperator(SyntaxKind syntaxkind, BoundUnaryOperatorKind kind, Type OperandType)
            : this(syntaxkind,kind,OperandType,OperandType)
        {
            
        }

        public BoundUnaryOperator(SyntaxKind syntaxkind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            Syntaxkind = syntaxkind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        public SyntaxKind Syntaxkind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }
    }

    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right)
        {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Left.Type;
        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression Right { get; }
    }
}
