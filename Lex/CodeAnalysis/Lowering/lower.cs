using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lex.CodeAnalysis.Binding;
using Lex.CodeAnalysis.Symbols;
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

        private BoundLabel GenerateLabel()
        {
            var name = $"Label{++_labelCount}";
            return new BoundLabel(name);
        }

        public static BoundBlockStatemnet Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            var result =  lowerer.RewriteStatement(statement);
            return Flatten(result);
        }

        private static BoundBlockStatemnet Flatten(BoundStatement statement)
        {
            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var stack = new Stack<BoundStatement>();
            stack.Push(statement);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (current is BoundBlockStatemnet block)
                {
                    foreach (var s in block.Statements.Reverse())
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
            if (node.ElseStatement == null)
            {
                // if <condition>
                //      <then>
                //
                // ---->
                //
                // gotoFalse <condition> end
                // <then>  
                // end:
                var endLabel = GenerateLabel();
                var gotoFalse = new BoundConditionalGotoStatment(endLabel, node.Condition, false);
                var endLabelStatement = new BoundLabelStatement(endLabel);
                var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(gotoFalse, node.ThenStatement, endLabelStatement));
                return RewriteStatement(result);
            }
            else
            {
                // if <condition>
                //      <then>
                // else
                //      <else>
                //
                // ---->
                //
                // gotoFalse <condition> else
                // <then>
                // goto end
                // else:
                // <else>
                // end:

                var elseLabel = GenerateLabel();
                var endLabel = GenerateLabel();

                var gotoFalse = new BoundConditionalGotoStatment(elseLabel, node.Condition, false);
                var gotoEndStatement = new BoundGotoStatment(endLabel);
                var elseLabelStatement = new BoundLabelStatement(elseLabel);
                var endLabelStatement = new BoundLabelStatement(endLabel);
                var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(
                    gotoFalse,
                    node.ThenStatement,
                    gotoEndStatement,
                    elseLabelStatement,
                    node.ElseStatement,
                    endLabelStatement
                ));
                return RewriteStatement(result);
            }
        }

        protected override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            // while <condition>
            //      <bode>
            //
            // ----->
            //
            // goto check
            // continue:
            // <body>
            // check:
            // gotoTrue <condition> continue
            // end:
            //
                
            var continueLabel = GenerateLabel();
            var checkLabel = GenerateLabel();
            var endLabel = GenerateLabel();

            var gotoCheck = new BoundGotoStatment(checkLabel);
            var continueLabelStatement = new BoundLabelStatement(continueLabel);
            var checkLabelStatement = new BoundLabelStatement(checkLabel);
            var gotoTrue = new BoundConditionalGotoStatment(continueLabel, node.Condition);
            var endLabelStatement = new BoundLabelStatement(endLabel);

            var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(
                gotoCheck,
                continueLabelStatement,
                node.Body,
                checkLabelStatement,
                gotoTrue,
                endLabelStatement
            ));

            return RewriteStatement(result);
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {
            // for <var> = <lower> to <upper> by <incriment>
            //      <body>
            //
            // ---->
            //
            // {
            //      var <var> = <lower>
            //      while (<var> <= <upper>)
            //      {
            //          <body>
            //          <var> = <var> + 1
            //      }   
            // }
            


            var variableDeclaration = new BoundVeriableDeclaration(node.Variable, node.LowerBound);
            var variableExpression = new BoundVariableExpression(node.Variable);
            
            var upperBoundSybmle = new VariableSymble("upperBound",true, TypeSymbol.Int);
            var upperBoundDeclaration = new BoundVeriableDeclaration(upperBoundSybmle,node.UpperBound);

            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(SyntaxKind.LessOrEqualToken, TypeSymbol.Int, TypeSymbol.Int),
                new BoundVariableExpression(upperBoundSybmle)
            );            

            var Ittertator = node.Itterator;
            var Operator = SyntaxKind.PlusToken;
            if(Ittertator == null)
            {
                Ittertator =new BoundLiteralExpression(1);
            }
            
            var increment = new BoundExpressionStatemnet(
                new BoundAssignmentExpression(
                    node.Variable,
                    new BoundBinaryExpression(
                            variableExpression,
                            BoundBinaryOperator.Bind(Operator, TypeSymbol.Int, TypeSymbol.Int),
                            Ittertator
                    )
                )
            );
            var whileBody = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(node.Body, increment));
            var whileStatement = new BoundWhileStatement(condition, whileBody);
            var result = new BoundBlockStatemnet(ImmutableArray.Create<BoundStatement>(
                variableDeclaration,upperBoundDeclaration, whileStatement));
     
            return RewriteStatement(result);
        }
    }
    
}