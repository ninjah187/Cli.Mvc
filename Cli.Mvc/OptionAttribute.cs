using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class OptionAttribute : Attribute
    {
        public string Description { get; }
    }
}
