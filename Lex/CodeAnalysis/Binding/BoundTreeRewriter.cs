using System;

namespace Lex.CodeAnalysis.Binding
{
    internal abstract class BoundTreeRewriter
    {

        public virtual  BoundStatement RewriteStatement(BoundStatement node)
        {
           switch(node.Kind)
           {
               
               default:
                    throw new Exception($"Unexepected node : {node.Kind}");
           }
        }

        public virtual BoundExpression RewriteExpression(BoundExpression node)
        {
            switch(node.Kind)
           {
               case BoundNodeKind.BinaryExpression:
                    return RewriteBinaryExpression((BoundBinaryExpression)node);
               case BoundNodeKind.LiteralExpression:
                    return RewriteLiteralExpression((BoundLiteralExpression)node);
               case BoundNodeKind.UnaryExpression:
                    return RewriteUnaryExpression((BoundUnaryExpression)node);
               case BoundNodeKind.VariableExpression:
                    return RewriteVariableExpression((BoundVariableExpression)node);
               case BoundNodeKind.AssignmentExpression:
                    return RewriteAssignmentExpression((BoundAssignmentExpression)node);
               
               default:
                    throw new Exception($"Unexepected node : {node.Kind}");
           }
        }

        
        protected virtual BoundExpression RewriteBinaryExpression(BoundBinaryExpression node)
        {
            var left = RewriteExpression(node.Left);
            var right = RewriteExpression(node.Right);
            if(left == node.Left && right == node.Right)
                return node;
            
            return new BoundBinaryExpression(left, node.Op, right);
        }

        protected virtual BoundExpression RewriteLiteralExpression(BoundLiteralExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteUnaryExpression(BoundUnaryExpression node)
        {
            var operand = RewriteExpression(node.Operand);
            if(operand == node.Operand)
                return node;
            
            return new BoundUnaryExpression(node.op, operand);
        }

        protected virtual BoundExpression RewriteVariableExpression(BoundVariableExpression node)
        {
            return node;
        }

        protected virtual BoundExpression RewriteAssignmentExpression(BoundAssignmentExpression node)
        {
            var expression = RewriteExpression(node.Expression);
            if(expression == node.Expression)
                return node;
            
            return new BoundAssignmentExpression(node.Variable, expression);
        }

    }
}
