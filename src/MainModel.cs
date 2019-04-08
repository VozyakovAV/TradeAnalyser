using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TradeAnalyser
{
    public class MainModel : INotifyPropertyChanged
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        // ---------------------------

        private DataTable _dataTable;
        public DataTable DataTable
        {
            get { return _dataTable; }
            set
            {
                _dataTable = value;
                OnPropertyChanged("DataTable");
            }
        }

        // ---------------------------

        private ChartData _chartData1;
        public ChartData ChartData1
        {
            get { return _chartData1; }
            set
            {
                _chartData1 = value;
                OnPropertyChanged("ChartData1");
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
    }
}
