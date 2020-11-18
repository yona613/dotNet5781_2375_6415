using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781__03A_2375_6415
{
    /// <summary>
    /// A class that will represent a bus line station.
    /// Contains all bus station data,
    /// and also contains the distance from the previous bus line station,
    /// and the travel time from the previous bus line station
    /// </summary>
    class BusLineStop : BusStop
    {
        /// <summary>
        /// empty CTOR
        /// </summary>
        /// <param name="number">number of the station</param>
        /// <param name="tmpAddress">address of the station</param>
        public BusLineStop(int number = 0, string tmpAddress = "") : base(number, tmpAddress) { }

        /// <summary>
        /// Distance from previous station
        /// </summary>
        private double distance = 0;

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        /// <summary>
        /// Calculates distance using coordinates (found on internet)
        /// </summary>
        /// <param name="tmpBus">The previous station</param>
        public void SetDistance(BusLineStop tmpBus)
        {
            double theta = tmpBus.longitude - longitude;
            distance = (Math.Sin((tmpBus.latitude * Math.PI) / 180.0) * Math.Sin((latitude * Math.PI) / 180.0) + Math.Cos((tmpBus.longitude * Math.PI) / 180.0) * Math.Cos((longitude * Math.PI) / 180.0) * Math.Cos((theta * Math.PI) / 180.0));
            distance = Math.Acos(distance);
            distance = (distance * 60 * 1.1515 * 1.609344);
        }

        /// <summary>
        /// Travel time from previous station
        /// </summary>
        private TimeSpan travelTime = new TimeSpan(0, 0, 0); //default 0 if first station

        public TimeSpan TravelTime
        {
            get { return travelTime; }
            set { travelTime = value; }
        }

        /// <summary>
        /// Calculates the travel time from the previous station.
        /// assuming that the bus runs on average at a speed of 40 km / h
        /// </summary>
        /// <param name="tmpBus">The previous station</param>
        /// <returns>The duration of the journey from the previous station as a show of TimeSpan</returns>
        public void SetTravelTime()
        {
            // Calculation of hours, minutes and seconds according to a rate of 40 km / h and depending on the distance
            travelTime = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0)));
        }

        public override string ToString()
        {
            string tmpString = base.ToString();
            tmpString += "   " + travelTime.ToString();
            return tmpString;
        }
    }
}
