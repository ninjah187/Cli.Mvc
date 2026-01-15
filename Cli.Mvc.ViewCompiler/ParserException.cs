using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    internal class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }
}
