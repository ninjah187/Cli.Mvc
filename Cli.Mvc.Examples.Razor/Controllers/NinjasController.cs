using Cli.Mvc.Examples.Razor.Models;
using Cli.Mvc.Examples.Razor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Razor.Controllers
{
    public class NinjasController : Controller
    {
        readonly NinjaService _ninjaService;

        public NinjasController(NinjaService ninjaService)
        {
            _ninjaService = ninjaService;
        }

        public IActionResult List()
        {
            return this.View(_ninjaService.Ninjas);
        }

        public IActionResult Add(string name)
        {
            var ninja = _ninjaService.Add(name);

            return this.View(ninja);
        }
    }
}
