using System.Collections.Immutable;
using Lex.CodeAnalysis.Binding;
using Lex.CodeAnalysis.Syntax;
using Lex.CodeAnalysis.Text;

namespace Lex.CodeAnalysis.Lowering
{
    internal sealed class Lowerer : BoundTreeRewriter
    {
        private Lowerer()
        {
            
        }

        public static BoundStatement Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            return lowerer.RewriteStatement(statement);
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