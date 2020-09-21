using Lex.CodeAnalysis.Symbols;
using Lex.CodeAnalysis.Syntax;
using System;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {


        public BoundUnaryOperator(SyntaxKind syntaxkind, BoundUnaryOperatorKind kind, TypeSymbol OperandType)
            : this(syntaxkind,kind,OperandType,OperandType)
        {
            
        }

        public BoundUnaryOperator(SyntaxKind syntaxkind, BoundUnaryOperatorKind kind, TypeSymbol operandType, TypeSymbol resultType)
        {
            Syntaxkind = syntaxkind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        public SyntaxKind Syntaxkind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public TypeSymbol OperandType { get; }
        public TypeSymbol ResultType { get; }
        public TypeSymbol Type { get; internal set; }

        private static BoundUnaryOperator[] _operator =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken,BoundUnaryOperatorKind.LogicalNegation, TypeSymbol.Int),
            new BoundUnaryOperator(SyntaxKind.PlusToken,BoundUnaryOperatorKind.Identity, TypeSymbol.Int),
            new BoundUnaryOperator(SyntaxKind.MinusToken,BoundUnaryOperatorKind.Negation, TypeSymbol.Int),
            new BoundUnaryOperator(SyntaxKind.TildToken,BoundUnaryOperatorKind.OnceComplement, TypeSymbol.Int),
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxkind,TypeSymbol operandType)
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
