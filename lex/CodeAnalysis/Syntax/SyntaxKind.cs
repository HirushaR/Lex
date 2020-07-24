namespace lex.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        //Tokens
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParanthesisToken,
        CloseParanthesisToken,
        BadToken,
        EndOfFileToken,

        //Expression
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,
        LiteralExpression
    }
}


