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
using BLApi;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLineStation.xaml
    /// </summary>
    public partial class AddStation : Window
    {
        IBL bl = BLFactory.GetBL();
        //station gets input from window
        BO.Station myStation = null;
        public AddStation()
        {
            InitializeComponent();
            myStation = new BO.Station();
            //initializes the map
            myStation.Coordinates = new Location() { Longitude = 35.203165854 , Latitude = 31.772663576 };
            mainGrid.DataContext = myStation;
            myMap.Center = myStation.Coordinates;
        }

        /// <summary>
        /// Event when add button clicked
        /// adds physical station to data
        /// </summary>
        /// <param name="sender">Add Button</param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.AddStation(myStation);
                Close();
            }
            catch (BO.BOBadStationException stationException)
            {
                MessageBox.Show(stationException.Message);
                numTb.Clear();

            }
            catch (BO.BOBadStationCoordinatesLongitudeException longitudeException)
            {
                MessageBox.Show(longitudeException.Message);
            }
            catch (BO.BOBadStationCoordinatesLatitudeException latitudeException)
            {
                MessageBox.Show(latitudeException.Message);
            }
            catch (BO.BOBadStationNumberException numberException)
            {
                MessageBox.Show(numberException.Message);
                numTb.Clear();
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

        /// <summary>
        /// Implementation of double click on map
        /// get coordinates of mouse double click and updates station coordinates
        /// </summary>
        /// <param name="sender">Mouse double click</param>
        /// <param name="e"></param>
        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(myMap);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = myMap.ViewportPointToLocation(mousePosition);

            StationPoint.Location = pinLocation;
        }
    }
}
