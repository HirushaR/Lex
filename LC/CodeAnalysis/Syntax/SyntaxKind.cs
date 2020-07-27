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
        SlashToken,
        BangToken,
        EaqulesEaqlesToken,
        BangEaqlesToken,
        AmpersandAmpersandToken,
        PipePieToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,


        //keywords
        TrueKeyword,
        FalseKeyword,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,

    }
}