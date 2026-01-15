using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc
{
    public class DocAttribute : Attribute
    {
        public string Text { get; }

        public DocAttribute(string text)
        {
            Text = text;
        }

        // TODO: add multiline docs by accepting Func<IEnumerable<string>>
    }

    public class DescriptionAttribute : Attribute
    {
        public string Text { get; }

        public DescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
