using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2375_6415
{
    /// <summary>
    /// A class that represents a bus stop
    /// </summary>
    class BusStop
    {
        /// <summary>
        /// empty CTOR
        /// </summary>
        /// <param name="number">number of the station</param>
        /// <param name="tmpAddress">address of the station</param>
        public BusStop(int number = 0, string tmpAddress = "")
        {
            busStationKey = number;
            address = tmpAddress;
            longitude = ((float)myRandom.r.NextDouble() * ((float)1.2)) + (float)34.3; // Lottery of longitude within the coordinates of the State of Israel
            latitude = ((float)myRandom.r.NextDouble() * ((float)2.3)) + (float)31; // Lottery of latitude within the coordinates of the State of Israel
        }
        /// <summary>
        /// number of the station
        /// </summary>
        protected int busStationKey;
        /// <summary>
        /// getter & setter for the number of the station
        /// </summary>
        public int BusStationKey
        {
            get { return busStationKey; }
            set { busStationKey = value; }
        }
        /// <summary>
        /// the station's latitude
        /// </summary>
        protected float latitude;
        /// <summary>
        /// getter for the station's latitude
        /// </summary>
        public float Latitude
        {
            get { return latitude; }
        }
        /// <summary>
        /// the station's longitude
        /// </summary>
        protected float longitude;
        /// <summary>
        /// getter for the station's Longitude
        /// </summary>
        public float Longitude
        {
            get { return longitude; }
        }
        /// <summary>
        /// the station's address
        /// </summary>
        protected string address;
        /// <summary>
        /// getter & setter for the station's address
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /// <summary>
        /// overriding ToString func which print out the station details
        /// </summary>
        /// <returns>string with all the details</returns>
        public override string ToString()
        {
            string tmpString = "Bus Station Code:  " + busStationKey.ToString() + ", " + latitude.ToString() + "°N " + longitude.ToString() + "°E";
            return tmpString;
        }
    }
}
