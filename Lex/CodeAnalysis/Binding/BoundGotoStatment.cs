namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundGotoStatment : BoundStatement
    {
    

        public BoundGotoStatment(LabelSymbol label)
        {
            Label = label;
        }

        public override BoundNodeKind Kind => BoundNodeKind.GotoStatment;

        public LabelSymbol Label { get; }
    }
}
