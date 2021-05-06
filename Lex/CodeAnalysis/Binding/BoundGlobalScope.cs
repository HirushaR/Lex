using System.Collections.Immutable;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundGlobalScope
    {
         public BoundGlobalScope(BoundGlobalScope previous, ImmutableArray<Diagnostic> diagnostics, ImmutableArray<FunctionSymbol> functions, ImmutableArray<VariableSymble> variables, ImmutableArray<BoundStatement> statements)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            Functions = functions;
            Variables = variables;
            Statements = statements;
        }

        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ImmutableArray<FunctionSymbol> Functions { get; }
        public ImmutableArray<VariableSymble> Variables { get; }
        public ImmutableArray<BoundStatement> Statements { get; }
    }
}

       
