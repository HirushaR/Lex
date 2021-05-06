using System.Collections.Generic;
using System;
using Lex.CodeAnalysis.Syntax;
using Lex.CodeAnalysis.Text;
using System.Text;
using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly SourceText _text;

        private int _position;

        private int _start;
        private SyntaxKind _kind;
        private object _value;

        public Lexer(SourceText text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        private char Peek(int offset)
        {
            var index = _position + offset;

            if (index >= _text.Length)
                return '\0';

            return _text[index];
        }

        public SyntaxToken Lex()
        {
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                    _kind = SyntaxKind.EndOfFileToken;
                    break;
                case '+':
                    _kind = SyntaxKind.PlusToken;
                    _position++;
                    break;
                case '-':
                    _kind = SyntaxKind.MinusToken;
                    _position++;
                    break;
                case '*':
                    _position++;
                    if (Current != '*')
                    {
                        _kind = SyntaxKind.StarToken;
                    }
                    else
                    {
                        _position++;
                        _kind = SyntaxKind.StarStarToken;
                    }
                    break;
                case '/':
                    _kind = SyntaxKind.SlashToken;
                    _position++;
                    break;
                case '%':
                    _kind = SyntaxKind.RemainderToken;
                    _position++;
                    break;
                case ':':
                    _kind = SyntaxKind.ColonToken;
                    _position++;
                    break;
                case '<':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.LessThanToken;
                    }
                    else
                    {
                        _position++;
                        _kind = SyntaxKind.LessOrEqualToken;
                    }
                    break;
                case '>':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.GreaterThanToken;
                    }
                    else
                    {
                        _position++;
                        _kind = SyntaxKind.GreaterOrEqualToken;
                    }
                    break;
                case '(':
                    _kind = SyntaxKind.OpenParenthesisToken;
                    _position++;
                    break;
                case ')':
                    _kind = SyntaxKind.CloseParenthesisToken;
                    _position++;
                    break;
                 case ',':
                    _kind = SyntaxKind.CommaToken;
                    _position++;
                    break;
                 case '~':
                    _kind = SyntaxKind.TildToken;
                    _position++;
                    break;
                 case '^':
                    _kind = SyntaxKind.HatToken;
                    _position++;
                    break;
                 case '{':
                    _kind = SyntaxKind.OpenBraceToken;
                    _position++;
                    break;
                case '}':
                    _kind = SyntaxKind.CloseBraceToken;
                    _position++;
                    break;
                case '&':
                     _position++;
                    if (Current != '&')
                    {
                        _kind = SyntaxKind.AmpersandToken;
                    }
                    else
                    {
                        _position++;
                        _kind = SyntaxKind.AmpersandAmpersandToken;
                    }
                    break;
                case '|':
                     _position++;
                    if (Current != '|')
                    {
                        _kind = SyntaxKind.PipeToken;
                    }
                    else
                    {
                        _position++;
                        _kind = SyntaxKind.PipePieToken;
                    }
                    break;
                case '=':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.EaqlesToken;
                    }
                    else
                    {
                        _position++;
                        _kind = SyntaxKind.EaqulesEaqlesToken;
                    }
                    break;
                case '!':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.BangToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.BangEaqlesToken;
                        _position++;
                    }
                    break;
                case '\"':
                    ReadString();
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ReadNumberToken();
                    break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    ReadWhiteSpace();
                    break;
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeyword();
                    }
                    else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpace();
                    }
                    else
                    {
                        _diagnostics.ReportBadCharactor(_position, Current);
                        _position++;
                    }
                    break;
            }

            var length = _position - _start;
            var text = SyntaxFacts.GetText(_kind);
            if (text == null)
                text = _text.ToString(_start, length);

            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadString()
        {
            //"Test \" asdasd"
            // asdasda"
            //
            // Skip the current
            _position++;
            var sb = new StringBuilder();
            var done = false;
            
            while(!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        var span = new TextSpan(_start,1);
                        _diagnostics.ReportUnterminatedString(span);
                        done = true;
                        break;
                    case '"':
                        if(Lookahead == '"')
                        {
                            sb.Append(Current);
                            _position+=2;
                        }
                        else
                        {
                            _position++;
                            done = true;
                        }

                        break;
                    default:
                        sb.Append(Current);
                        _position++;
                        break;
                }
            }

            _kind = SyntaxKind.StringToken;
            _value = sb.ToString();
        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                _position++;

            _kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            if (!int.TryParse(text, out var value))
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, TypeSymbol.Int);

            _value = value;
            _kind = SyntaxKind.NumberToken;
        }

        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current))
                _position++;

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            _kind = SyntaxFacts.GetKeywordKind(text);
        }
    }

}