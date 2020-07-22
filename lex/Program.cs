using System;

namespace lex
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine(">");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;
                if (line == "1 + 2 * 3")
                {
                    Console.WriteLine("7");
                }
                else
                {
                    Console.WriteLine("ERROE:invalid Expression");
                }
            }
        }
    }

    class SyntaxToken
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
    }


    class Lexer
    {
        private readonly string _text;
        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }
        private char Current
        {
            get
            {
                if(_position >= _text.Length)
                {
                    return '\0';
                }
                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        // every time we called next token we check next item and keep going
        public SyntaxToken NextToken()
        {
            //<numbers>
            //+-*?()
            //<whitespace>

            if(char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.NumberToken, start, text,value);
            }
            
            if(char.IsWhiteSpace(Current))
            {
                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, value);
            }

            if(Current == '+')
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            if (Current == '-')
                return new SyntaxToken(SyntaxKind.MinusToke, _position++, "-", null);
            if (Current == '*')
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            if (Current == '/')
                return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
            if (Current == '(')
                return new SyntaxToken(SyntaxKind.OpenParanthesisToken, _position++, "(", null);
            if (Current == ')')
                return new SyntaxToken(SyntaxKind.CloseParanthesisToken, _position++, ")", null);

            return new SyntaxToken(SyntaxKind.BadToken, _position, _text.Substring(_position - 1, 1), null);

        }
    }

    enum SyntaxKind
    {
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToke,
        StarToken,
        SlashToken,
        OpenParanthesisToken,
        CloseParanthesisToken,
        BadToken
    }
}
