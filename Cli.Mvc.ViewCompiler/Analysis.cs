using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class Analysis(IReadOnlyList<string> errors)
    {
        public bool IsValid => Errors.Count == 0;

        public IReadOnlyList<string> Errors { get; } = errors;
        public IReadOnlyList<string> Warnings { get; }
    }
}
