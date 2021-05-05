using System.Collections.Immutable;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundProgram
    {
        public BoundProgram(BoundGlobalScope globalScope, DiagnosticBag diagnostics, ImmutableDictionary<FunctionSymbol, BoundBlockStatemnet> functionBodies)
        {
            GlobalScope = globalScope;
            Diagnostics = diagnostics;
            FunctionBodies = functionBodies;
        }

        public BoundGlobalScope GlobalScope { get; }
        public DiagnosticBag Diagnostics { get; }
        public ImmutableDictionary<FunctionSymbol, BoundBlockStatemnet> FunctionBodies { get; }
    }
}
