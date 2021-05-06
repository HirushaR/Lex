using System.Collections.Immutable;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundProgram
    {
      public BoundProgram(ImmutableArray<Diagnostic> diagnostics, ImmutableDictionary<FunctionSymbol, BoundBlockStatemnet> functions, BoundBlockStatemnet statement)
        {
       
            Diagnostics = diagnostics;
           
            Functions = functions;
            Statement = statement;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ImmutableDictionary<FunctionSymbol, BoundBlockStatemnet> Functions { get; }
        public BoundBlockStatemnet Statement { get; }
    }
}

