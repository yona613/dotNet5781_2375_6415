using BLApi;
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
    /// Interaction logic for UpdateLine.xaml
    /// </summary>
    public partial class UpdateLine : Window
    {
        IBL bl = BLFactory.GetBL();
        int lineNumber;
        BO.LineToShow myLine;
        public UpdateLine(BO.LineToShow tmpLine)
        {
            InitializeComponent();
            myLine = tmpLine;
            MainGrid.DataContext = myLine;
            areaCboBox.SelectedIndex = (int)myLine.LineArea;
            areaCboBox.ItemsSource = Enum.GetValues(typeof(BO.Area));
            lineNumber = tmpLine.LineNumber;    
        }

        /// <summary>
        /// Event when Add station Button clicked
        /// opens add station window for that specific line
        /// </summary>
        /// <param name="sender">Add station Button</param>
        /// <param name="e"></param>
        private void addSttBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddLineStation(myLine).ShowDialog();
            myLine = bl.GetBusLineToShow(lineNumber);
            MainGrid.DataContext = myLine;
        }


        /// <summary>
        /// Event when Delete station Button clicked
        /// deletes the station from the line
        /// </summary>
        /// <param name="sender">Delete station Button</param>
        /// <param name="e"></param>
        private void deleteStt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteLineStation(((sender as Button).DataContext as BO.LineStationToShow).StationId, myLine.LineNumber);
                myLine = bl.GetBusLineToShow(lineNumber);
                MainGrid.DataContext = myLine;
            }
            catch (BO.BOLineDeleteException lineException)
            {
                MessageBox.Show(lineException.Message);
                this.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Event when Add Line Departing Button clicked
        /// opens add Line Departing window for that specific line
        /// </summary>
        /// <param name="sender">Add Line Departing Button</param>
        /// <param name="e"></param>
        private void addLineDepartingBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddLineDeparting(myLine.LineNumber).ShowDialog();
            myLine = bl.GetBusLineToShow(lineNumber);
            MainGrid.DataContext = myLine;
        }


        /// <summary>
        /// Evenet when apply button clicked
        /// Updates line by sending update to bl
        /// closes the window
        /// </summary>
        /// <param name="sender">Apply button</param>
        /// <param name="e"></param>
        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myLine.LineArea = (BO.Area)(int)areaCboBox.SelectedItem;
                bl.UpdateLine(myLine, lineNumber);
                Close();
            }
            catch (BO.BOBadLineException lineException)
            {
                myLine.LineNumber = lineNumber;
                MessageBox.Show(lineException.Message);
            }
        }


        /// <summary>
        /// Event when Delete Line Departing Button clicked
        /// deletes Line Departing from that line
        /// </summary>
        /// <param name="sender">Delete Line Departing Button</param>
        /// <param name="e"></param>
        private void deleteLineDeparting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.DeleteLineDeparting(myLine.LineNumber, ((sender as Button).DataContext as BO.LineDeparting).StartTime);
                myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
                MainGrid.DataContext = myLine;
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Event when Update Line Departing Button clicked
        /// opens update line departing window
        /// </summary>
        /// <param name="sender">Update Line Departing Button</param>
        /// <param name="e"></param>
        private void updateLineDepartingBtn_Click(object sender, RoutedEventArgs e)
        {
            new UpdateLineDeparting((sender as Button).DataContext as BO.LineDeparting).ShowDialog();
            myLine = bl.GetBusLineToShow(lineNumber);
            MainGrid.DataContext = myLine;
        }

        /// <summary>
        /// Event when update distance Button clicked
        /// opens update distance window
        /// </summary>
        /// <param name="sender">update distance Button</param>
        /// <param name="e"></param>
        private void distanceBtn_Click(object sender, RoutedEventArgs e)
        {
            new DistanceTimeInput((((sender as Button).DataContext) as BO.LineStationToShow), lineNumber).ShowDialog();
            myLine = bl.GetBusLineToShow(lineNumber);
            MainGrid.DataContext = myLine;
        }
    }
}
