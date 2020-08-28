namespace Lex.CodeAnalysis.Binding
{
    internal sealed class BoundVeriableDeclaration : BoundStatement
    {
        public BoundVeriableDeclaration(VariableSymble variable, BoundExpression initializer)
        {
            Variable = variable;
            Initializer = initializer;
        }

        public override BoundNodeKind Kind => BoundNodeKind.VariableDeclaration;
        public VariableSymble Variable { get; }
        public BoundExpression Initializer { get; }

    }
}
