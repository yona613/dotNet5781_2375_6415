﻿using System;
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
using System.Collections.ObjectModel;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLineStation.xaml
    /// </summary>
    public partial class AddLineStationNewLine : Window

    {
        BO.Station myStation = null;
        BO.LineToShow myLine;
        List<BO.Station> stations;
        //List<BO.LineStation> lineStations;
        //List<BO.PairStations> pairStations;

        ObservableCollection<BO.LineStationToShow> myLineStationsToShow;
        public AddLineStationNewLine(List<BO.Station> tmpStations, ObservableCollection<BO.LineStationToShow> tmpLineStationsToShow, BO.LineToShow tmpLine )
        {
            InitializeComponent();
            stationCbb.ItemsSource = MainWindow.bl.GetAllStationsToAdd(0);
            myLine = tmpLine;
            //lineStations = tmpLineStations;
            stations = tmpStations;
            myLineStationsToShow = tmpLineStationsToShow;
            //pairStations = tmpPairStations;
            indexLb.Content = myLineStationsToShow.Count + 1;
            //indexCb.ItemsSource = MainWindow.bl.GetAllIndexesToAdd(myLine.LineNumber);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (stationCb.IsChecked == true)
            {
                try
                {
                    //myStation.Coordinates = new Location() { Longitude = double.Parse(LongitudeTb.Text), Latitude = double.Parse(LatitudeTb.Text) };
                    //MainWindow.bl.AddStation(myStation);
                    //BO.LineStation lineStation = new BO.LineStation() { Index = lineStations.Count + 1, LineNumber = myLine.LineNumber, StationNumber = myStation.StationId };
                    //lineStations.Add(lineStation);
                    //MainWindow.bl.AddStationToLine(lineStation);
                    try
                    {
                        bool flag = MainWindow.bl.CheckNewStation(myStation);
                        if (MainWindow.bl.CheckNewStation(myStation))
                        {
                            stations.Add(myStation);
                            myLineStationsToShow.Add(new BO.LineStationToShow()
                            {
                                Name = myStation.Name,
                                Address = myStation.Address,
                                Coordinates = myStation.Coordinates,
                                StationId = myStation.StationId,
                                Index = (int)indexLb.Content,
                                lineNumber = myLine.LineNumber
                            }
                            );
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
                    //MainWindow.bl.AddStationToLine(new BO.LineStation() { Index = (int)indexCb.SelectedItem, LineNumber = myLine.LineNumber, StationNumber = (stationCbb.SelectedItem as BO.StationToAdd).StationId });
                    //lineStations.Add(new BO.LineStation() { Index = lineStations.Count + 1, LineNumber = myLine.LineNumber, StationNumber = (stationCbb.SelectedItem as BO.StationToAdd).StationId });
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            myStation = new BO.Station();
            myStation.Coordinates = new Location() { Longitude = 35.203165854, Latitude = 31.772663576 };
            myMap.Center = myStation.Coordinates;
            newStationGrid.IsEnabled = true;
            stationCbb.IsEnabled = false;
            newStationGrid.DataContext = myStation;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            stationCbb.IsEnabled = true;
            newStationGrid.IsEnabled = false;
            myStation = null;
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