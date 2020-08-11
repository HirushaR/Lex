namespace Lex.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        //Statement
        BlockStatement,
        ExpressionStatement,

        //Expression
        LiteralExpression,
        UnaryExpression,
        AssignmentExpression,
        BinaryExpression,
        VariableExpression,
      
    }
}
