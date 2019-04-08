using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAnalyser
{
    public class HeatMapItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public HeatMapItem(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return $"({X},{Y}): {Z}";
        }
    }
}
