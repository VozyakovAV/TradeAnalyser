using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAnalyser
{
    public class HeatMapData : INotifyPropertyChanged
    {
        public ObservableCollection<HeatMapItem> Items { get; set; }

        public double Min { get { return Items == null ? 0 : Items.Min(x => x.Z); } }
        public double Max { get { return Items == null ? 0 : Items.Max(x => x.Z); } }
        public double Avg { get { return Items == null ? 0 : Items.Average(x => x.Z); } }

        // ---------------------------

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void Update(IEnumerable<DataRow> rows, string columnX, string columnY, string columnZ)
        {
            var items = new List<HeatMapItem>();

            foreach (DataRow row in rows)
            {
                var x = (double)row[columnX];
                var y = (double)row[columnY];
                var z = (double)row[columnZ];
                items.Add(new HeatMapItem(x, y, z));
            }

            var groups = items.GroupBy(x => $"{x.X},{x.Y}").ToList();
            var items2 = groups.Select(x =>
            {
                var l = x.ToList();
                var f = l.First();
                var avgZ = l.Average(y => y.Z);
                return new HeatMapItem(f.X, f.Y, avgZ);
            }).OrderBy(x => x.X).ThenBy(x => x.Y).ToList();

            Items = new ObservableCollection<HeatMapItem>(items2);

            OnPropertyChanged("Items");
            OnPropertyChanged("Min");
            OnPropertyChanged("Max");
            OnPropertyChanged("Avg");
        }
    }
}
