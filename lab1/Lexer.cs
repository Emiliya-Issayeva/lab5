using System;
using System.Collections.Generic;

namespace lab1
{
    public enum TokenType
    {
        Number,
        Plus,
        Minus,
        Multiply,
        Divide,
        LParen,
        RParen,
        EndOfInput
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public int Position { get; }

        public Token(TokenType type, string lexeme, int position)
        {
            Type = type;
            Lexeme = lexeme;
            Position = position;
        }

        public override string ToString() => $"{Type}('{Lexeme}') at {Position}";
    }

    public class Lexer
    {
        private readonly string _text;
        private int _pos;
        public Lexer(string text) { _text = text; _pos = 0; }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();
            while (true)
            {
                SkipWhite();
                if (_pos >= _text.Length)
                {
                    tokens.Add(new Token(TokenType.EndOfInput, "", _pos + 1));
                    break;
                }

                char c = _text[_pos];
                if (char.IsDigit(c))
                {
                    var start = _pos;
                    while (_pos < _text.Length && char.IsDigit(_text[_pos]))
                        _pos++;
                    var num = _text.Substring(start, _pos - start);
                    tokens.Add(new Token(TokenType.Number, num, start + 1));
                }
                else
                {
                    switch (c)
                    {
                        case '+':
                            tokens.Add(new Token(TokenType.Plus, "+", _pos + 1)); _pos++; break;
                        case '-':
                            tokens.Add(new Token(TokenType.Minus, "-", _pos + 1)); _pos++; break;
                        case '*':
                            tokens.Add(new Token(TokenType.Multiply, "*", _pos + 1)); _pos++; break;
                        case '/':
                            tokens.Add(new Token(TokenType.Divide, "/", _pos + 1)); _pos++; break;
                        case '(':
                            tokens.Add(new Token(TokenType.LParen, "(", _pos + 1)); _pos++; break;
                        case ')':
                            tokens.Add(new Token(TokenType.RParen, ")", _pos + 1)); _pos++; break;
                        default:
                            // Лексическая ошибка с позицией
                            throw new ParserException($"Недопустимый символ «{c}» на позиции {_pos + 1}");
                    }
                }
            }
            return tokens;
        }

        private void SkipWhite()
        {
            while (_pos < _text.Length && char.IsWhiteSpace(_text[_pos]))
                _pos++;
        }
    }
}
