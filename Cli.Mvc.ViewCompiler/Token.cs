using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class Token(string value, TokenType type)
    {
        public string Value { get; } = value;
        public TokenType Type { get; } = type;

        public override string ToString()
        {
            var formattedValue = Value;

            if (Value == "\n" || Value == "\r\n")
            {
                formattedValue = Value
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r");
            }

            return $"Token(\"{formattedValue}\", {Type})";
        }
    }
}
