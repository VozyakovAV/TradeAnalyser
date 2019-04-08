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
        public ObservableCollection<string> Columns { get; protected set; }
        public double MaximimX { get { return Items == null ? 0 : Items.Max(x => x.X); } }
        public double MinimumX { get { return Items == null ? 0 : Items.Min(x => x.X); } }

        // ---------------------------

        private DataTable _dataTable;
        public DataTable DataTable
        {
            get { return _dataTable; }
            set
            {
                _dataTable = value;
                var columns = _dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                Columns = new ObservableCollection<string>(columns);
                Update();
            }
        }

        // ---------------------------

        private string _selectedX;
        public string SelectedX
        {
            get { return _selectedX; }
            set
            {
                _selectedX = value;
                Update();
            }
        }

        // ---------------------------

        private string _selectedY;
        public string SelectedY
        {
            get { return _selectedY; }
            set
            {
                _selectedY = value;
                Update();
            }
        }

        // ---------------------------

        private string _selectedZ;
        public string SelectedZ
        {
            get { return _selectedZ; }
            set
            {
                _selectedZ = value;
                Update();
            }
        }

        // ---------------------------

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void Update()
        {
            if (SelectedX != null && SelectedY != null)
            {
                var chartItems = new List<ChartItem>();

                foreach (DataRow row in DataTable.Rows)
                {
                    var x = (double)row[SelectedX];
                    var y = (double)row[SelectedY];
                    chartItems.Add(new ChartItem(x, y));
                }

                chartItems = chartItems.OrderBy(x => x.X).ToList();
                var groups = chartItems.GroupBy(x => x.X).ToList();
                var chartItems2 = groups.Select(x => new ChartItem(x.Key, x.ToList().Average(y => y.Y))).ToList();

                Items = new ObservableCollection<ChartItem>(chartItems2);
            }

            OnPropertyChanged("Items");
            OnPropertyChanged("Columns");
            OnPropertyChanged("MaximimX");
            OnPropertyChanged("MinimumX");
            OnPropertyChanged("DataTable");
            OnPropertyChanged("SelectedX");
            OnPropertyChanged("SelectedY");
            OnPropertyChanged("SelectedZ");
        }
    }
}
