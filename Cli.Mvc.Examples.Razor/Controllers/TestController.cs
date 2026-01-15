using Cli.Mvc.Examples.Razor.Models;
using Cli.Mvc.Examples.Razor.Views.Compiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Razor.Controllers
{
    public class TestController : Controller
    {
        public IActionResult View()
        {
            var ninja = new Ninja("Karol");

            var view = new WorldView(ninja);

            Console.WriteLine(view.Render());

            return Ok("Success!");
        }
    }
}
