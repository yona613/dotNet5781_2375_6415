using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for UpdateStation.xaml
    /// </summary>
    public partial class UpdateStation : Window
    {
        BO.Station myStation;
        public UpdateStation(BO.Station tmpStation)
        {
            InitializeComponent();
            myStation = tmpStation;        
            mainGrid.DataContext = tmpStation;
        }

        /// <summary>
        /// Event when update Button clicked
        /// Updates station by sending update to bl
        /// </summary>
        /// <param name="sender">Update button</param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.UpdateStation(myStation);
                Close();
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
