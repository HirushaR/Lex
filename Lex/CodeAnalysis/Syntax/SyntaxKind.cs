namespace Lex.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        NumberToken,
        StringToken,
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
        TildToken,
        HatToken,
        AmpersandToken,    
        AmpersandAmpersandToken,
        PipeToken,
        PipePieToken,
        OpenParenthesisToken,
        CloseParenthesisToken,      
        OpenBraceToken,
        CloseBraceToken,
         CommaToken,
        IdentifierToken,


        //keywords
        ElseKeyword,
        FalseKeyword,
        ForKeyword,
        ToKeyword,
        ByKeyWord,
        TrueKeyword,
        IfKeyword,
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
        CallExpression,
       
    }
}