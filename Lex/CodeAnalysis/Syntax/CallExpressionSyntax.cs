namespace Lex.CodeAnalysis.Syntax
{
    public sealed class CallExpressionSyntax : ExpressionSyntax
    {
        public CallExpressionSyntax(SyntaxToken identifier,SyntaxToken openParanthesis, SeparatedSyntaxList<ExpressionSyntax> arguments,SyntaxToken closeParanthesis)
        {
            Identifier = identifier;
            OpenParanthesis = openParanthesis;
            Arguments = arguments;
            CloseParanthesis = closeParanthesis;
        }
        public override SyntaxKind Kind => SyntaxKind.CallExpression;

        public SyntaxToken Identifier { get; }
        public SyntaxToken OpenParanthesis { get; }
        public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        public SyntaxToken CloseParanthesis { get; }
    }
}