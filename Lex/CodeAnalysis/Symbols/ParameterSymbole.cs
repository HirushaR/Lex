namespace Lex.CodeAnalysis.Symbols
{
    public sealed class ParameterSymbol : LocalVariableSymbol
    {
        public ParameterSymbol(string name, TypeSymbol type)
            : base(name, isReadOnly:true,type)
        {
            
        }
        public override SymbolKind kind => SymbolKind.Parameter;
    }

}