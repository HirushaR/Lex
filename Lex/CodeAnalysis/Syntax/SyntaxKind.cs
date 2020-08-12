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
        TrueKeyword,
        LetKeyword,
        FalseKeyword,
        VarKeyword,

        CompilationUnit,

        //Statements
        BlockStatement,
        ExpressionStatemnet,
        VeriableDeclaration,

        // Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        AssigmentExpression,

       
    }
}