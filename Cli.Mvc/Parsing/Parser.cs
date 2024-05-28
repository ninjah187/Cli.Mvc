using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cli.Mvc.Parsing
{
    public class Parser
    {
        readonly List<string> _arguments = new List<string>();
        readonly Dictionary<string, string> _options = new Dictionary<string, string>();

        readonly IEnumerable<Token> _tokens;

        string _optionKey = null;
        
        public Parser(string command)
        {
            _tokens = new Tokenizer().Tokenize(command);
        }

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens;
        }

        public ParsedParams Parse()
        {
            var input = new Stack<Token>(_tokens.Reverse());

            while (input.Any())
            {
                var token = input.Pop();

                if (token.Type == TokenType.Word)
                {
                    SaveValue(input, token.Value);
                    continue;
                }

                if (token.Type == TokenType.Option)
                {
                    SetOptionKey(token.Value);
                    continue;
                }

                throw new ParsingException($"Unrecognized token. Type: {token.Type}, value: {token.Value}.");
            }

            return new ParsedParams(_arguments, new Params(_options));
        }

        void SaveValue(Stack<Token> input, string value)
        {
            if (_optionKey == null)
            {
                _arguments.Add(value);
            }
            else
            {
                _options[_optionKey] = value;

                while (input.Any())
                {
                    var token = input.Peek();

                    if (token == null)
                    {
                        break;
                    }

                    if (token.Type == TokenType.Option)
                    {
                        break;
                    }

                    input.Pop();

                    _options[_optionKey] = _options[_optionKey] + " " + token.Value;
                }
            }
        }

        void SetOptionKey(string value)
        {
            _optionKey = value;
            _options[_optionKey] = null;
        }
    }
}
