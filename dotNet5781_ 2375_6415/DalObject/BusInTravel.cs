using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    class BusInTravel
    {
        private int key;

        public int Key
        {
            get { return key; }
            set { key = value; }
        }

        private int license;

        public int License
        {
            get { return license; }
            set { license = value; }
        }

        private int line;

        public int Line
        {
            get { return line; }
            set { line = value; }
        }

        private DateTime departureTime;

        public DateTime DepartureTime
        {
            get { return departureTime; }
            set { departureTime = value; }
        }

        private DateTime realDeparture;

        public DateTime RealDeparture
        {
            get { return realDeparture; }
            set { realDeparture = value; }
        }

        private int lastPassedStation;

        public int LastPassedStation
        {
            get { return lastPassedStation; }
            set { lastPassedStation = value; }
        }

        private DateTime timeLastPassedStation;

        public DateTime TimeLastPassedStation
        {
            get { return timeLastPassedStation; }
            set { timeLastPassedStation = value; }
        }

        private DateTime timeAtNextStation;

        public DateTime TimeAtNextStation
        {
            get { return timeAtNextStation; }
            set { timeAtNextStation = value; }
        }

        private int driversId;

        public int DriversId
        {
            get { return driversId; }
            set { driversId = value; }
        }
    }
}
