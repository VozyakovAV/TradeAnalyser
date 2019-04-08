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
    public class ChartData : INotifyPropertyChanged
    {
        public ObservableCollection<ChartItem> Items { get; protected set; }
        public double MaximimX { get { return Items == null ? 0 : Items.Max(x => x.X); } }
        public double MinimumX { get { return Items == null ? 0 : Items.Min(x => x.X); } }

        // ---------------------------

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void Update(DataTable dataTable, string columnX, string columnY)
        {
            var items = new List<ChartItem>();

            foreach (DataRow row in dataTable.Rows)
            {
                var x = (double)row[columnX];
                var y = (double)row[columnY];
                items.Add(new ChartItem(x, y));
            }

            items = items.OrderBy(x => x.X).ToList();
            var groups = items.GroupBy(x => x.X).ToList();
            var items2 = groups.Select(x => new ChartItem(x.Key, x.ToList().Average(y => y.Y))).ToList();

            Items = new ObservableCollection<ChartItem>(items2);

            OnPropertyChanged("Items");
            OnPropertyChanged("MaximimX");
            OnPropertyChanged("MinimumX");
        }
    }
}
