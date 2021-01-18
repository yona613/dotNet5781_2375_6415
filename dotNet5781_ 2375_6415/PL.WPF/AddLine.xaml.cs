using BLApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLine.xaml
    /// </summary>
    public partial class AddLine : Window
    {
        BO.LineToShow myLine;
        //collection to get new LineStations for new line
        ObservableCollection<BO.LineStationToShow> stationsToShow = new ObservableCollection<BO.LineStationToShow>();
        //collection to get new Physical Stations for new line
        List<BO.Station> stations = new List<BO.Station>() { };

        IBL bl = BLFactory.GetBL();
        
        public AddLine()
        {
            InitializeComponent();
            myLine = new BO.LineToShow();
            MainGrid.DataContext = myLine;
            areaCboBox.ItemsSource = Enum.GetValues(typeof(BO.Area));
            stationsList.ItemsSource = stationsToShow;
        }

        /// <summary>
        /// Event when Add station Button clicked
        /// opens add station window for that specific line
        /// </summary>
        /// <param name="sender">Add station Button</param>
        /// <param name="e"></param>
        private void AddSttBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddLineStationNewLine(stations, stationsToShow, myLine).ShowDialog();
        }

        /// <summary>
        /// Event when Delete station Button clicked
        /// deletes the station from collections of line
        /// </summary>
        /// <param name="sender">Delete station Button</param>
        /// <param name="e"></param>
        private void DeleteStt_Click(object sender, RoutedEventArgs e)
        {            
            for (int i = ((sender as Button).DataContext as BO.LineStationToShow).Index - 1; i < stationsToShow.Count; i++)
            {
                stationsToShow[i].Index--; //updates all indexes after the station removed
            }
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].StationId == ((sender as Button).DataContext as BO.LineStationToShow).StationId)
                {
                    stations.RemoveAt(i); //removes station from collections
                }
            }
            stationsToShow.Remove((sender as Button).DataContext as BO.LineStationToShow);
            stationsList.Items.Refresh();
        }


        /// <summary>
        /// Event when submit button clicked
        /// adds station to data by querying bl
        /// </summary>
        /// <param name="sender">Submit Button</param>
        /// <param name="e"></param>
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddLine(myLine, stations.ToList(), stationsToShow.ToList());
                Close();
            }
            catch (BO.BONewLineInsuffisantStationsException lineException)
            {
                MessageBox.Show(lineException.Message);
            }
            catch (BO.BOBadLineNumberException lineException)
            {
                MessageBox.Show(lineException.Message);
            }
            catch (BO.BOBadLineException lineException)
            {
                MessageBox.Show(lineException.Message);
            }
        }

        /// <summary>
        /// Raised on every key of keyboard before it is displayed on screen
        /// used to check that only digits are entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "\r") //checks that key wasn't enter
            {
                Regex myReg = new Regex("[^0-9]+"); //gets regular expression that allows only digits
                e.Handled = myReg.IsMatch(e.Text); //checks taht key entered is regular expression
            }
            if (e.Handled) //if not regular expression
            {
                MessageBox.Show($"Wrong Input !!!! \n {e.Text} is not a digit !!");
            }
        }
    }
}