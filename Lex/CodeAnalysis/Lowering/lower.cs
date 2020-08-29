using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lex.CodeAnalysis.Binding;
using Lex.CodeAnalysis.Syntax;
using Lex.CodeAnalysis.Text;

namespace Lex.CodeAnalysis.Lowering
{
    internal sealed class Lowerer : BoundTreeRewriter
    {
        private int _labelCount;
        private Lowerer()
        {
            
        }
        private LabelSymbol GenarateLabel()
        {
            var name = $"Label{++_labelCount}";
            return new LabelSymbol(name);
        }

        public static BoundBlockStatemnet Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            var result = lowerer.RewriteStatement(statement);
            return Flatten(result);
        }
        private static BoundBlockStatemnet Flatten(BoundStatement statement)
        {
            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var stack = new Stack<BoundStatement>();
            stack.Push(statement);

            while(stack.Count > 0)
            {
                var current = stack.Pop();

                if(current is BoundBlockStatemnet block)
                {
                    foreach(var s in block.Statements.Reverse())
                        stack.Push(s);
                }
                else
                {
                    builder.Add(current);

                }
            }
            return new BoundBlockStatemnet(builder.ToImmutable());
        }

        protected override BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
        
            if( node.ElseStatement == null)
            {
            // if <condition>
            //      then

            // 
            // gotoIfFalse <condition> end
            //      then
            // end:
            // 
            //             
                var endLable = GenarateLabel();
                var gotoFalse = new BoundConditionalGotoStatment(endLable,node.Condition, true);
                var endLabelStatment = new BoundLabelStatement(endLable);
                var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(gotoFalse,node.ThenStatement,endLabelStatment));
                return RewriteStatement(result);
            }
            else
            {
            // if <condition>
            //      then
            // else
            //      then
            // gotoIfFalse <condition> else
            //      then
            // goto end
            // else:
            //     else
            //end
                var endLable = GenarateLabel();
                var elseLabel = GenarateLabel();
                var gotoFalse = new BoundConditionalGotoStatment(elseLabel,node.Condition, true);
                var gotoEndStatemnet = new BoundGotoStatment(endLable);
                var elseLabelStatment = new BoundLabelStatement(elseLabel);
                var endLabelStatment = new BoundLabelStatement(endLable);
                var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(
                    gotoFalse,
                    node.ThenStatement,
                    gotoEndStatemnet,
                    elseLabelStatment,
                    node.ElseStatement,
                    endLabelStatment
                    )
                );
                 return RewriteStatement(result);
            }    
        
        }

        protected override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            //while <condition>
            //  body
            //
            //goto check
            // continue:
            //      body
            //check:
            //  gotoTrue condition continue
            // end:

            var endLable = GenarateLabel();
            var checkLabel = GenarateLabel();
            var continueLabel = GenarateLabel();

            var gotoCheck = new BoundGotoStatment(checkLabel);
            var continueLabelStatement = new BoundLabelStatement(continueLabel);
            var checkLableStatement =  new BoundLabelStatement(checkLabel);
            var gotoTrue = new BoundConditionalGotoStatment(checkLabel,node.Condition,false);
            var endLabelStatement = new BoundLabelStatement(endLable);

            var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(
                    gotoCheck,
                    continueLabelStatement,
                    node.Body,
                    checkLableStatement,
                    gotoTrue,
                    endLabelStatement
                    )
                );
            return RewriteStatement(result);
                
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {
            //for <var> = <lower> to <upper>
            // <body>
            //---/
            // {
            //  var <var> = <lower>
            //  while  (<var> <= <upper>)
            //  {
            //      <body>
            //      <var> = <var> + 1
            //  }
            //
            //
            //}
        
            var variableDeclaration  = new BoundVeriableDeclaration(node.Variable,node.LowerBound);
            var variableExpression = new BoundVariableExpression(node.Variable);
            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(SyntaxKind.LessOrEqualToken, typeof(int), typeof(int)),
                node.UpperBound
            );
            var increment = new BoundExpressionStatemnet( 
                new BoundAssignmentExpression(node.Variable,
                    new BoundBinaryExpression(
                        variableExpression,
                        BoundBinaryOperator.Bind(SyntaxKind.PlusToken,typeof(int),typeof(int)),
                        new BoundLiteralExpression(1)
                    )
                )
            );

            var whilebody = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(node.Body,increment));
            var whileStatement = new BoundWhileStatement(condition,whilebody);
            var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(variableDeclaration,whileStatement));
            return RewriteStatement(result);
        
        }   
      
    }

    
}