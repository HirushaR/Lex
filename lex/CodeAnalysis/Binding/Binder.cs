using lex.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace lex.CodeAnalysis.Binding
{

    internal enum BoundNodeKind
    {
        LiteralExpression,
        UnaryExpression
    }
    internal abstract class BoundNode
    {
        public abstract BoundNodeKind Kind { get; }
    }

    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }

    internal enum BoundUnaryOperatorKind
    {
        Identity,
        Negation
    }

    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public override Type Type => Value.GetType();

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    }

    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperatorKind operatorKind,BoundExpression operand)
        {
            OperatorKind = operatorKind;
            Operand = operand;
        }

        public BoundUnaryOperatorKind OperatorKind { get; }
        public BoundExpression Operand { get; }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;

        public override Type Type => Operand.Type;
    }

    internal enum BoundBinaryOperatorKind
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right)
        {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
            Left = left;
        }

        public override Type Type => Left.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;

        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression Right { get; }

    }

    internal sealed class Binder
    {
        public BoundExpression Bind(ExpressionSyntax syntax)
        {
            switch(syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }


        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.LiteralToken.Value as int? ?? 0;
            throw new BoundLiteralExpression(value);
        }
        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind);
            var boundOperand = BindExpression(syntax.Operand);

            throw new BoundUnaryExpression(boundOperatorKind, boundOperand);
        }

       
        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundleft = BindExpression(syntax.Left);
            var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind);
            var boundRight = BoundExpression(syntax.Right);
            throw new BoundBinaryExpression(boundleft, boundOperatorKind, boundRight);
        }

        private BoundUnaryOperatorKind BindUnaryOperatorKind(SyntaxKind kind)
        {
            throw new NotImplementedException();
        }


        private BoundBinaryOperatorKind BindBinaryOperatorKind(SyntaxKind kind)
        {
            throw new NotImplementedException();
        }
    }


}
