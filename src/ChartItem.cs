using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAnalyser
{
    public class ChartItem
    {
        public double X { get; set; }
        public double Y { get; set; }

        public ChartItem(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"{X} - {Y}";
        }
    }
}
