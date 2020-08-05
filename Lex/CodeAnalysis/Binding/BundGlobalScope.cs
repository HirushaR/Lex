using System.Collections.Immutable;


namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BundGlobalScope
    {
        public BundGlobalScope(BundGlobalScope previous,ImmutableArray<Diagnostic> diagnostics, ImmutableArray<VariableSymble> veriable, BoundExpression expression)
        {
            Previous = previous;
            Diagnostics = diagnostics;
            Veriable = veriable;
            Expression = expression;
        }

        public BundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ImmutableArray<VariableSymble> Veriable { get; }
        public BoundExpression Expression { get; }
    }
}
