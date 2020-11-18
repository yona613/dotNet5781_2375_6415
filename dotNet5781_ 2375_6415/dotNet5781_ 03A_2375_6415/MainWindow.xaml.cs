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

namespace dotNet5781_03A_2375_6415
{

    using static Math;


    /// <summary>
    /// Assigning the bus line to a specific area from a defined area list
    /// or be cross-areas (general)/// </summary>
    enum Area { General, North, South, Center, Jerusalem };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusLinesList myList = new BusLinesList(); //creates a list of lines
        private Line currentDisplayBusLine;

        public MainWindow()
        {
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

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as Line).LineNumber);
        }

        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = myList[index].myList.First();
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.stations;
        }

        static public int getIntInput()
        {
            string tmpString;
            int tmpNum;
            do
            {
                tmpString = Console.ReadLine(); // reads the input
                try
                {
                    if (!int.TryParse(tmpString, out tmpNum)) // didn't succeed to switch the input to integer
                    {
                        throw new InvalidCastException("Invalid input !!");
                    }
                    break;
                }
                catch (InvalidCastException ex)
                {
                    Console.WriteLine($" ERROR : {ex.ToString()}");
                }
            } while (true);
            return tmpNum;
        }

    }
}
