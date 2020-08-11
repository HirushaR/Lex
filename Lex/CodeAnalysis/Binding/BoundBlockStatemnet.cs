using System.Collections.Immutable;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundBlockStatemnet : BoundStatement
    {
        public BoundBlockStatemnet(ImmutableArray<BoundStatement> statements)
        {
            Statements = statements;
        }

        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;
        public ImmutableArray<BoundStatement> Statements { get; }

       
    }
}
