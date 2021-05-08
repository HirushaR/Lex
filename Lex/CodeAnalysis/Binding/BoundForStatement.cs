using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundForStatement : BoundLoopStatement
    {
         public BoundForStatement(VariableSymble variable, BoundExpression lowerBound, BoundExpression upperBound,BoundExpression itterator, BoundStatement body, BoundLabel bodyLabel, BoundLabel breakLabel, BoundLabel continueLabel)
            : base(body, bodyLabel,breakLabel, continueLabel)
        {
            Variable = variable;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Itterator = itterator;
            
        }

        public override BoundNodeKind Kind => BoundNodeKind.ForStatement;
        public VariableSymble Variable { get; }
        public BoundExpression LowerBound { get; }
        public BoundExpression UpperBound { get; }
        public BoundExpression Itterator { get; }

    }
}
