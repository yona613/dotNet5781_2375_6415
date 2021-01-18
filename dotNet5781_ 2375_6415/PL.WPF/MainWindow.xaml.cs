using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        /// <summary>
        /// Event when line is checked
        /// Display data grid of lines and add ew line button
        /// </summary>
        /// <param name="sender">Line Radio button</param>
        /// <param name="e"></param>
        private void LineChB_Checked(object sender, RoutedEventArgs e)
        {
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            stationDataGrid.Visibility = Visibility.Hidden;
            busDataGrid.Visibility = Visibility.Hidden;
            lineDataGrid.Visibility = Visibility.Visible;
            lineDataGrid.ItemsSource = linesList;
            AddBtn.IsEnabled = true;
            BtnTblock.Text = "Add new Line";
        }

        /// <summary>
        /// Event when bus is checked
        /// Display data grid of buses and add new bus button
        /// </summary>
        /// <param name="sender">bus Radio button</param>
        /// <param name="e"></param>
        private void BusChB_Checked(object sender, RoutedEventArgs e)
        {
            var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
            lineDataGrid.Visibility = Visibility.Hidden;
            stationDataGrid.Visibility = Visibility.Hidden;
            busDataGrid.Visibility = Visibility.Visible;
            busDataGrid.ItemsSource = busList;
            AddBtn.IsEnabled = true;
            BtnTblock.Text = "Add new Bus";
        }
        /// <summary>
        /// Event when station is checked
        /// Display data grid of stations and add new station button
        /// </summary>
        /// <param name="sender">bus station button</param>
        /// <param name="e"></param>
        private void StationChB_Checked(object sender, RoutedEventArgs e)
        {
            var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
            lineDataGrid.Visibility = Visibility.Hidden;
            busDataGrid.Visibility = Visibility.Hidden;
            stationDataGrid.Visibility = Visibility.Visible;
            stationDataGrid.ItemsSource = stationList;
            AddBtn.IsEnabled = true;
            BtnTblock.Text = "Add new Station";
        }

        /// <summary>
        /// Event when delete line button clicked
        /// send delete line query to bl
        /// and updates lines list
        /// </summary>
        /// <param name="sender">Delete line button</param>
        /// <param name="e"></param>
        private void DeleteLine_Click(object sender, RoutedEventArgs e)
        {
            bl.DeleteLine((((sender as Button).DataContext) as BO.BusLine).LineNumber);
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            lineDataGrid.ItemsSource = linesList;
        }


        /// <summary>
        /// Event when delete bus button clicked
        /// send delete bus query to bl
        /// and updates buses list
        /// </summary>
        /// <param name="sender">Delete bus button</param>
        /// <param name="e"></param>
        private void DeleteBus_Click(object sender, RoutedEventArgs e)
        {
            bl.DeleteBus((((sender as Button).DataContext) as Bus).License);
            var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
            busDataGrid.ItemsSource = busList;
        }


        /// <summary>
        /// Event when delete station button clicked
        /// send delete station query to bl
        /// and updates station list
        /// </summary>
        /// <param name="sender">Delete station button</param>
        /// <param name="e"></param>
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
            stationDataGrid.ItemsSource = stationList;
        }

        /// <summary>
        /// Event when update line button clicked
        /// opens update line window
        /// and updates lines list
        /// </summary>
        /// <param name="sender">Update line button</param>
        /// <param name="e"></param>
        private void UpdateLine_Click(object sender, RoutedEventArgs e)
        {
            BO.LineToShow myLineToShow = bl.GetBusLineToShow(((sender as Button).DataContext as BO.BusLine).LineNumber);
            (new UpdateLine(myLineToShow)).ShowDialog();
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            lineDataGrid.ItemsSource = linesList;
        }

        /// <summary>
        /// Event when update station button clicked
        /// opens update station window
        /// and updates stations list
        /// </summary>
        /// <param name="sender">Update station button</param>
        /// <param name="e"></param>
        private void updateStation_Click(object sender, RoutedEventArgs e)
        {
            new UpdateStation(((sender as Button).DataContext) as BO.Station).ShowDialog();
            var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
            stationDataGrid.ItemsSource = stationList;
        }

        /// <summary>
        /// Event when add button clicked
        /// depends opens window to add type that is checked
        /// </summary>
        /// <param name="sender">Add new button</param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lineChB.IsChecked == true)
            {
                new AddLine().ShowDialog();
                var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
                lineDataGrid.ItemsSource = linesList;
            }
            else if (busChB.IsChecked == true)
            {
                new AddBus().ShowDialog();
                var busList = bl.GetAllBuses().OrderBy(x => x.License).ToList();
                busDataGrid.ItemsSource = busList;
            }
            else if (stationChB.IsChecked == true)
            {
                new AddStation().ShowDialog();
                var stationList = bl.GetAllStations().OrderBy(x => x.StationId).ToList();
                stationDataGrid.ItemsSource = stationList;
            }
            else
            {
                MessageBox.Show("Please choose an object to add !!");
            }
        }


        /// <summary>
        /// Event when object double clicked in list
        /// opens data window of that object
        /// </summary>
        /// <param name="sender">Mouse double Click</param>
        /// <param name="e"></param>
        private void ListDoubleMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (busChB.IsChecked == true)
            {
                new BusData(busDataGrid.SelectedItem as BO.Bus).ShowDialog();
            }
            else if (stationChB.IsChecked == true)
            {
                //for process of digital panel, because we need to be able to cancell process when closing window
                BackgroundWorker digitalPanelBw = new BackgroundWorker();
                digitalPanelBw.WorkerSupportsCancellation = true;
                new StationData(bl.getStationToShow((stationDataGrid.SelectedItem as BO.Station).StationId), digitalPanelBw).ShowDialog();
                digitalPanelBw.CancelAsync();//when window closed cancel backgroundworker's work
            }
            else if (lineChB.IsChecked == true)
            {
                new LineData(MainWindow.bl.GetBusLineToShow((lineDataGrid.SelectedItem as BO.BusLine).LineNumber)).ShowDialog();
            }
        }
    }
}
