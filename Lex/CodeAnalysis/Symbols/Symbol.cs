namespace Lex.CodeAnalysis.Symbols
{
    public abstract class Symbol
    {
        private protected Symbol(string name)
        {
            Name = name;
        }

        public abstract SymbolKind kind {get;}
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }



}