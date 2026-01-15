using Cli.Mvc.Examples.Razor.Models;
using Cli.Mvc.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Examples.Razor
{
    public class ListView(List<Ninja> model)
    {
        public List<Ninja> Model { get; } = model;

        public void Render()
        {
            Console.WriteLine("Ninja's list:");

            if (Model.Count == 0)
            {
                Console.WriteLine("There are no ninjas");
            }
            else
            {
                foreach (var ninja in Model)
                {
                    Console.WriteLine("- " + ninja.Name);
                }
            }
        }
    }
}
