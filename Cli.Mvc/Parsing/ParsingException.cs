using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Parsing
{
    internal class ParsingException : Exception
    {
        public ParsingException(string message) : base(message) {}
    }
}
