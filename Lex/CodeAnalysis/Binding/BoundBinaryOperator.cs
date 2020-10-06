using Lex.CodeAnalysis.Symbols;
using Lex.CodeAnalysis.Syntax;
using System;

namespace Lex.CodeAnalysis.Binding
{

        internal sealed class BoundBinaryOperator
        {
         private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, TypeSymbol type)
            : this(syntaxKind, kind, type, type, type)
        {
        }

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, TypeSymbol operandType, TypeSymbol resultType)
            : this(syntaxKind, kind, operandType, operandType, resultType)
        {
        }

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, TypeSymbol leftType, TypeSymbol rightType, TypeSymbol resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public TypeSymbol LeftType { get; }
        public TypeSymbol RightType { get; }
        public TypeSymbol Type { get; }
        private static BoundBinaryOperator[] _operator =
            {
            new BoundBinaryOperator(SyntaxKind.PlusToken,BoundBinaryOperatorKind.Addition, TypeSymbol.Int),
            new BoundBinaryOperator(SyntaxKind.MinusToken,BoundBinaryOperatorKind.Subtraction, TypeSymbol.Int),
            new BoundBinaryOperator(SyntaxKind.StarToken,BoundBinaryOperatorKind.Multiplication, TypeSymbol.Int),
            new BoundBinaryOperator(SyntaxKind.StarStarToken,BoundBinaryOperatorKind.Power, TypeSymbol.Int),
            new BoundBinaryOperator(SyntaxKind.SlashToken,BoundBinaryOperatorKind.Division, TypeSymbol.Int),
            new BoundBinaryOperator(SyntaxKind.RemainderToken,BoundBinaryOperatorKind.Remainder, TypeSymbol.Int),
            
            new BoundBinaryOperator(SyntaxKind.AmpersandToken,BoundBinaryOperatorKind.BitwiseAnd, TypeSymbol.Int),
            new BoundBinaryOperator(SyntaxKind.PipeToken,BoundBinaryOperatorKind.BitwiseOr, TypeSymbol.Int),
            new BoundBinaryOperator(SyntaxKind.HatToken,BoundBinaryOperatorKind.BitwiseXor, TypeSymbol.Int),

            new BoundBinaryOperator(SyntaxKind.GreaterThanToken,BoundBinaryOperatorKind.GreaterThan, TypeSymbol.Int, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.LessThanToken,BoundBinaryOperatorKind.LessThan, TypeSymbol.Int, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.EaqulesEaqlesToken,BoundBinaryOperatorKind.Equals, TypeSymbol.Int, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.BangEaqlesToken,BoundBinaryOperatorKind.NotEquals, TypeSymbol.Int, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.LessOrEqualToken,BoundBinaryOperatorKind.LessOrEqual, TypeSymbol.Int, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.GreaterOrEqualToken,BoundBinaryOperatorKind.GreaterOrEqual, TypeSymbol.Int, TypeSymbol.Bool),

            new BoundBinaryOperator(SyntaxKind.AmpersandToken,BoundBinaryOperatorKind.BitwiseAnd, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken,BoundBinaryOperatorKind.LogicalAnd, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.GreaterThanToken,BoundBinaryOperatorKind.GreaterThan, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.LessThanToken,BoundBinaryOperatorKind.LessThan, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.PipeToken,BoundBinaryOperatorKind.BitwiseOr, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.PipePieToken,BoundBinaryOperatorKind.LogicalOr, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.HatToken,BoundBinaryOperatorKind.BitwiseXor, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.EaqulesEaqlesToken,BoundBinaryOperatorKind.Equals, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.BangEaqlesToken,BoundBinaryOperatorKind.NotEquals, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.LessOrEqualToken,BoundBinaryOperatorKind.LessOrEqual, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.GreaterOrEqualToken,BoundBinaryOperatorKind.GreaterOrEqual, TypeSymbol.Bool),
            new BoundBinaryOperator(SyntaxKind.PlusToken,BoundBinaryOperatorKind.Addition, TypeSymbol.String),
        };

            public static BoundBinaryOperator Bind(SyntaxKind syntaxkind, TypeSymbol leftType, TypeSymbol rightType)
            {
                foreach (var op in _operator)
                {
                    if (op.SyntaxKind == syntaxkind && op.LeftType == leftType && op.RightType == rightType)
                        return op;
                }
                return null;

            }

        }
    
}
