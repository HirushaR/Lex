namespace Lex.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        //Statement
        BlockStatement,
        VariableDeclaration,
        ExpressionStatement,
        IfStatement,
        WhileStatement,

        //Expression
        LiteralExpression,
        UnaryExpression,
        AssignmentExpression,
        BinaryExpression,
        VariableExpression,
       
    }
}
