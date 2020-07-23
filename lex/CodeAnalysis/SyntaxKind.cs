namespace lex.CodeAnalysis
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
        ParenthesizedExpression
    }
}


