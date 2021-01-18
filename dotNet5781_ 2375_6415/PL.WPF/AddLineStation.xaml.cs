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
using System.Device.Location;
using Microsoft.Maps.MapControl.WPF;
using System.Text.RegularExpressions;
using BLApi;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLineStation.xaml
    /// </summary>
    public partial class AddLineStation : Window
    {
        //station to get input from window
        BO.Station myStation = null;
        BO.LineToShow myLine;
        IBL bl = BLFactory.GetBL();

        public AddLineStation(BO.LineToShow tmpLine)
        {
            InitializeComponent();
            //get all stations we can add to line
            stationCbb.ItemsSource = MainWindow.bl.GetAllStationsToAdd(tmpLine.LineNumber);
            myLine = tmpLine;
            //get indexes we can add to line
            indexCb.ItemsSource = MainWindow.bl.GetAllIndexesToAdd(myLine.LineNumber);
        }

        /// <summary>
        /// Event when add button clicked
        /// adds station to line
        /// if new station then adds physical station to data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (stationCb.IsChecked == true) //if new physical station
            {
                try
                {
                    bl.AddStation(myStation); //add station to data
                    BO.LineStation lineStation = new BO.LineStation() { Index = (int)indexCb.SelectedItem, LineNumber = myLine.LineNumber, StationNumber = myStation.StationId };
                    bl.AddStationToLine(lineStation); //add line station to line
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
            else
            {
                try
                {
                    //only adds line station
                    bl.AddStationToLine(new BO.LineStation() { Index = (int)indexCb.SelectedItem, LineNumber = myLine.LineNumber, StationNumber = (stationCbb.SelectedItem as BO.StationToAdd).StationId });
                    Close();
                }
                catch(BO.BOBadLineStationException lineStationException)
                {
                    MessageBox.Show(lineStationException.Message);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        /// <summary>
        /// Event when new station checkbox checked
        /// enables input of new station data
        /// </summary>
        /// <param name="sender">New station checkbox</param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            myStation = new BO.Station();
            myStation.Coordinates = new Location() { Longitude = 35.203165854, Latitude = 31.772663576 };
            myMap.Center = myStation.Coordinates;
            newStationGrid.IsEnabled = true;
            stationCbb.IsEnabled = false;
            newStationGrid.DataContext = myStation;
        }

        /// <summary>
        /// Event when new station checkbox unchecked
        /// disables input of new station data
        /// </summary>
        /// <param name="sender">New station checkbox</param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            stationCbb.IsEnabled = true;
            newStationGrid.IsEnabled = false;
            myStation = null;
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

            // Determine the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(myMap);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = myMap.ViewportPointToLocation(mousePosition);

            StationPoint.Location = pinLocation;
        }
    }
}
