using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.ViewCompiler.Tests
{
    public class EmitterTests
    {
        [Fact]
        public void CanEmitRenderingClass()
        {
            var template = """
                @model Cli.Mvc.Examples.Razor.Models.Ninja

                Hello world @Model.Name
                """;

            var expectedCode = """
                using System;
                using System.Text;

                namespace TestNamespace
                {
                    public class TestClassView
                    {
                        public Cli.Mvc.Examples.Razor.Models.Ninja Model { get; }
                
                        public TestClassView(Cli.Mvc.Examples.Razor.Models.Ninja model)
                        {
                            Model = model;
                        }
                
                        public string Render()
                        {
                            var sb = new StringBuilder();
                
                            sb.Append("\r\n\r\nHello world ");
                            sb.Append(Model.Name);

                            return sb.ToString();
                        }
                    }
                }
                """;

            var emitter = new ViewRenderingCodeEmitter();

            var code = emitter.EmitClass("TestNamespace", "TestClass", template);

            Assert.Equal(expectedCode, code);
        }

        [Fact]
        public void CanEmitForeach()
        {
            var template = """
                @model List<Cli.Mvc.Examples.Razor.Models.Ninja>

                Hello world!

                @foreach (var ninja in Model)
                {
                    @ninja.Name
                }
                """;

            var expectedCode = """
                using System;
                using System.Text;

                namespace TestNamespace
                {
                    public class TestClassView
                    {
                        public List<Cli.Mvc.Examples.Razor.Models.Ninja> Model { get; }
                
                        public TestClassView(List<Cli.Mvc.Examples.Razor.Models.Ninja> model)
                        {
                            Model = model;
                        }
                
                        public string Render()
                        {
                            var sb = new StringBuilder();
                
                            sb.Append("\r\n\r\nHello world!\r\n\r\n");
                            foreach (var ninja in Model)
                            {
                                sb.Append(ninja.Name);
                                sb.Append("\r\n");
                            }

                            return sb.ToString();
                        }
                    }
                }
                """;

            var emitter = new ViewRenderingCodeEmitter();

            var code = emitter.EmitClass("TestNamespace", "TestClass", template);

            Assert.Equal(expectedCode, code);
        }
    }
}
