using System.Collections.Immutable;

namespace Lex.CodeAnalysis.Syntax
{
    public sealed class BlockStatementSynatx :StatementSyntax
    {
        public BlockStatementSynatx(SyntaxToken openBraceToken, ImmutableArray<StatementSyntax> statements, SyntaxToken closeBraceToken)
        {
            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;
        }
         public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public SyntaxToken OpenBraceToken { get; }
        public ImmutableArray<StatementSyntax> Statements { get; }
        public SyntaxToken CloseBraceToken { get; }
     
    }
}