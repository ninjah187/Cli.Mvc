using Cli.Mvc.Examples.Ninjas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Ninjas.Services
{
    public class NinjaService
    {
        readonly List<Ninja> _ninjas = [];

        public NinjaService() {}

        public void Create(string name)
        {
            var ninja = new Ninja(name);
            _ninjas.Add(ninja);
        }
    }
}
