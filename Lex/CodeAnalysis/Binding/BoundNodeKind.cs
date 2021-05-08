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
        CallExpression,
        LiteralExpression,
        UnaryExpression,
        AssignmentExpression,
        BinaryExpression,
        VariableExpression,
        ErrorExpression,
        ConversionExpression,
        ReturnStatement,
    }
}
