using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for StationData.xaml
    /// </summary>
    public partial class StationData : Window
    {
        BO.Station myStation;
        public StationData(BO.Station tmpStation)
        {
            InitializeComponent();
            myStation = tmpStation;
            mainGrid.DataContext = tmpStation;
            lineList.DataContext = MainWindow.bl.GetAllLinesOfStation(tmpStation.StationId);
        }
    }
}
