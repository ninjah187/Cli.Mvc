using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cli.Mvc;

namespace Cli.Mvc.Example.controllers
{
    public class HelloController : Controller
    {
        public IActionResult World(
            [Description("name of a person")] string name,
            [Description("ageo of a person")] int? age
            )
        {
            if ( age.HasValue ) 
            {
                return Ok($"Hi {name}, you are {age} years old");
            } else
            {
                return Ok($"Hi {name}");
            }

        }        
    }
}
