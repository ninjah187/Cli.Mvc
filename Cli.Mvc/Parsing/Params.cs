using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Parsing
{
    public class Params
    {
        public int Count => _params.Count;

        readonly Dictionary<string, string> _params;

        public Params(Dictionary<string, string> @params)
        {
            _params = @params;
        }

        public bool Exists(string name)
        {
            return _params.ContainsKey(name);
        }

        public T Get<T>(string name)
        {
            return (T) Convert.ChangeType(_params[name], typeof(T));
        }

        public object Get(string name, Type type)
        {
            if (type == typeof(bool))
            {
                return _params.ContainsKey(name);
            } else if (Nullable.GetUnderlyingType(type) == null)
            {
                return Convert.ChangeType(_params[name], type);                
            } 
            return null;
        }

        public string this[string name]
        {
            get
            {
                var hasValue = _params.TryGetValue(name, out var value);
                return hasValue ? value : null;
            }
        }
    }
}
