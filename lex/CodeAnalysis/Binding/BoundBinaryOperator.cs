﻿using lex.CodeAnalysis.Syntax;
using System;

namespace lex.CodeAnalysis.Binding
{

    internal sealed partial class BoundBinaryExpression
    {
        internal sealed class BoundBinaryOperator
        {
            public BoundBinaryOperator(SyntaxKind syntaxkind, BoundBinaryOperatorKind kind, Type type)
                :this(syntaxkind, kind,type,type,type)
            {

            }

            public BoundBinaryOperator(SyntaxKind syntaxkind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
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
            new BoundBinaryOperator(SyntaxKind.SlashToken,BoundBinaryOperatorKind.Division, typeof(int)),

            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken,BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePieToken,BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
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
}