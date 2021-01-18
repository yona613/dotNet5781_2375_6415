using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using BLApi;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLineStation.xaml
    /// </summary>
    public partial class AddLineStationNewLine : Window
    {
        IBL bl = BLFactory.GetBL();
        BO.Station myStation = null;
        BO.LineToShow myLine;
        List<BO.Station> stations;
        //observable collection of new line's stations
        //to be able to update by adding new station
        ObservableCollection<BO.LineStationToShow> myLineStationsToShow;
        
        /// <summary>
        /// Gets all parameters from new line window
        /// to be able to update all collections and lines data
        /// </summary>
        /// <param name="tmpStations"></param>
        /// <param name="tmpLineStationsToShow"></param>
        /// <param name="tmpLine"></param>
        public AddLineStationNewLine(List<BO.Station> tmpStations, ObservableCollection<BO.LineStationToShow> tmpLineStationsToShow, BO.LineToShow tmpLine )
        {
            InitializeComponent();
            stationCbb.ItemsSource = MainWindow.bl.GetAllStationsToAdd(0);
            myLine = tmpLine;
            stations = tmpStations;
            myLineStationsToShow = tmpLineStationsToShow;
            indexLb.Content = myLineStationsToShow.Count + 1;
        }


        /// <summary>
        /// Event when Add station button clicked
        /// checks if old station or new stations and edits data depending n checked choice
        /// </summary>
        /// <param name="sender">Add button</param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (stationCb.IsChecked == true)//if new station
            {
                try
                {
                    bool flag = bl.CheckNewStation(myStation); //checks that station does'nt already exist
                    if (MainWindow.bl.CheckNewStation(myStation))
                    {
                        stations.Add(myStation); //adds new physical station to collection
                        myLineStationsToShow.Add(new BO.LineStationToShow()
                        {
                            Name = myStation.Name,
                            Address = myStation.Address,
                            Coordinates = myStation.Coordinates,
                            StationId = myStation.StationId,
                            Index = (int)indexLb.Content,
                            lineNumber = myLine.LineNumber
                        }
                        );//adds line station to collection
                        Close();
                    }
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
                catch (BO.BOBadStationException exception)
                {
                    MessageBox.Show(exception.Message);
                    numTb.Clear();
                }
            }
            else
            {
                try
                {
                    // adds new line station to collection 
                    BO.Station tmpStation = MainWindow.bl.GetStation((stationCbb.SelectedItem as BO.StationToAdd).StationId);
                    myLineStationsToShow.Add(new BO.LineStationToShow()
                    {
                        Name = tmpStation.Name,
                        Address = tmpStation.Address,
                        Coordinates = tmpStation.Coordinates,
                        StationId = tmpStation.StationId,
                        Index = (int)indexLb.Content,
                        lineNumber = myLine.LineNumber
                    }
                     );                  
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

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(myMap);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = myMap.ViewportPointToLocation(mousePosition);

            StationPoint.Location = pinLocation;
        }
    }
}
