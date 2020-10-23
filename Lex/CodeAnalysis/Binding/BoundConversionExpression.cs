using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundConversionExpression : BoundExpression
    {
        public BoundConversionExpression(TypeSymbol type, BoundExpression expression)
        {
            Type = type;
            Expression = expression;
        }

        public override TypeSymbol Type { get; }

        public override BoundNodeKind Kind => BoundNodeKind.ConversionExpression;

        public BoundExpression Expression { get; }
    }
}
