namespace Lex.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        //Statement
        BlockStatement,
        ForStatement,
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
