using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
using System.Device.Location;

namespace DS
{
    public static class FriendFunctions
    {
        static public double GetDistanceTo(this Location from, Location to)
        {
            GeoCoordinate tmpFrom = new GeoCoordinate() { Longitude = from.Longitude, Latitude = from.Latitude };
            GeoCoordinate tmpTo = new GeoCoordinate() { Longitude = to.Longitude, Latitude = to.Latitude };
            return tmpFrom.GetDistanceTo(tmpTo);
        }
    }
}
