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


        private static BoundUnaryOperator[] _operator =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken,BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.PlusToken,BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken,BoundUnaryOperatorKind.Negation, typeof(int)),
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxkind,Type operandType)
        {
            foreach(var op in _operator)
            {
                if (op.Syntaxkind == syntaxkind && op.OperandType == operandType)
                    return op;
            }
            return null;
        }

    }
}
