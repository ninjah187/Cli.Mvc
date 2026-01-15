using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Ninjas.Controllers
{
    [Description("Manage monsters.")]
    public class MonsterController : Controller
    {
        [Description("List all monsters.")]
        public IActionResult List()
        {
            return Ok("");
        }
    }
}
