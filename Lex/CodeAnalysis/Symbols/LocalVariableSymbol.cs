namespace Lex.CodeAnalysis.Symbols
{
   public class LocalVariableSymbol : VariableSymble
    {
        internal LocalVariableSymbol(string name, bool isReadOnly, TypeSymbol type)
            : base(name, isReadOnly, type)
        {
        }

        public override SymbolKind kind => SymbolKind.LocalVariable;
    }
}