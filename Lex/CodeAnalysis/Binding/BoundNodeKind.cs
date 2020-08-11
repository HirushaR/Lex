namespace Lex.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        //Statement
        BlockStatement,
        VariableDeclaration,
        ExpressionStatement,

        //Expression
        LiteralExpression,
        UnaryExpression,
        AssignmentExpression,
        BinaryExpression,
        VariableExpression,
      
    }
}
