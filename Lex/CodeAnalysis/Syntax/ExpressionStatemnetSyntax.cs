namespace Lex.CodeAnalysis.Syntax
{
    public sealed class ExpressionStatemnetSyntax : StatementSyntax
    {
        // a = 10
        // M() 
        public ExpressionStatemnetSyntax(ExpressionSyntax expression)
        {
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.ExpressionStatemnet;
        public ExpressionSyntax Expression { get; }

        
    }
}