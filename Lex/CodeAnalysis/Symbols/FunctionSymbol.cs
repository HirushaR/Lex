using System.Collections.Immutable;

namespace Lex.CodeAnalysis.Symbols
{
    public sealed class FunctionSymbol : Symbol
    {
        public FunctionSymbol(string name, ImmutableArray<ParameterSymbole> parameter,TypeSymbol type)
            :base(name)
        {
            Parameter = parameter;
            Type = type;
        }

        public override SymbolKind kind => SymbolKind.Function;

        public ImmutableArray<ParameterSymbole> Parameter { get; }
        public TypeSymbol Type { get; }
    }

}