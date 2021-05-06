namespace Lex.CodeAnalysis.Symbols
{
    public sealed class GlobalVariableSymbol : VariableSymble
    {
        internal GlobalVariableSymbol(string name, bool isReadOnly, TypeSymbol type)
            : base(name, isReadOnly, type)
        {
        }

        public override SymbolKind kind => SymbolKind.GlobalVariable;
    }
}