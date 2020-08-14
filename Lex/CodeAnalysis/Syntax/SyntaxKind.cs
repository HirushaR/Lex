namespace Lex.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        StarStarToken,
        SlashToken,
        RemainderToken,
        LessThanToken,
        GreaterThanToken,
        LessOrEqualToken,
        GreaterOrEqualToken,
        EaqlesToken,
        BangToken,
        EaqulesEaqlesToken,
        BangEaqlesToken,
        AmpersandAmpersandToken,
        PipePieToken,
        OpenParenthesisToken,
        CloseParenthesisToken,      
        OpenBraceToken,
        CloseBraceToken,
        IdentifierToken,


        //keywords
        ElseKeyword,
        FalseKeyword,
        ForKeyword,
        ToKeyword,
        TrueKeyword,
        IfKeyword,
        LetKeyword,
        
        VarKeyword,
        WhileKeyword,
              
        

        CompilationUnit,
        
        ElseClouse,

        //Statements
        BlockStatement,
        ExpressionStatemnet,
        ForStatement,
        IfStatement,
        VeriableDeclaration,
        WhileStatement,


        // Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        AssigmentExpression,
        
    }
}