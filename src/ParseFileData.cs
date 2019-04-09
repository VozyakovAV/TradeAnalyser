using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TradeAnalyser
{
    public class ParseFileData
    {
        public static DataTable Parse(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var dt = new DataTable();
            IList<DataColumnCustom> columns = null;

            if (lines.Length > 1)
            {
                var columnNames = SplitLine(lines[0]);
                var rowValues = SplitLine(lines[1]);
                columns = ParseColumns(columnNames, rowValues);
                dt.Columns.AddRange(columns.ToArray());
            }

            for (int i = 1; i < lines.Length; i++)
            {
                var values = SplitLine(lines[i]);
                var row = dt.NewRow();

                foreach (var col in columns)
                {
                    row[col] = ParseDouble(values[col.Index]);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        private static IList<string> SplitLine(string line)
        {
            return line.Split(';').Select(x => x.Trim()).ToList();
        }

        private static IList<DataColumnCustom> ParseColumns(IList<string> columnNames, IList<string> rowValues)
        {
            var columns = new List<DataColumnCustom>();

            for (int i = 0; i < columnNames.Count; i++)
            {
                var columnName = columnNames[i];
                var rowValue = rowValues[i];
                Type type = null;

                if (IsDouble(rowValue))
                {
                    type = typeof(double);
                }

                if (type != null)
                {
                    var col = new DataColumnCustom(i, columnName, type);
                    columns.Add(col);
                }
            }

            return columns;
        }


        private static double ParseDouble(string value)
        {
            double d = 0;
            double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out d);
            return d;
        }

        private static bool IsDouble(string value)
        {
            double d = 0;
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out d);
        }

        private class DataColumnCustom : DataColumn
        {
            public int Index { get; set; }

            public DataColumnCustom(int index, string columnName, Type dataType) : base(columnName, dataType)
            {
                this.Index = index;
            }
        }

    }
}
