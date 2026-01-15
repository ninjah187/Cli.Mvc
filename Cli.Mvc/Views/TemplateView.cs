using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Views
{
    public class TemplateView<T> : View
    {
        protected T Model { get; }
        protected string Template { get; set; }

        public TemplateView(T model)
        {
            Model = model;
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }

    class TemplateTest : TemplateView<string>
    {
        public TemplateTest()
            : base("Hello world!")
        {
            Template = "{{Model}}";

            
        }
    }

    internal class TemplateView
    {
        public string Template = @"
            @for ninja in Model:
              - { ninja.Name }
            @endfor
            
            @if Model.Count == 0:
                There is nothing to display.
            @else
                - { Model[0].Name }
            @endif
        ";
    }
}
