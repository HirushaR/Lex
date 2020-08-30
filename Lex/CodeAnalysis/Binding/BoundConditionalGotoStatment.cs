namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundConditionalGotoStatment : BoundStatement
    {
    

        public BoundConditionalGotoStatment(LabelSymbol label,BoundExpression condition,bool jumpIfTrue =true)
        {
            Label = label;
            Condition = condition;
            JumpIfTrue = jumpIfTrue;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatment;

        public LabelSymbol Label { get; }
        public BoundExpression Condition { get; }
        public bool JumpIfTrue { get; }
    }
}
