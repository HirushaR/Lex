using System;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;
            if(Value is bool)
                Type = TypeSymbol.Bool;
            else if ( Value is int)
                Type = TypeSymbol.Int;
            else if ( Value is string)
                Type = TypeSymbol.String;
            else 
                throw new Exception($"Unexpected literal '{Value}' of type {value.GetType()}.");
        }

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;

        public object Value { get; }

        public override TypeSymbol Type {get;}
    }
}
