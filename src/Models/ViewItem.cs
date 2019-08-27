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

        private IList<DataRow> _rowsAll;
        private IList<DataRow> _rowsView;

        private DataTable _dataTable;
        public DataTable DataTable
        {
            get { return _dataTable; }
            set
            {
                _dataTable = value;
                _rowsAll = _dataTable.Rows.Cast<DataRow>().ToList();
                _rowsView = _rowsAll;
                var columns = _dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                columns.Insert(0, string.Empty);
                Columns = new ObservableCollection<string>(columns);
#if DEBUG
                var columnX = columns.FirstOrDefault(x => x.Contains("BarsExit"));
                var columnY = columns.FirstOrDefault(x => x.Contains("Средний П/У в %"));

                if (columnX != null && columnY != null)
                {
                    SelectedX = columnX;
                    SelectedY = columnY;
                }
#endif
                Update();
            }
        }

        // ---------------------------

        #region Filters

        private string _filterColumn1;
        public string FilterColumn1
        {
            get { return _filterColumn1; }
            set
            {
                _filterColumn1 = value;

                if (string.IsNullOrEmpty(_filterColumn1))
                {
                    _filterValues1 = null;
                }
                else
                {
                    var values = _rowsAll.Select(x => (double)x[_filterColumn1]).Distinct().OrderBy(x => x).ToList();
                    _filterValues1 = new ObservableCollection<double>(values);
                }

                _filterSelected1 = null;
                Update();
            }
        }

        // ---------------------------

        private double? _filterSelected1;
        public double? FilterSelected1
        {
            get { return _filterSelected1; }
            set
            {
                _filterSelected1 = value;
                FilterRows();
                Update();
            }
        }

        // ---------------------------

        private ObservableCollection<double> _filterValues2;
        public ObservableCollection<double> FilterValues2
        {
            get { return _filterValues2; }
            set
            {
                _filterValues2 = value;
                Update();
            }
        }

        // ---------------------------

        private string _filterColumn2;
        public string FilterColumn2
        {
            get { return _filterColumn2; }
            set
            {
                _filterColumn2 = value;

                if (string.IsNullOrEmpty(_filterColumn2))
                {
                    _filterValues2 = null;
                }
                else
                {
                    var values = _rowsAll.Select(x => (double)x[_filterColumn2]).Distinct().OrderBy(x => x).ToList();
                    _filterValues2 = new ObservableCollection<double>(values);
                }

                _filterSelected2 = null;
                Update();
            }
        }

        // ---------------------------

        private double? _filterSelected2;
        public double? FilterSelected2
        {
            get { return _filterSelected2; }
            set
            {
                _filterSelected2 = value;
                FilterRows();
                Update();
            }
        }

        private void FilterRows()
        {
            var rows = _rowsAll.AsEnumerable();

            if (!string.IsNullOrEmpty(FilterColumn1) && FilterSelected1 != null)
            {
                rows = rows.Where(x => (double)x[FilterColumn1] == FilterSelected1);
            }

            if (!string.IsNullOrEmpty(FilterColumn2) && FilterSelected2 != null)
            {
                rows = rows.Where(x => (double)x[FilterColumn2] == FilterSelected2);
            }

            _rowsView = rows.ToList();
        }

        // ---------------------------

        private ObservableCollection<double> _filterValues1;
        public ObservableCollection<double> FilterValues1
        {
            get { return _filterValues1; }
            set
            {
                _filterValues1 = value;
                Update();
            }
        }

        #endregion

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
                    ChartData.Update(_rowsView, SelectedX, SelectedY);
                    IsChartVisible = true;
                }
                else
                {
                    HeatMapData.Update(_rowsView, SelectedX, SelectedY, SelectedZ);
                    IsHeatMapVisible = true;
                }
            }

            OnPropertyChanged("FilterColumn1");
            OnPropertyChanged("FilterSelected1");
            OnPropertyChanged("FilterValues1");

            OnPropertyChanged("FilterColumn2");
            OnPropertyChanged("FilterSelected2");
            OnPropertyChanged("FilterValues2");

            OnPropertyChanged("ChartData");
            OnPropertyChanged("HeatMapData");
            OnPropertyChanged("IsChartVisible");
            OnPropertyChanged("IsHeatMapVisible");
        }
    }
}
