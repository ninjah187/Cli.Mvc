using Cli.Mvc.Parsing;
using Cli.Mvc.Runtime;
using Cli.Mvc.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc
{
    public class Controller
    {
        // rename to non-plural form? Argument / Option ? works better with Option.Exists()

        public ICommandContext CommandContext { get; private set; }

        public Params Arguments => CommandContext.Arguments;
        public Params Options => CommandContext.Options;

        protected IActionResult Ok(string message) => new MessageView(message);
        protected IActionResult Error(string message) => new MessageView(message);
        protected IActionResult Help() => new HelpView(CommandContext.Path);

        protected IActionResult List<T>(IEnumerable<T> source, Func<T, string> mapper) => new ListView<T>(source, mapper);

        protected IActionResult Table<T>(IEnumerable<T> rows, string[] header, Func<T, string[]> columns) => new TableView<T>(rows, header, columns);
        protected IActionResult Table<T>(IEnumerable<T> rows, Func<T, string[]> columns) => new TableView<T>(rows, columns); // or better naming source, column ?
        protected IActionResult Table(string[][] values, bool withHeaders = false) => new TableView(values, withHeaders);
    }
}
