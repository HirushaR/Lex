namespace lex.CodeAnalysis
{
    internal sealed partial class Parser
    {
        internal static class SyntaxFacts
        {

            // Mathematical operator
            public static int GetBinaryOperatorPrecedence(SyntaxKind kind)
            {
                switch (kind)
                {
                    case SyntaxKind.StarToken:
                    case SyntaxKind.SlashToken:
                        return 2;
                    case SyntaxKind.PlusToken:
                    case SyntaxKind.MinusToken:
                        return 1;
                    default:
                        return 0;
                }
            }
        }
    }
}


