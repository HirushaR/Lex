namespace Lex.CodeAnalysis.Symbols
{
    public class ParameterSymbole : VariableSymble
    {
        public ParameterSymbole(string name, TypeSymbol type)
            : base(name, isreadOnly:true,type)
        {
            
        }
        public override SymbolKind kind => SymbolKind.Parameter;
    }

}