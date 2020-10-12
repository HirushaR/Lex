namespace Lex.CodeAnalysis.Symbols
{


    public sealed class TypeSymbol : Symbol
    {
        public static readonly TypeSymbol Error = new TypeSymbol("?");
        public static readonly TypeSymbol Int = new TypeSymbol("int");
        public static readonly TypeSymbol Bool = new TypeSymbol("bool");
        public static readonly TypeSymbol String = new TypeSymbol("string");
        private TypeSymbol(string name)
        : base(name)
        {

        }

        public override SymbolKind kind => SymbolKind.Type;
    }

}