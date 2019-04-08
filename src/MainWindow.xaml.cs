using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TradeAnalyser
{
    public partial class MainWindow : Window
    {
        protected MainModel State { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.State = new MainModel();
            this.DataContext = State;

#if DEBUG
            LoadFile(@"D:\Data\Дела\Алго\TSLab\ТИ2_А3\Eдлин.csv");
#endif
        }

        private void SelectFolderClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Файл (*.csv)|*.csv";

            if (dlg.ShowDialog(this) == true)
            {
                State.FilePath = dlg.FileName;
                LoadFile(State.FilePath);
            }
        }

        private void LoadFile(string file)
        {
            if (File.Exists(file))
            {
                State.DataTable = ParseFileData.Parse(file);

                State.ChartData1 = new ChartData()
                {
                    DataTable = State.DataTable
                };

            }
        }
    }
}
