using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Razor.Models
{
    public class Ninja
    {
        public string Name { get; }

        public Ninja(string name)        {
            Name = name;
        }
    }
}
