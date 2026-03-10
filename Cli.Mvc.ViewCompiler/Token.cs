using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class Token(string value, TokenType type, int position, int line, int column)
    {
        public string Value { get; } = value;
        public TokenType Type { get; } = type;

        public int Position { get; } = position; // rename to index ???
        public int Line { get; } = line;
        public int Column { get; } = column; // Rename to (row, column) or (line, character) or (line, start)???

        public override string ToString()
        {
            var formattedValue = Value;

            if (Value == "\n" || Value == "\r\n")
            {
                formattedValue = Value
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r");
            }

            return $"Token(\"{formattedValue}\", {Type}, {Position}, {Line}, {Column})";
        }
    }
}
