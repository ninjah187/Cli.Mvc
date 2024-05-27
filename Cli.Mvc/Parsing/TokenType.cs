using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Parsing
{
    public enum TokenType
    {
        Word,
        Option,
        OptionWithValue,
        End
    }
}
