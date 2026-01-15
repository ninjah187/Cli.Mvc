using Cli.Mvc.Examples.Razor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Razor.Services
{
    public class NinjaService
    {
        public IReadOnlyList<Ninja> Ninjas => _ninjas;
        readonly List<Ninja> _ninjas = [];

        public Ninja Add(string name)
        {
            var ninja = new Ninja(name);

            _ninjas.Add(ninja);

            return ninja;
        }
    }
}
