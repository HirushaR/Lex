namespace Lex.CodeAnalysis.Symbols
{
    public sealed class ParameterSymbole : Symbol
    {
        public ParameterSymbole(string name)
            :base(name)
        {
            
        }
        public override SymbolKind kind => SymbolKind.Parameter;
    }

}