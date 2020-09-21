using System;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundVariableExpression : BoundExpression
    {
        

        public BoundVariableExpression(VariableSymble variable)
        {
            Variable = variable;
        }

        public string Name { get; }
        public override Type Type => Variable.Type;
        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
        public VariableSymble Variable { get; }
    }
}
