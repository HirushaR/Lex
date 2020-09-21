using System;
using System.Collections.Immutable;

namespace Lex.CodeAnalysis.Binding
{
    internal abstract class BoundTreeRewriter
    {
        public virtual  BoundStatement RewriteStatement(BoundStatement node)
        {
           switch(node.Kind)
           {
                case BoundNodeKind.BlockStatement:
                    return RewriteBlockStatement((BoundBlockStatemnet)node);
                case BoundNodeKind.ForStatement:
                    return RewriteForStatement((BoundForStatement)node);
                case BoundNodeKind.ExpressionStatement:
                    return RewriteExpressionStatement((BoundExpressionStatemnet)node);
                case BoundNodeKind.IfStatement:
                    return RewriteIfStatement((BoundIfStatement)node);
                case BoundNodeKind.LabelStatement:
                    return RewriteLabelStatement((BoundLabelStatement)node);
                case BoundNodeKind.GotoStatment:
                    return RewriteGotoStatment((BoundGotoStatment)node);
                 case BoundNodeKind.ConditionalGotoStatment:
                    return RewriteConditionalGotoStatment((BoundConditionalGotoStatment)node);
                case BoundNodeKind.VariableDeclaration:
                    return RewriteVariableDeclaration((BoundVeriableDeclaration)node);
                case BoundNodeKind.WhileStatement:
                    return RewriteWhileStatement((BoundWhileStatement)node);
                default:
                    throw new Exception($"Unexepected node : {node.Kind}");
           }
        }

        protected virtual BoundStatement RewriteConditionalGotoStatment(BoundConditionalGotoStatment node)
        {
            var condition = RewriteExpression(node.Condition);
            if(condition == node.Condition)
                return node;
            return new BoundConditionalGotoStatment(node.Label,condition,node.JumpIfTrue);
        }

        protected virtual BoundStatement RewriteGotoStatment(BoundGotoStatment node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteLabelStatement(BoundLabelStatement node)
        {
            return node;
        }

        protected virtual BoundStatement RewriteBlockStatement(BoundBlockStatemnet node)
        {
            ImmutableArray<BoundStatement>.Builder builder = null;

            for (var i = 0; i < node.Statements.Length; i++)
            {
                var oldStatement = node.Statements[i];
                var newStatement = RewriteStatement(oldStatement);
                if(newStatement != oldStatement)
                {
                    if(builder == null)
                    {
                        builder = ImmutableArray.CreateBuilder<BoundStatement>(node.Statements.Length);
                        for( var j =0; j<i ;j++)
                            builder.Add(node.Statements[j]);
                    }
                }
                if( builder != null)
                    builder.Add(newStatement);
            }
            if(builder == null)
                return node;
            
            return new BoundBlockStatemnet(builder.MoveToImmutable());
        }

        protected virtual BoundStatement RewriteForStatement(BoundForStatement node)
        {
            var lowerBound = RewriteExpression(node.LowerBound);
            var upperBound = RewriteExpression(node.UpperBound);
            var Itterator = RewriteExpression(node.Itterator);
            var body = RewriteStatement(node.Body);
            
            if( lowerBound == node.LowerBound && upperBound == node.LowerBound && body == node.Body)
                return node;
            
            return new BoundForStatement(node.Variable, lowerBound,upperBound,Itterator, body);
            
        }

        protected virtual BoundStatement RewriteExpressionStatement(BoundExpressionStatemnet node)
        {
           var expression = RewriteExpression(node.Expression);
            if(expression == node.Expression)
                return node;
            
            return new BoundExpressionStatemnet(expression);
        }

        protected virtual BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var thenStatement = RewriteStatement(node.ThenStatement);
            var elseStatement = node.ElseStatement == null ? null : RewriteStatement(node.ElseStatement);
            if(condition == node.Condition && thenStatement == node.ThenStatement && elseStatement == node.ElseStatement)
                return node;
            
            return new BoundIfStatement(condition,thenStatement,elseStatement);
        }
        protected virtual BoundStatement RewriteVariableDeclaration(BoundVeriableDeclaration node)
        {
            var initilizer = RewriteExpression(node.Initializer);
            if(initilizer == node.Initializer)
                return node;
            
            return new BoundVeriableDeclaration(node.Variable, initilizer);
        }
        protected virtual BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            var condition = RewriteExpression(node.Condition);
        
            var body = RewriteStatement(node.Body);
            
            if( condition == node.Condition && body == node.Body)
                return node;
            
            return new BoundWhileStatement(condition, body);
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
            
            return new BoundUnaryExpression(node.Op, operand);
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
