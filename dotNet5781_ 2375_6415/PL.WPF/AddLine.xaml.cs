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
        BO.LineToShow myLine;
        ObservableCollection<BO.LineStationToShow> stationsToShow = new ObservableCollection<BO.LineStationToShow>();
        List<BO.Station> stations = new List<BO.Station>() { };

        
        public AddLine()
        {
            InitializeComponent();
            myLine = new BO.LineToShow();
            MainGrid.DataContext = myLine;
            areaCboBox.ItemsSource = Enum.GetValues(typeof(BO.Area));
            stationsList.ItemsSource = stationsToShow;
        }

        private void AddSttBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddLineStationNewLine(stations, stationsToShow, myLine).ShowDialog();
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
            catch (BO.BOBadLineNumberException lineException)
            {
                MessageBox.Show(lineException.Message);
            }
            catch (BO.BOBadLineException lineException)
            {
                MessageBox.Show(lineException.Message);
            }
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