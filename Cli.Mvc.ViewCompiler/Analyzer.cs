using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class Analyzer
    {
        public Analysis Analyze(AbstractSyntaxTree tree)
        {
            var errors = new List<string>();

            foreach (var node in tree.Traverse())
            {
                switch (node)
                {
                    case IfNode @if:
                        if (@if.Body == null)
                        {
                            errors.Add($"Unexpected @if keyword at {@if.Keyword.Line}:{@if.Keyword.Column}");
                        }
                        break;
                }
            }

            return new Analysis(errors);
        }
    }
}
