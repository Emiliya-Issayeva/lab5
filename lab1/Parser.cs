using System;
using System.Collections.Generic;

namespace lab1
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message) { }
    }

    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos;

        public Parser(string input)
        {
            var lexer = new Lexer(input);
            _tokens = lexer.Tokenize();
            _pos = 0;
        }

        private Token Look => _tokens[_pos];

        private Token Consume(TokenType expected)
        {
            if (Look.Type == expected)
                return _tokens[_pos++];
            // синтаксическая ошибка с позицией
            var foundLexeme = Look.Type == TokenType.EndOfInput ? "конец ввода" : Look.Lexeme;
            throw new ParserException(
                $"Ожидался «{Symbol(expected)}», найден «{foundLexeme}» на позиции {Look.Position}");
        }


        private string Symbol(TokenType t) => t switch
        {
            TokenType.Plus => "+",
            TokenType.Minus => "-",
            TokenType.Multiply => "*",
            TokenType.Divide => "/",
            TokenType.LParen => "(",
            TokenType.RParen => ")",
            TokenType.Number => "число",
            TokenType.EndOfInput => "конец ввода",
            _ => t.ToString()
        };

        public List<Token> Parse()
        {
            var poliz = new List<Token>();
            ParseE(poliz);
            if (Look.Type != TokenType.EndOfInput)
                throw new ParserException($"Лишний токен «{Look.Lexeme}» после конца выражения");
            return poliz;
        }

        private void ParseE(List<Token> poliz)
        {
            ParseT(poliz);
            ParseA(poliz);
        }

        // A -> { (+|-) T }*
        private void ParseA(List<Token> poliz)
        {
            while (Look.Type == TokenType.Plus || Look.Type == TokenType.Minus)
            {
                var op = Consume(Look.Type);
                ParseT(poliz);
                poliz.Add(op);
            }
        }

        private void ParseT(List<Token> poliz)
        {
            ParseO(poliz);
            ParseB(poliz);
        }

        // B -> { (*|/) O }*
        private void ParseB(List<Token> poliz)
        {
            while (Look.Type == TokenType.Multiply || Look.Type == TokenType.Divide)
            {
                var op = Consume(Look.Type);
                ParseO(poliz);
                poliz.Add(op);
            }
        }

        private void ParseO(List<Token> poliz)
        {
            if (Look.Type == TokenType.Number)
            {
                poliz.Add(Consume(TokenType.Number));
            }
            else if (Look.Type == TokenType.LParen)
            {
                Consume(TokenType.LParen);
                ParseE(poliz);
                Consume(TokenType.RParen);
            }
            else
            {
                // синтаксическая ошибка: не число и не '('
                throw new ParserException(
                    $"Ожидалось число или «(», найдено «{Symbol(Look.Type)}» на позиции {Look.Position}");
            }
        }

        public double EvaluatePoliz(List<Token> poliz)
        {
            var stack = new Stack<double>();
            foreach (var tok in poliz)
            {
                switch (tok.Type)
                {
                    case TokenType.Number:
                        stack.Push(double.Parse(tok.Lexeme));
                        break;
                    case TokenType.Plus:
                        {
                            var b = stack.Pop();
                            var a = stack.Pop();
                            stack.Push(a + b);
                            break;
                        }
                    case TokenType.Minus:
                        {
                            var b = stack.Pop();
                            var a = stack.Pop();
                            stack.Push(a - b);
                            break;
                        }
                    case TokenType.Multiply:
                        {
                            var b = stack.Pop();
                            var a = stack.Pop();
                            stack.Push(a * b);
                            break;
                        }
                    case TokenType.Divide:
                        {
                            var b = stack.Pop();
                            var a = stack.Pop();
                            if (b == 0)
                                throw new ParserException("Нельзя делить на ноль");
                            stack.Push(a / b);
                            break;
                        }
                    default:
                        throw new ParserException($"Неподдерживаемый токен «{tok.Lexeme}» при вычислении");
                }
            }
            if (stack.Count != 1)
                throw new ParserException("Некорректное выражение.");
            return stack.Pop();
        }
    }
}