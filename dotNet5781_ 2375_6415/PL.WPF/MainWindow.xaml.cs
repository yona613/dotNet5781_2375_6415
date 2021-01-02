using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
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
using BLApi;
using BO;
using ViewModel;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<BusLine> listOfLines = new ObservableCollection<BusLine>() { };
        public static IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            bl = BLFactory.GetBL();
        }

        private void lineChB_Checked(object sender, RoutedEventArgs e)
        {
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            ListLB.Visibility = Visibility.Visible;
            busChB.IsChecked = false;
            stationChB.IsChecked = false;
            ListLB.DataContext = linesList;
        }

        private void lineChB_Unchecked(object sender, RoutedEventArgs e)
        {         
            ListLB.Visibility = Visibility.Collapsed;
        }

        private void busChB_Checked(object sender, RoutedEventArgs e)
        {
            var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
            ListB.Visibility = Visibility.Visible;
            lineChB.IsChecked = false;
            stationChB.IsChecked = false;
            ListB.DataContext = busList;
        }

        private void busChB_Unchecked(object sender, RoutedEventArgs e)
        {
            ListB.Visibility = Visibility.Collapsed;
        }

        private void stationChB_Checked(object sender, RoutedEventArgs e)
        {
            var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
            ListS.Visibility = Visibility.Visible;
            busChB.IsChecked = false;
            lineChB.IsChecked = false;
            ListS.DataContext = stationList;
        }

        private void stationChB_Unchecked(object sender, RoutedEventArgs e)
        {
            ListS.Visibility = Visibility.Collapsed;
        }

        private void deleteLine_Click(object sender, RoutedEventArgs e)
        {
            bl.DeleteLine((((sender as Button).DataContext) as BO.BusLine).LineNumber);
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            ListLB.DataContext = linesList;
        }

        private void deleteBus_Click(object sender, RoutedEventArgs e)
        {
            bl.DeleteBus((((sender as Button).DataContext) as BO.Bus).License);
            var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
            ListB.DataContext = busList;
        }

        private void deleteStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteStation((((sender as Button).DataContext) as BO.Station).StationId);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            
            var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
            ListS.DataContext = stationList;
        }

        private void updateLine_Click(object sender, RoutedEventArgs e)
        {
            BO.LineToShow myLineToShow = bl.GetBusLineToShow(((sender as Button).DataContext as BO.BusLine).LineNumber);
            (new UpdateLine(myLineToShow)).ShowDialog();
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            ListLB.DataContext = linesList;
        }
    }
}
