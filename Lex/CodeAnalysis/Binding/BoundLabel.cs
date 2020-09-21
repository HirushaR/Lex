namespace Lex.CodeAnalysis.Binding
{
    public sealed class BoundLabel
    {
        public BoundLabel(string name)
        {
            Name = name;
        
        }

        public string Name { get; }
        public override string ToString() => Name;

    }

}