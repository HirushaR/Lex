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

     
        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
        public VariableSymble Variable { get; }

        public override TypeSymbol Type => Variable.Type;
    }
}
