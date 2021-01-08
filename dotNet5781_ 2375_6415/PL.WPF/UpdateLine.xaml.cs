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
            /*try
            {
                stationsList.DataContext = MainWindow.bl.GetAllStationsOfLine(myLine.LineNumber);
                LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            } */         
        }

        private void addSttBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddLineStation(myLine).ShowDialog();
            myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
            MainGrid.DataContext = myLine;
            //stationsList.DataContext = MainWindow.bl.GetAllStationsOfLine(myLine.LineNumber);
        }

        private void deleteStt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.DeleteLineStation(((sender as Button).DataContext as BO.LineStationToShow).StationId, myLine.LineNumber);
                myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
                MainGrid.DataContext = myLine;
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

        private void addLineDepartingBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddLineDeparting(myLine.LineNumber).ShowDialog();
            myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
            MainGrid.DataContext = myLine;
            //LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
        }

        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myLine.LineArea = (BO.Area)(int)areaCboBox.SelectedItem;
                MainWindow.bl.UpdateLine(myLine, lineNumber);
                Close();
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

        private void deleteLineDeparting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.DeleteLineDeparting(myLine.LineNumber, ((sender as Button).DataContext as BO.LineDeparting).StartTime);
                myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
                MainGrid.DataContext = myLine;
                //LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
        }

        private void updateLineDepartingBtn_Click(object sender, RoutedEventArgs e)
        {
            new UpdateLineDeparting((sender as Button).DataContext as BO.LineDeparting).ShowDialog();
            myLine = MainWindow.bl.GetBusLineToShow(lineNumber);
            MainGrid.DataContext = myLine;
            //LineDepartingList.DataContext = MainWindow.bl.GetAllLineDepartingBy(x => x.LineNumber == myLine.LineNumber).OrderBy(x => x.StartTime).ToList();
        }
    }
}
