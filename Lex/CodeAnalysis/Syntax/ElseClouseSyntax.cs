namespace Lex.CodeAnalysis.Syntax
{
    public sealed class ElseClouseSyntax : SyntaxNode
    {
        public ElseClouseSyntax(SyntaxToken elseKeyword, StatementSyntax elseStatement)
        {
            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;
        }

        public override SyntaxKind Kind => SyntaxKind.ElseClouse;

        public SyntaxToken ElseKeyword { get; }
        public StatementSyntax ElseStatement { get; }
    }
}