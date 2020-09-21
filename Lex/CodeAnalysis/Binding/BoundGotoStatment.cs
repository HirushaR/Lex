namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundGotoStatment : BoundStatement
    {
    

        public BoundGotoStatment(BoundLabel label)
        {
            Label = label;
        }

        public override BoundNodeKind Kind => BoundNodeKind.GotoStatment;

        public BoundLabel Label { get; }
    }
}
