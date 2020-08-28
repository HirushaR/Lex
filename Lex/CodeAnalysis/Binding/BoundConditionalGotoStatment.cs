namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundConditionalGotoStatment : BoundStatement
    {
    

        public BoundConditionalGotoStatment(LabelSymbol label,BoundExpression condition,bool jumpIfFales =false)
        {
            Label = label;
            
        }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatment;

        public LabelSymbol Label { get; }
    }
}
