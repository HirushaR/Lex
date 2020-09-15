namespace Lex.CodeAnalysis.Syntax
{
    public sealed class VeriableDeclarationSyntax : StatementSyntax
    {
        public VeriableDeclarationSyntax(SyntaxToken identifier, SyntaxToken equalsToken,ExpressionSyntax initializer)
        {

            Identifier = identifier;
            EqualsToken = equalsToken;
            Initializer = initializer;
        }
        public override SyntaxKind Kind => SyntaxKind.VeriableDeclaration;
        public SyntaxToken Identifier { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }
    }
}