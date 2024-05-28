using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Views
{
    public class ListView : View
    {
        readonly string _title;
        readonly IEnumerable<string> _values;

        public ListView(string title, IEnumerable<string> values)
        {
            _title = title;
            _values = values;
        }

        public ListView(IEnumerable<string> values)
            : this(null, values)
        {
        }

        public override void Render() => List(_values);
    }

    public class ListView<T> : View
    {
        readonly string _title;
        readonly IEnumerable<T> _items;
        readonly Func<T, string> _mapper;

        public ListView(string title, IEnumerable<T> items, Func<T, string> mapper)
        {
            _title = title;
            _items = items;
            _mapper = mapper;
        }

        public ListView(IEnumerable<T> items, Func<T, string> mapper)
            : this(null, items, mapper)
        {
        }

        public override void Render() => List(_title, _items, _mapper);
    }
}
