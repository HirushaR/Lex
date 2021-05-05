// using Lex.CodeAnalysis.Binding;

namespace Lex.CodeAnalysis.Syntax
{
    public sealed class VeriableDeclarationSyntax : StatementSyntax
    {
        public VeriableDeclarationSyntax(SyntaxToken identifier, TypeClauseSyntax typeClause,  SyntaxToken equalsToken,ExpressionSyntax initializer)
        {

            Identifier = identifier;
            TypeClause = typeClause;
            EqualsToken = equalsToken;
            Initializer = initializer;
            
        }
        public override SyntaxKind Kind => SyntaxKind.VeriableDeclaration;
        public SyntaxToken Identifier { get; }
        public TypeClauseSyntax TypeClause { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax Initializer { get; }
    }
}