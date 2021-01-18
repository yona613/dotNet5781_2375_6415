using Microsoft.Maps.MapControl.WPF;
using System.Linq;
using System.Windows;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for LineMap.xaml
    /// </summary>
    public partial class LineMap : Window
    {
        public LineMap(BO.LineToShow tmpLine)
        {
            InitializeComponent();
            mainGrid.DataContext = tmpLine;
            myMap.Center = tmpLine.LineStations.ElementAt(tmpLine.LineStations.Count() / 2).Coordinates;

            //creates Line's route on the map
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polyline.Locations = new LocationCollection() { };
            foreach (var item in tmpLine.LineStations)
            {
                // The pushpin to add to the map.
                Pushpin pin = new Pushpin();
                pin.Location = item.Coordinates;

                polyline.Locations.Add(item.Coordinates);

                // Adds the pushpin to the map.
                myMap.Children.Add(pin);
            }
            polyline.StrokeThickness = 1;
            polyline.Opacity = 0.7;
            myMap.Children.Add(polyline);
        }
    }
}
