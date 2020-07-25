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

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }

     

        
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
        }


        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Left.Type;
        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression Right { get; }

    }

    internal sealed class Binder
    {

        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpression BindExpression(Syntax.BoundExpression syntax)
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
            var value = syntax.NumberToken.Value as int? ?? 0;
            return new BoundLiteralExpression(value);
        }
        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);

            if(boundOperatorKind == null)
            {
                _diagnostics.Add($"Unary Operator '{syntax.OperatorToken.Text}' it's not design for type {boundOperand.Type}.");
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperatorKind.Value, boundOperand);
        }

      
        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundleft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind, boundleft.Type,boundRight.Type);
            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Unary Operator '{syntax.OperatorToken.Text}' it's not design for type {boundleft.Type} and {boundRight.Type}.");
                return boundleft;
            }

            return new BoundBinaryExpression(boundleft, boundOperatorKind.Value, boundRight);
        }

        private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind,Type operandType)
        {

            if (operandType != typeof(int))
                return null;
           
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return BoundUnaryOperatorKind.Identity;
                case SyntaxKind.MinusToken:
                    return BoundUnaryOperatorKind.Negation;
                default:
                    throw new Exception($"Unexpected Unary Operator {kind}");
            }
        }


        private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType,Type rightType)
        {

            if (leftType != typeof(int) || rightType != typeof(int))
                return null;
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return BoundBinaryOperatorKind.Addition;
                case SyntaxKind.MinusToken:
                    return BoundBinaryOperatorKind.Subtraction;
                case SyntaxKind.StarToken:
                    return BoundBinaryOperatorKind.Multiplication;
                case SyntaxKind.SlashToken:
                    return BoundBinaryOperatorKind.Division;
                default:
                    throw new Exception($"Unexpected Binary Operator {kind}");
            }
        }
    }


}
