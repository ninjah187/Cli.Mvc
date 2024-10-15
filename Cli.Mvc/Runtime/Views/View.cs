using Cli.Mvc.Rendering;
using Cli.Mvc.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cli.Mvc.Views
{
    public abstract class View : IActionResult
    {
        [Service] protected IRenderer Renderer { get; private set; }

        public abstract void Render();

        protected void Line(string value = null) => Renderer.WriteLine(value);

        protected void Write(string value) => Renderer.Write(value);

        protected void Write(string value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Write(value);
            }
        }

        protected void Color(ConsoleColor color, Action action)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            action();

            Console.ForegroundColor = oldColor;
        }

        protected void List<T>(string title, IEnumerable<T> items, Func<T, string> mapper) => List(title, items.Select(mapper));

        protected void List<T>(IEnumerable<T> items, Func<T, string> mapper) => List(null, items, mapper);

        protected void List(IEnumerable<string> values) => List(null, values);

        protected void List(string title, IEnumerable<string> values)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Line(title);
            }

            foreach (var value in values)
            {
                Line($"- {value}");
            }
        }

        // string[,]
        /**
         * new[]
         * {
         *    
         * }
         * 
         * 
         */

        protected void Table<T>(IEnumerable<T> rows, Func<T, string[]> column)
        {
            var values = rows.Select(item => column(item)).ToArray();
            Table(values);
        }

        protected void Table<T>(IEnumerable<T> rows, string[] header, Func<T, string[]> column)
        {
            var withHeaders = header != null;

            var values = new List<string[]>();

            if (withHeaders)
            {
                values.Add(header);
            }

            values.AddRange(rows.Select(row => column(row)));

            //new List<string>().Concat()

            //var values = (header ?? Array.Empty<string>())
            //    .Concat(rows.Select(row => column(row))
            //    .ToArray();

            Table(values.ToArray(), withHeaders);
        }

        protected void Table(string[][] values, bool withHeaders = false)
        {
            var rows = values.Length;
            var columns = values.Max(column => column.Length);

            var cellPadding = 1;

            var tableWidth = values.Max(column => column.Sum(value => value.Length + 2))
                // + 2  // left and right border
                + columns;

            var columnWidths = EnumerateColumns(values).Select(column => column.Max(value => value.Length)).ToArray();

            Write("+");
            Write("-", tableWidth - 1);
            Write("+");

            Line();

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {

                    //if (column == -1 || column == columns)
                    //{
                    //    Write("|");
                    //    continue;
                    //}

                    var columnWidth = columnWidths[column];

                    var value = values[row][column];

                    if (column == 0)
                    {
                        Write("|");
                    }

                    Write(" ");
                    Write(value);

                    if (value.Length == columnWidth)
                    {
                        Write(" |");
                    }
                    else
                    {
                        var count = columnWidth - value.Length;

                        for (int k = 0; k < count; k++)
                        {
                            Write(" ");
                        }

                        Write(" |");
                    }
                }

                Line();

                if (row == 0 && withHeaders)
                {
                    Write("+");
                    Write("-", tableWidth - 1);
                    Write("+");
                    Line();
                }
            }

            Write("+");
            Write("-", tableWidth - 1);
            Write("+");
        }

        static IEnumerable<string[]> EnumerateColumns(string[][] values)
        {
            var columns = values.Max(column => column.Length);

            for (int column = 0; column < columns; column++)
            {
                var currentColumn = new List<string>();

                for (int row = 0; row < values.Length; row++)
                {
                    currentColumn.Add(values[row][column]);
                }

                yield return currentColumn.ToArray();

                currentColumn.Clear();
            }
        }

        static IEnumerable<string> EnumerateValues(string[,] values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }
    }
}
