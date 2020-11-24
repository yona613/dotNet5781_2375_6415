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
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_02_2375_6415;

namespace dotNet5781_03A_2375_6415
{

    /// <summary>
    /// Assigning the bus line to a specific area from a defined area list
    /// or be cross-areas (general)/// </summary>
    public enum Area { General, North, South, Center, Jerusalem };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BusLinesList myList = new BusLinesList(); //creates a list of lines
        private BusLine currentDisplayBusLine; //gets current Line

        public MainWindow()
        {
            //initialazing list of Bus Lines
            for (int i = 0; i < 10; i++) //adds lines
            {
                myList.AddLine(i + 1);
            }
            for (int i = 0; i < 10; i++) //adds stations
            {
                int k = 0;
                int tmp = 0;
                for (int j = 0; j < 20; j++)
                {
                    tmp = myRandom.r.Next(1, 41);
                    try
                    {
                        myList.AddStation(i + 1, k, tmp);
                    }
                    // catches arr empties for the boot continuity of stations and lines and to avoid duplication
                    catch (ArgumentOutOfRangeException ex)
                    {
                    }
                    catch (NotFoundException ex1)
                    {
                    }
                    catch (ArgumentException ex2)
                    {
                    }
                    k = tmp;
                }
            }

            InitializeComponent();
            cbBusLines.ItemsSource = myList;
            cbBusLines.DisplayMemberPath = "LineNumber";
            cbBusLines.SelectedIndex = 0;
        }

        /// <summary>
        /// Event when line is changed
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">param changed</param>
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //use the show bus line function to show data of bus line
            ShowBusLine((cbBusLines.SelectedValue as BusLine).LineNumber);
        }

        /// <summary>
        /// Function that shows data of bus line on the grid
        /// </summary>
        /// <param name="index"></param>
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = myList[index].myList.First(); //gets bus line to show
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
            tbArea.Text = myList[index].myList.First().BusArea.ToString();
        }

        /// <summary>
        /// Event when exit button is pressed to close program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
