namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundConditionalGotoStatment : BoundStatement
    {
    

        public BoundConditionalGotoStatment(BoundLabel label,BoundExpression condition,bool jumpIfTrue =true)
        {
            Label = label;
            Condition = condition;
            JumpIfTrue = jumpIfTrue;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatment;

        public BoundLabel Label { get; }
        public BoundExpression Condition { get; }
        public bool JumpIfTrue { get; }
    }
}
