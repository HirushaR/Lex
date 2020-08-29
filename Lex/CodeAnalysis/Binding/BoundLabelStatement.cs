namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundLabelStatement : BoundStatement
    {
        public BoundLabelStatement(LabelSymbol label)
        {
            Label = label;
        }

        public LabelSymbol Label { get; }

        public override BoundNodeKind Kind => BoundNodeKind.LabelStatement;
    }
}
