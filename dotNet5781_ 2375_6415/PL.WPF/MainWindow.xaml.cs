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
        public static IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            bl = BLFactory.GetBL();
        }

        private void LineChB_Checked(object sender, RoutedEventArgs e)
        {
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            ListLB.Visibility = Visibility.Visible;
            busChB.IsChecked = false;
            stationChB.IsChecked = false;
            ListLB.DataContext = linesList;
            AddBtn.IsEnabled = true;
            BtnTblock.Text = "Add new Line";
        }

        private void LineChB_Unchecked(object sender, RoutedEventArgs e)
        {         
            ListLB.Visibility = Visibility.Collapsed;
            BtnTblock.Text = "Please choose a object to add";
            AddBtn.IsEnabled = false;
        }

        private void BusChB_Checked(object sender, RoutedEventArgs e)
        {
            var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
            ListB.Visibility = Visibility.Visible;
            lineChB.IsChecked = false;
            stationChB.IsChecked = false;
            ListB.DataContext = busList;
            AddBtn.IsEnabled = true;
            BtnTblock.Text = "Add new Bus";
        }

        private void BusChB_Unchecked(object sender, RoutedEventArgs e)
        {
            ListB.Visibility = Visibility.Collapsed;
            BtnTblock.Text = "Please choose a object to add";
            AddBtn.IsEnabled = false;
        }

        private void StationChB_Checked(object sender, RoutedEventArgs e)
        {
            var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
            ListS.Visibility = Visibility.Visible;
            busChB.IsChecked = false;
            lineChB.IsChecked = false;
            ListS.DataContext = stationList;
            AddBtn.IsEnabled = true;
            BtnTblock.Text = "Add new Station";
        }

        private void StationChB_Unchecked(object sender, RoutedEventArgs e)
        {
            ListS.Visibility = Visibility.Collapsed;
            BtnTblock.Text = "Please choose a object to add";
            AddBtn.IsEnabled = false;
        }

        private void DeleteLine_Click(object sender, RoutedEventArgs e)
        {
            bl.DeleteLine((((sender as Button).DataContext) as BO.BusLine).LineNumber);
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            ListLB.DataContext = linesList;
        }

        private void DeleteBus_Click(object sender, RoutedEventArgs e)
        {
            bl.DeleteBus((((sender as Button).DataContext) as BO.Bus).License);
            var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
            ListB.DataContext = busList;
        }

        private void DeleteStation_Click(object sender, RoutedEventArgs e)
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

        private void UpdateLine_Click(object sender, RoutedEventArgs e)
        {
            BO.LineToShow myLineToShow = bl.GetBusLineToShow(((sender as Button).DataContext as BO.BusLine).LineNumber);
            (new UpdateLine(myLineToShow)).ShowDialog();
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            ListLB.DataContext = linesList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lineChB.IsChecked == true)
            {

            }
            else if (busChB.IsChecked == true)
            {
                new AddBus().ShowDialog();
                var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
                ListB.DataContext = busList;
            }
            else if (stationChB.IsChecked == true)
            {
                new AddStation().ShowDialog();
                var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
                ListS.DataContext = stationList;
            }
            else
            {
                MessageBox.Show("Please choose an object to add !!");
            }
        }

        private void updateStation_Click(object sender, RoutedEventArgs e)
        {
            new UpdateStation(((sender as Button).DataContext) as BO.Station).ShowDialog();
            var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
            ListS.DataContext = stationList;
        }
    }
}
