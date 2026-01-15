using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Configuration
{
    public interface IConfigurationService
    {
        T GetValue<T>(string key);
    }
}
