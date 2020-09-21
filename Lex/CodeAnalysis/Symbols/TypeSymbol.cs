namespace Lex.CodeAnalysis.Symbols
{
    public sealed class TypeSymbol : Symbol
    {
        internal TypeSymbol(string name)
        : base(name)
        {

        }

        public override SymbolKind kind => SymbolKind.Type;
    }

}