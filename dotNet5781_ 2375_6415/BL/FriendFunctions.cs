using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
using System.Device.Location;

namespace BL
{
    /// <summary>
    /// For extensions functions
    /// </summary>
    public static class FriendFunctions
    {
        /// <summary>
        /// Extension Function to calculate distance between 
        /// two locations using Coordinates function
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        static public double GetDistanceTo(this Location from, Location to)
        {
            GeoCoordinate tmpFrom = new GeoCoordinate() { Longitude = from.Longitude, Latitude = from.Latitude };
            GeoCoordinate tmpTo = new GeoCoordinate() { Longitude = to.Longitude, Latitude = to.Latitude };
            return tmpFrom.GetDistanceTo(tmpTo)/1000;
        }
    }
}
