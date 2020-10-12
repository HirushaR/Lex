using System.Collections.Immutable;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundCallExpression : BoundExpression
    {
        public BoundCallExpression(FunctionSymbol function, ImmutableArray<BoundExpression> argument)
        {
            Function = function;
            Argument = argument;
        }
        public override TypeSymbol Type =>Function.Type;

        public override BoundNodeKind Kind => BoundNodeKind.CallExpression;

        public FunctionSymbol Function { get; }
        public ImmutableArray<BoundExpression> Argument { get; }
    }
}
