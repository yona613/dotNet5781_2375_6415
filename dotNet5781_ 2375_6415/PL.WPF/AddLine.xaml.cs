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
        BO.LineDeparting myLineDeparting;
        BO.Station myFirst;
        BO.Station myLast;
        ObservableCollection<BO.LineDeparting> lineDepartings = new ObservableCollection<BO.LineDeparting>();
        ObservableCollection<BO.StationToShow> stationsToShows = new ObservableCollection<BO.StationToShow>();
        List<BO.Station> stations = new List<BO.Station>() { };
        List<BO.LineStation> lineStations = new List<BO.LineStation>() { };

        
        public AddLine()
        {
            InitializeComponent();
            myLine = new BO.LineToShow();
            //myLine.FirstStation = tmpFirst.StationId;
            //myLine.LastStation = tmpLast.StationId;
            //lineToShows.Add(myLine);


            MainGrid.DataContext = myLine;
            areaCboBox.ItemsSource = Enum.GetValues(typeof(BO.Area));
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
            new AddLineStationNewLine(stations, lineStations, myLine);
            //BO.LineToShow showLine = (BO.LineToShow)myLine;
            //new AddLineStation(myLine).ShowDialog();
            //myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
            //MainGrid.DataContext = myLine;
            //stationsList.DataContext = MainWindow.bl.GetAllStationsOfLine(myLine.LineNumber);
        }

        private void DeleteStt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //MainWindow.bl.DeleteLineStation(((sender as Button).DataContext as BO.LineStationToShow).StationId, myLine.LineNumber);
                //myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
                //MainGrid.DataContext = myLine;
                //stationsList.DataContext = MainWindow.bl.GetAllStationsOfLine(myLine.LineNumber);
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

        private void AddLineDepartingBtn_Click(object sender, RoutedEventArgs e)
        {
            //myLineDeparting.LineNumber = myLine.LineNumber;
            //lineDepartings.Add(myLineDeparting);

            //new AddLineDeparting(myLine.LineNumber).ShowDialog();
            //myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
            //MainGrid.DataContext = myLine;
            //LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //myLine.LineArea = (BO.Area)(int)areaCboBox.SelectedItem;
                //MainWindow.bl.AddLine(myLine, lineNumber);
                //Close();
            }
            catch (BO.BOBadLineException lineException)
            {
                myLine.LineNumber = lineNumber;
                MessageBox.Show(lineException.Message);
            }
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message);
            //}
        }

        private void DeleteLineDeparting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // MainWindow.bl.DeleteLineDeparting(myLine.LineNumber, ((sender as Button).DataContext as BO.LineDeparting).StartTime);
                //myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
                //MainGrid.DataContext = myLine;
                //LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
        }

        private void UpdateLineDepartingBtn_Click(object sender, RoutedEventArgs e)
        {
            //new UpdateLineDeparting((sender as Button).DataContext as BO.LineDeparting).ShowDialog();
            //myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
           // MainGrid.DataContext = myLine;
            //LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
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