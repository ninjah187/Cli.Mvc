using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Views
{
    public class TableView : View
    {
        readonly string[][] _values;
        readonly bool _withHeader;

        public TableView(string[][] values, bool withHeader)
        {
            _values = values;
            _withHeader = withHeader;
        }

        public override void Render()
        {
            Table(_values, _withHeader);
        }
    }

    public class TableView<T> : View
    {
        readonly IEnumerable<T> _rows;
        readonly string[] _header;
        readonly Func<T, string[]> _columns;

        public TableView(IEnumerable<T> rows, Func<T, string[]> columns)
            : this(rows, Array.Empty<string>(), columns)
        {
        }

        public TableView(IEnumerable<T> rows, string[] header, Func<T, string[]> columns)
        {
            _rows = rows;
            _header = header;
            _columns = columns;
        }

        public override void Render()
        {
            //var data = _header == null ? Array.Empty<string[]>() : new[] { _header };

            //var ninjas = data.Concat(
            //        _rows.
            //    )
            //    .ToArray();

            // TODO: imo more logical order of arguments here is: header, rows, columns
            Table(_rows, _header, _columns);
        }
    }
}
