using Cli.Mvc.Examples.Razor.Models;
// using Cli.Mvc.Examples.Razor.Views.Compiled;
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
            var ninjas = new List<Ninja>
            {
                new("Karol"),
                new("Lexi")
            };

            // var view = new WorldView(ninjas);

            // Console.WriteLine(view.Render());

            // Console.Write();

            // return Ok(view.Render());

            return Ok("Success!");
        }
    }
}
