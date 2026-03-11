using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.ViewCompiler.Tests
{
    public class AnalyzerTests
    {
        [Fact]
        public void Success()
        {
            var text = """@if""";

            var expectedErrors = new List<string>
            {
                "Unexpected @if keyword at 0:0"
            };

            var parser = new Parser();
            var analyzer = new Analyzer();

            var ast = parser.Parse(text);
            var analysis = analyzer.Analyze(ast);

            Assert.Equivalent(expectedErrors, analysis.Errors, true);
        }
    }
}
