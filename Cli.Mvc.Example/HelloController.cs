using Cli.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Example
{
    public class ExampleAttribute : Attribute
    {


        public ExampleAttribute(Func<string> exampleFunc)
        {
        }

        public ExampleAttribute(string example, string output = null)
        {
        }
    }

    public class HelloController : Controller
    {
        [Description("Say hello to a world.")]
        [Example("hello world Bob 30 --city London", "Hello Bob!")]
        public IActionResult World(
            [Description("Name of an user.")] string name,
            [Description("Age of an user.")] int age,
            [Option][Description("To be polite or not.")] bool polite,
            [Option][Description("To be .")] string city)
        {
            if (age < 18)
            {
                return Ok($"Cannot access with age {age}. Only adult users allowed.");
            }

            if (polite)
            {
                return Ok($"Hello world, sincere {name}!");
            }

            return Ok($"Hello world {name} from city of {city}!");
        }

        static string WorldExample()
        {
            return $"hello world Bob 30 --city London";
        }

        [Description("List all the dogs.")]
        public IActionResult Dogs()
        {
            var dogs = new List<Dog>
            {
                new() { Name = "Lolek" },
                new() { Name = "Lexi "}
            };
            return List(dogs, dog => dog.Name);
        }
    }

    class Dog
    {
        public string Name { get; set; }
    }
}
