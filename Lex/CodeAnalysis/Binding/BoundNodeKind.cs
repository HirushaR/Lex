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
        LabelStatement,
        GotoStatment,
        ConditionalGotoStatment,
        WhileStatement,

        //Expression
        LiteralExpression,
        UnaryExpression,
        AssignmentExpression,
        BinaryExpression,
        VariableExpression,
       
    }
}
