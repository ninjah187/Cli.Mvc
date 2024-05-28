using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc
{
    public class DescriptionAttribute : Attribute
    {
        public string Text { get; }

        public DescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
