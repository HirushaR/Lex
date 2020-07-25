
using lex.CodeAnalysis.Syntax;
using lex.CodeAnalysis.Binding;
using System;
using BoundExpression = lex.CodeAnalysis.Binding.BoundExpression;

namespace lex.CodeAnalysis
{

     sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }



        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression node)
        {
            // BinaryExpression
            //NumberExpression

            if (node is BoundLiteralExpression n)
                return (int)n.Value;

            if( node is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
                    default:
                        throw new Exception($"Unexpected Unary operator {u.OperatorKind.Kind}");
                }
            }

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                if (b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                else if (b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                else if (b.OperatorToken.Kind == SyntaxKind.StarToken)
                    return left * right;
                else if (b.OperatorToken.Kind == SyntaxKind.SlashToken)
                    return left / right;
                else
                    throw new Exception($"Unexpected binary Operator {b.OperatorToken.Kind}");
            }

            if (node is ParenthesizedExpressionSyntax p)
                return EvaluateExpression(p.Expression);

            throw new Exception($"Unexpedcted node {node.Kind}");

        }
    }
}


