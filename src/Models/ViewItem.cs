using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeAnalyser
{
    public class ViewItem : INotifyPropertyChanged
    {
        public ObservableCollection<string> Columns { get; protected set; }
        public ChartData ChartData { get; protected set; }
        public HeatMapData HeatMapData { get; protected set; }
        public bool IsChartVisible { get; protected set; }
        public bool IsHeatMapVisible { get; protected set; }

        public ViewItem()
        {
            this.ChartData = new ChartData();
            this.HeatMapData = new HeatMapData();
        }

        // ---------------------------

        private DataTable _dataTable;
        public DataTable DataTable
        {
            get { return _dataTable; }
            set
            {
                _dataTable = value;
                var columns = _dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                columns.Insert(0, string.Empty);
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
            IsChartVisible = false;
            IsHeatMapVisible = false;

            if (!string.IsNullOrEmpty(SelectedX) && !string.IsNullOrEmpty(SelectedY))
            {
                if (string.IsNullOrEmpty(SelectedZ))
                {
                    ChartData.Update(DataTable, SelectedX, SelectedY);
                    IsChartVisible = true;
                }
                else
                {
                    HeatMapData.Update(DataTable, SelectedX, SelectedY, SelectedZ);
                    IsHeatMapVisible = true;
                }
            }

            OnPropertyChanged("ChartData");
            OnPropertyChanged("HeatMapData");
            OnPropertyChanged("IsChartVisible");
            OnPropertyChanged("IsHeatMapVisible");
        }
    }
}
