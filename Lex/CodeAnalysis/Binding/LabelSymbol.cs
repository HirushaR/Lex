namespace Lex.CodeAnalysis.Binding
{
    public sealed class LabelSymbol
    {
        public LabelSymbol(string name)
        {
            Name = name;
        
        }

        public string Name { get; }
        public override string ToString() => Name;

    }

}