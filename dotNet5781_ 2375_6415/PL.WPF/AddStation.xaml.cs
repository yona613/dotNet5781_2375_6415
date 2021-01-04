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
using Microsoft.Maps.MapControl.WPF;
using System.Device.Location;
using System.Text.RegularExpressions;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLineStation.xaml
    /// </summary>
    public partial class AddStation : Window

    {
        BO.Station myStation = null;
        public AddStation()
        {
            InitializeComponent();
            LongitudeTb.Text = "00,0000";
            LatitudeTb.Text = "00,0000";
            myStation = new BO.Station();
            newStationGrid.DataContext = myStation;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myStation.Coordinates = new Location() { Longitude = double.Parse(LongitudeTb.Text), Latitude = double.Parse(LatitudeTb.Text) };
                MainWindow.bl.AddStation(myStation);
                Close();
            }
            catch (BO.BOBadStationException stationException)
            {
                MessageBox.Show(stationException.Message);
                numberTb.Clear();

            }
            catch (BO.BOBadStationCoordinatesLongitudeException longitudeException)
            {
                MessageBox.Show(longitudeException.Message);
                LongitudeTb.Text = "00,0000";
            }
            catch (BO.BOBadStationCoordinatesLatitudeException latitudeException)
            {
                MessageBox.Show(latitudeException.Message);
                LatitudeTb.Text = "00,0000";
            }
            catch (BO.BOBadStationNumberException numberException)
            {
                MessageBox.Show(numberException.Message);
                numberTb.Clear();
            }
            catch (BO.BOBadStationNameException nameException)
            {
                MessageBox.Show(nameException.Message);
            }
            catch (BO.BOBadStationAddressException addressException)
            {
                MessageBox.Show(addressException.Message);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
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
