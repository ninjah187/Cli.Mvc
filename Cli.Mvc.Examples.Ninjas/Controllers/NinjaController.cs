using Cli.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Ninjas.Controllers
{
    [Description("Manage clan of ninjas.")]
    public class NinjaController : Controller
    {
        /// <summary>
        /// Add a ninja.
        /// </summary>
        /// <param name="name">Ninja's name.</param>
        [Description("Add a ninja.")]
        public IActionResult Add([Description("Ninja's name.")] string name)
        {
            return Ok($"Ninja {name} added");
        }

        /// <summary>
        /// Change weapon of a ninja.
        /// </summary>
        [Route("equip weapon")]
        [Description("Change ninja's weapon.")]
        public IActionResult ChangeWeapon(
            [Description("Name of target ninja.")] string ninja,
            [Description("Weapon to equip.")] string weapon)
        {
            return Ok($"Ninja {ninja} equipped {weapon}");
        }
    }
}
