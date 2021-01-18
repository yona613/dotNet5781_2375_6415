using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public static IBL bl;
        public UserWindow()
        {
            InitializeComponent();
            bl = BLFactory.GetBL();
        }

        /// <summary>
        /// Event when line is checked
        /// Display data grid of lines
        /// </summary>
        /// <param name="sender">Line Radio button</param>
        /// <param name="e"></param>
        private void LineChB_Checked(object sender, RoutedEventArgs e)
        {
            var linesList = bl.GetAllBusLines().OrderBy(x => x.LineNumber).ToList();
            stationDataGrid.Visibility = Visibility.Hidden;
            lineDataGrid.Visibility = Visibility.Visible;
            lineDataGrid.ItemsSource = linesList;
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
            stationDataGrid.Visibility = Visibility.Visible;
            stationDataGrid.ItemsSource = stationList;
        }
                
        /// <summary>
        /// Event when object double clicked in list
        /// opens data window of that object
        /// </summary>
        /// <param name="sender">Mouse double Click</param>
        /// <param name="e"></param>
        private void ListDoubleMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (stationChB.IsChecked == true)
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