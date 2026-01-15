using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.HelloWorld
{
    public class HelloController : Controller
    {
        public IActionResult World()
        {
            return Ok("Hello world!");
        }
    }
}
