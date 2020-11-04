using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2375_6415
{
    

    class BusStop
    {
        private int busStationKey;

        public int BusStationKey
        {
            get { return busStationKey; }
        }

        void SetBusStationKey()
        {
            Console.WriteLine("enter bus stop code");
            string tmpCode = Console.ReadLine();

            while (!(int.TryParse(tmpCode, out busStationKey)))
            {
                Console.WriteLine("enter bus stop code");
                tmpCode = Console.ReadLine();
            }
        }

        private float latitude;

        public float Latitude
        {
            get { return latitude; }
        }

        public void SetLatitude(Random random)
        {
            latitude = ((float)random.NextDouble() * ((float)2.3)) + (float)31;
        }

        private float longitude;

        public float Longitude
        {
            get { return longitude; }
        }

        public void SetLongitude(Random random)
        {
            longitude = ((float)random.NextDouble()*((float)1.2))+(float)34.3;
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public override string ToString()
        {
            string tmpString = "Bus Station Code:  " + busStationKey.ToString() + ", " + latitude.ToString() + "°N " + longitude.ToString() + "°E";
            return tmpString;
        }
    }

    class BusLineStop : BusStop
    {
        private int distance;
        
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        private TimeSpan travelTime;

        public TimeSpan TravelTime
        {
            get { return travelTime; }
            set { travelTime = value; }
        }
    
    }
 

    enum Area { General , North , South , Center , Jerusalem};

    class Line : IComparable
    {
        private int busLine;

        public int BusLine
        {
            get { return busLine; }
            set { busLine = value; }
        }

        private BusLineStop firstStation;

        public BusLineStop FirstStation
        {
            get { return firstStation; }
            set { firstStation = value; }
        }

        private BusLineStop lastStation;

        public BusLineStop LastStation
        {
            get { return lastStation; }
            set { lastStation = value; }
        }

        private Area busArea;

        public Area BusArea
        {
            get { return busArea; }
            set { busArea = value; }
        }

        List<BusLineStop> stations = new List<BusLineStop>();

        public override string ToString()
        {
            string tmpString = "BusLine:  " + busLine.ToString() + ", " + busArea.ToString() + " / ";
            for (int i = 0; i < stations.Count; i++)
            {
                tmpString += stations[i].BusStationKey.ToString();
                tmpString += " ";
            }

            return tmpString;
        }

        public void addStation(int index, BusLineStop newStop)
        {
            if (!checkStation(newStop.BusStationKey))
            {
                if (index <= stations.Count)
                {
                    stations.Insert(index - 1, newStop);
                    if (index == 1)
                    {
                        firstStation = newStop;
                    }
                }
                else if (index == stations.Count + 1)
                {
                    stations.Add(newStop);
                    lastStation = newStop;
                }
            }
        }

        public void deleteStation(int stationNum)
        {
            int i = 0;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stationNum)
                {
                    break;
                }
            }
            if (stations[i].BusStationKey!=stationNum)
            {
            }
            stations.RemoveAt(i);
            if (i==0)
            {
                firstStation = stations[0];
            }
            if ((i-1) == stations.Count)
            {
                lastStation = stations[i - 1];
            } 
        }

        public bool checkStation(int stationNum)
        { 
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stationNum)
                {
                    return true;
                }
            }
            return false;
        }

        public int Distance(int stop1, int stop2)
        {
            int i = 0;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stop1)
                {
                    break;
                }
            }
            if (stations[i].BusStationKey != stop1 || !checkStation(stop2))
            {
            }
            int distance = 0;
            do
            {
                distance += stations[++i].Distance;
            } while (stations[i].BusStationKey!=stop2);
            return distance;
        }

        public TimeSpan time(int stop1, int stop2)
        {
            int i = 0;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stop1)
                {
                    break;
                }
            }
            if (stations[i].BusStationKey != stop1 || !checkStation(stop2))
            {
            }
            TimeSpan time = new TimeSpan();
            do
            {
                time += stations[++i].TravelTime;
            } while (stations[i].BusStationKey != stop2);
            return time;
        }

        public Line subLine (int stop1, int stop2)
        {
            int i = 0;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stop1)
                {
                    break;
                }
            }
            if (stations[i].BusStationKey != stop1 || !checkStation(stop2))
            {
            }
            Line subLine = new Line();
            subLine.firstStation = stations[i];
            do
            {
                subLine.stations.Add(stations[i]);
            } while (stations[i++].BusStationKey != stop2);
            subLine.LastStation = stations[i - 1];
            return subLine;
        }

        public int CompareTo(object line2)
        {
            TimeSpan time1 = time(firstStation.BusStationKey, LastStation.BusStationKey);
            TimeSpan time2 = time(((Line)line2).firstStation.BusStationKey, ((Line)line2).lastStation.BusStationKey);
            return time1.CompareTo(time2);
        }

    }



    class Program
    {
        static void Main(string[] args)
        {
        }

        static Random r = new Random(DateTime.Now.Millisecond);
    }

    
}
