namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundConditionalGotoStatment : BoundStatement
    {
    

        public BoundConditionalGotoStatment(LabelSymbol label,BoundExpression condition,bool jumpIfFales =false)
        {
            Label = label;
            Condition = condition;
            JumpIfFales = jumpIfFales;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatment;

        public LabelSymbol Label { get; }
        public BoundExpression Condition { get; }
        public bool JumpIfFales { get; }
    }
}
