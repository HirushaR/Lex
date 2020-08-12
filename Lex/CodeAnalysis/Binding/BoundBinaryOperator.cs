using Lex.CodeAnalysis.Syntax;
using System;

namespace Lex.CodeAnalysis.Binding
{

        internal sealed class BoundBinaryOperator
        {
            private BoundBinaryOperator(SyntaxKind syntaxkind, BoundBinaryOperatorKind kind, Type type)
                :this(syntaxkind, kind,type,type,type)
            {

            }
            private BoundBinaryOperator(SyntaxKind syntaxkind, BoundBinaryOperatorKind kind, Type operandType,Type resultType)
                  : this(syntaxkind, kind, operandType, operandType, resultType)
            {

            }

        private BoundBinaryOperator(SyntaxKind syntaxkind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
            {
                Syntaxkind = syntaxkind;
                Kind = kind;
                LeftType = leftType;
                RightType = rightType;
                ResultType = resultType;
            }

            public SyntaxKind Syntaxkind { get; }
            public BoundBinaryOperatorKind Kind { get; }
            public Type LeftType { get; }
            public Type RightType { get; }
            public Type ResultType { get; }


            private static BoundBinaryOperator[] _operator =
            {
            new BoundBinaryOperator(SyntaxKind.PlusToken,BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken,BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarToken,BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarStarToken,BoundBinaryOperatorKind.Power, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken,BoundBinaryOperatorKind.Division, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.RemainderToken,BoundBinaryOperatorKind.Remainder, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.GreaterThanToken,BoundBinaryOperatorKind.GreaterThan, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LessThanToken,BoundBinaryOperatorKind.LessThan, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EaqulesEaqlesToken,BoundBinaryOperatorKind.Equals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEaqlesToken,BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LessOrEqualToken,BoundBinaryOperatorKind.LessOrEqual, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.GreaterOrEqualToken,BoundBinaryOperatorKind.GreaterOrEqual, typeof(int), typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken,BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.GreaterThanToken,BoundBinaryOperatorKind.GreaterThan, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LessThanToken,BoundBinaryOperatorKind.LessThan, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePieToken,BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EaqulesEaqlesToken,BoundBinaryOperatorKind.Equals, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEaqlesToken,BoundBinaryOperatorKind.NotEquals, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LessOrEqualToken,BoundBinaryOperatorKind.LessOrEqual, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.GreaterOrEqualToken,BoundBinaryOperatorKind.GreaterOrEqual, typeof(bool)),
        };

            public static BoundBinaryOperator Bind(SyntaxKind syntaxkind, Type leftType, Type rightType)
            {
                foreach (var op in _operator)
                {
                    if (op.Syntaxkind == syntaxkind && op.LeftType == leftType && op.RightType == rightType)
                        return op;
                }
                return null;

            }

        }
    
}
