namespace Lex.CodeAnalysis.Syntax
{
    // for i =1 to 10

    public sealed class ForStatementSyntax: StatementSyntax
    {
        public ForStatementSyntax(SyntaxToken identifier, SyntaxToken equalsToken, ExpressionSyntax lowerBound,ExpressionSyntax upperBoud)
        {
            Identifier = identifier;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            UpperBoud = upperBoud;
        }

        public override SyntaxKind Kind => SyntaxKind.ForStatement;

        public SyntaxToken Identifier { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax LowerBound { get; }
        public ExpressionSyntax UpperBoud { get; }

        
    }
}