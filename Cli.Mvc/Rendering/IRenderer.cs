using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Rendering
{
    public interface IRenderer
    {
        void WriteLine(string value = null);
        void Write(string value);
    }
}
