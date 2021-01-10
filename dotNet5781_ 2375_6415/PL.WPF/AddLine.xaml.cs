using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLine.xaml
    /// </summary>
    public partial class AddLine : Window
    {
        int lineNumber;
        BO.LineToShow myLine;
        //BO.LineDeparting myLineDeparting;
        //BO.Station myFirst;
        //BO.Station myLast;
        //ObservableCollection<BO.LineDeparting> lineDepartings = new ObservableCollection<BO.LineDeparting>();
        ObservableCollection<BO.LineStationToShow> stationsToShow = new ObservableCollection<BO.LineStationToShow>();
        List<BO.Station> stations = new List<BO.Station>() { };
        //List<BO.LineStation> lineStations = new List<BO.LineStation>() { };
        //List<BO.PairStations> pairStations = new List<BO.PairStations>() { };

        
        public AddLine()
        {
            InitializeComponent();
            myLine = new BO.LineToShow();
            //myLine.FirstStation = tmpFirst.StationId;
            //myLine.LastStation = tmpLast.StationId;
            //lineToShows.Add(myLine);


            MainGrid.DataContext = myLine;
            areaCboBox.ItemsSource = Enum.GetValues(typeof(BO.Area));
            stationsList.ItemsSource = stationsToShow;
            //lineNumber = tmpLine.LineNumber;
            /*try
            s{
                stationsList.DataContext = MainWindow.bl.GetAllStationsOfLine(myLine.LineNumber);
                LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            } */
        }

        private void AddSttBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddLineStationNewLine(stations, stationsToShow, myLine).ShowDialog();
            //BO.LineToShow showLine = (BO.LineToShow)myLine;
            //new AddLineStation(myLine).ShowDialog();
            //myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
            //MainGrid.DataContext = myLine;
            //stationsList.DataContext = MainWindow.bl.GetAllStationsOfLine(myLine.LineNumber);
        }

        private void DeleteStt_Click(object sender, RoutedEventArgs e)
        {            
            for (int i = ((sender as Button).DataContext as BO.LineStationToShow).Index - 1; i < stationsToShow.Count; i++)
            {
                stationsToShow[i].Index--;
            }
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].StationId == ((sender as Button).DataContext as BO.LineStationToShow).StationId)
                {
                    stations.RemoveAt(i);
                }
            }
            stationsToShow.Remove((sender as Button).DataContext as BO.LineStationToShow);
            stationsList.Items.Refresh();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.AddLine(myLine, stations.ToList(), stationsToShow.ToList());
                Close();
            }
            catch (BO.BONewLineInsuffisantStationsException lineException)
            {
                MessageBox.Show(lineException.Message);
            }
            catch (BO.BOBadLineException lineException)
            {
                MessageBox.Show(lineException.Message);
            }
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message);
            //}
        }

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