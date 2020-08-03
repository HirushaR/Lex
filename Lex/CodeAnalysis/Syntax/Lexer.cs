using System.Collections.Generic;

namespace Lex.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        private int _position;
        private int _start;
        private SyntaxKind _kind;
        private object _value;
       
        public Lexer(string text)
        {
            _text = text;
        }
  
        public DiagnosticBag Diagnostics => _diagnostics;

        private char Current => Peek(0);
        private char Lookahed => Peek(1);
        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';

            return _text[index];
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {

            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            if (char.IsDigit(Current))
            {

                ReadNumberToken();
                
            }

            else if (char.IsWhiteSpace(Current))
            {
                

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - _start;
                var text = _text.Substring(_start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, _start, text, null);
            }
            else if (char.IsLetter(Current))
            {

                while (char.IsLetter(Current))
                    Next();

                var length = _position - _start;
                var text = _text.Substring(_start, length);
                var kind = SyntaxFacts.GetKeyworkKind(text);
                return new SyntaxToken(kind, _start, text, null);

            }
            else {
                switch (Current)
                {
                    case '\0':
                        return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
                    case '+':
                        return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                    case '-':
                        return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                    case '*':
                        return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                    case '/':
                        return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                    case '(':
                        return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                    case ')':
                        return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);                    
                    case '&':
                        if (Lookahed == '&')
                        {  
                            _position += 2;
                            return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _start, "&&", null);                       
                        }
                        break;
                    
                    case '|':
                        if (Lookahed == '|')
                        {
                             _position += 2;
                             return new SyntaxToken(SyntaxKind.PipePieToken, _start, "||", null);
                        }
                        
                        break;
                    case '=':
                        if (Lookahed == '=')
                        {
                             _position += 2;
                             return new SyntaxToken(SyntaxKind.EaqulesEaqlesToken, _start, "==", null);
                        }
                        else
                        {
                            _position += 1;
                            return new SyntaxToken(SyntaxKind.EaqlesToken, _start, "=", null);
                        }

                    case '!':
                        if (Lookahed == '=')
                        {
                            _position += 2;
                            return new SyntaxToken(SyntaxKind.BangEaqlesToken, _start, "!=", null);
                        }
                        else
                        {
                             _position += 1;
                             return new SyntaxToken(SyntaxKind.BangToken, _start, "!", null);
                        }
                    default:
                        _diagnostics.ReportBadCharactor(_position, Current);
                        _position++;
                        break;
                }
            }

            var length = _position - _start;
            var text = SyntaxFacts.GetText(_kind);
            if(text== null)
                text = _text.Substring(_start,length);
            
            return new SyntaxToken(_kind, _start, text, _value);
            
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                Next();

            var length = _position - _start;
            var text = _text.Substring(_start, length);
            if (!int.TryParse(text, out var value))
                _diagnostics.ReportInvalidNumber(new TextSpan(_start,length), _text,typeof(int));

            _value = value;
            _kind = SyntaxKind.NameExpression;
            
        }
    }

}