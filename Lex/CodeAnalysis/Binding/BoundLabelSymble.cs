namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundLabelSymble : BoundStatement
    {
        public BoundLabelSymble(LabelSymbol label)
        {
            Label = label;
        }

        public LabelSymbol Label { get; }

        public override BoundNodeKind Kind => BoundNodeKind.LabelStatement;
    }
}
