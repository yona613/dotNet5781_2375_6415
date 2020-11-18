using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dotNet5781_03A_2375_6415
{
    /// <summary>
    /// A class that will represent a single bus line
    ///  which is defined as a route of various bus line stations///
    /// </summary>
    class Line : IComparable<Line>, IEnumerable
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="number">the line number</param>
        /// <param name="tmpArea">the line area</param>
        public Line(int number = 0, Area tmpArea = Area.General)
        {
            lineNumber = number;
            busArea = tmpArea;
            stations = new List<BusLineStop>();
        }
        /// <summary>
        /// the line number
        /// </summary>
        private int lineNumber;
        /// <summary>
        /// getter & setter for the line number
        /// </summary>
        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }
        /// <summary>
        /// The first stop the line passes through
        /// </summary>
        private BusLineStop firstStation;
        /// <summary>
        /// getter & setter for the first station
        /// </summary>
        public BusLineStop FirstStation
        {
            get { return firstStation; }
            set { firstStation = value; }
        }
        /// <summary>
        /// The last stop the line passes through
        /// </summary>
        private BusLineStop lastStation;
        /// <summary>
        /// getter & setter for the last station
        /// </summary>
        public BusLineStop LastStation
        {
            get { return lastStation; }
            set { lastStation = value; }
        }
        /// <summary>
        /// The area in which the line travels
        /// </summary>
        private Area busArea;
        /// <summary>
        /// getter & setter for the area
        /// </summary>
        public Area BusArea
        {
            get { return busArea; }
            set { busArea = value; }
        }
        /// <summary>
        /// List of bus stops where the line stops
        /// </summary>
        public List<BusLineStop> stations;
        /// <summary>
        /// overriding ToString func which print out the line details
        /// including the line number, the area where the line operates and the list of station numbers
        /// </summary>
        /// <returns>string with all the details</returns>
        public override string ToString()
        {
            string tmpString = "BusLine:  " + lineNumber.ToString() + ", " + busArea.ToString() + " / ";
            for (int i = 0; i < stations.Count; i++) // add to the string the list of station numbers
            {
                tmpString += stations[i].BusStationKey.ToString();
                tmpString += " ";
            }

            return tmpString;
        }
        /// <summary>
        /// Adds station in line
        /// if Index out of range throw ArgumentOutOfRangeException
        /// if station already exists throw ArgumentException
        /// </summary>
        /// <param name="predStation">Key of last station in the list of stations (if first station enter 0)</param>
        /// <param name="stationNum">the number of the station</param>
        /// <param name="tmpAdress">the address of the station</param>
        public void AddStation(int predStation, int stationNum, string tmpAdress = "")
        {
            int index = 0; //index where to insert
            if (predStation != 0) //if not first stop
            {
                for (; index < stations.Count; index++) //looks for index 
                {
                    if (stations[index].BusStationKey == predStation)
                    {
                        break;
                    }
                }
                index++;
                if (index > stations.Count) // if index out of range
                {
                    throw new ArgumentOutOfRangeException("index");
                }
            }
            if (!CheckStation(stationNum)) // If the station does not exist on the route
            {
                BusLineStop tmpStation = new BusLineStop(stationNum, tmpAdress);
                if (index < stations.Count)
                {
                    if (index == 0) //if adds first stop
                    {
                        firstStation = tmpStation; //update first stop
                        stations.Insert(index, tmpStation); //adds station
                        stations[index + 1].SetDistance(stations[index]); //update distance and time
                        stations[index + 1].SetTravelTime();
                    }
                    else
                    {
                        stations.Insert(index, tmpStation); //adds station
                        stations[index].SetDistance(stations[index - 1]);  //update distance and time
                        stations[index].SetTravelTime();
                        stations[index + 1].SetDistance(stations[index]);
                        stations[index + 1].SetTravelTime();
                    }
                }
                else //if (index == stations.Count) //if adds last stop
                {
                    if (index != 0) //if last stop isn't first stop
                    {
                        stations.Add(tmpStation); //adds station
                        lastStation = tmpStation; //update last stop
                        stations[index].SetDistance(stations[index - 1]);  //update distance and time
                        stations[index].SetTravelTime();
                    }
                    else //if last station is first station
                    {
                        firstStation = tmpStation; //update first stop
                        stations.Add(tmpStation); //adds station
                        lastStation = tmpStation; //update last stop
                    }
                }
            }
            else // ERROR: Bus station already exists, bus can pass by the same station only once
            {
                throw new ArgumentException("Bus station already exists, bus can pass by the same station only once ");
            }
        }

        /// <summary>
        /// Deletes station in line
        /// If station doesn't exist throws NotFoundException
        /// </summary>
        /// <param name="stationNum">Number of station</param>
        public void DeleteStation(int stationNum)
        {
            int i = 0;
            for (; i < stations.Count; i++) //goes over the all line
            {
                if (stations[i].BusStationKey == stationNum) //if found
                {
                    break;
                }
            }
            if ((i + 1) > stations.Count) //if not found
            {
                throw new NotFoundException("ERROR : Bus stop not found !!");
            }
            if ((i + 1) != stations.Count) //if not last station
            {
                if (i == 0) //if it is the first station
                {
                    firstStation = stations[i + 1]; //update first station
                    stations[i + 1].Distance = 0; //update distance and time
                    stations[i + 1].SetTravelTime();
                }
                else
                {
                    stations[i + 1].SetDistance(stations[i - 1]);  //update distance and time
                    stations[i + 1].SetTravelTime();
                }
            }
            else  //if it is last station
            {
                lastStation = stations[i - 1]; //update last station
            }
            stations.RemoveAt(i); //remove the station at found index
        }
        /// <summary>
        /// Checks if station exists in line and returns boolean
        /// </summary>
        /// <param name="stationNum">Number of station</param>
        /// <returns></returns>
        public bool CheckStation(int stationNum)
        {
            for (int i = 0; i < stations.Count; i++) //goes over the all list
            {
                if (stations[i].BusStationKey == stationNum) //if found
                {
                    return true;
                }
            }
            return false; //else
        }


        /// <summary>
        /// Calculates the time of route between 2 stations
        /// </summary>
        /// <param name="stop1">Source's station</param>
        /// <param name="stop2">Destination's station</param>
        /// <returns>Return time in TimeSpan Form</returns>
        public TimeSpan Time(int stop1, int stop2)
        {
            int i = 0;
            for (; i < stations.Count; i++) //searches for first stop
            {
                if (stations[i].BusStationKey == stop1) //found
                {
                    break;
                }
            }
            int j = i;
            for (; j < stations.Count; j++) //searches for second stop
            {
                if (stations[j].BusStationKey == stop2) //found
                {
                    break;
                }
            }
            if ((i == stations.Count) || (j == stations.Count)) //if doesn't find first or second stop
            {
                throw new NotFoundException("Cannot calculate duration, route doesn't exist in line !!");
            }
            TimeSpan time = new TimeSpan(); //creates new TimeSpan
            do
            {
                time += stations[++i].TravelTime; //sums time from first station to least station using time function
            } while (stations[i].BusStationKey != stop2);
            return time;
        }
        /// <summary>
        /// creates a subline composed by the route between 2 stops
        /// </summary>
        /// <param name="stop1">Source's stop</param>
        /// <param name="stop2">Destination's stop</param>
        /// <returns>Return a subLine</returns>
        public Line SubLine(int stop1, int stop2)
        {
            int i = 0;
            bool flag = false; //checks if stations found
            for (; i < stations.Count; i++) //goes over the all line
            {
                if (stations[i].BusStationKey == stop1) //if station found
                {
                    flag = true; //first station was found
                    break;
                }
            }
            if (flag == true) //if first station was found
            {
                for (int j = i; j < (stations.Count); j++) //goes over all remaining line to find second station
                {
                    if (stations[j].BusStationKey == stop2) //if second station
                    {
                        flag = true;//second station found
                        break;
                    }
                    flag = false; //didn't find second station
                }
            }
            Line subLine = new Line(); //creates a new subLine
            if (flag == false) //if 2 stations not found
            {
                subLine = null;
                return subLine; //returns a null subline
            }
            subLine.firstStation = stations[i]; //updates first station of subLine
            do
            {
                subLine.stations.Add(stations[i]); //adds stations to subLine until reaches the second station
            } while (stations[i++].BusStationKey != stop2);
            subLine.LastStation = stations[i - 1]; //updates last station of subLine
            return subLine;
        }
        /// <summary>
        /// Finds station in line and returns it
        /// </summary>
        /// <param name="number">Number of the stataion</param>
        /// <returns>Returns the station found</returns>
        public BusLineStop FindStation(int number)
        {
            BusLineStop tmpStop = new BusLineStop(-1); //creataes a new station with number -1 (not found)
            foreach (BusLineStop item in stations) //goes over all stations in list
            {
                if (item.BusStationKey == number) //if found
                {
                    return item; //return the station found
                }
            }
            return tmpStop; //else return a -1 station (not found)
        }
        /// <summary>
        /// Implementation of function compareTo of Icomparable interface
        /// </summary>
        /// <param name="line2">Line to compare with</param>
        /// <returns></returns>
        public int CompareTo(Line line2)
        {
            //compares the time of the 2 lines
            TimeSpan time1 = Time(firstStation.BusStationKey, LastStation.BusStationKey);
            TimeSpan time2 = line2.Time(line2.firstStation.BusStationKey, line2.lastStation.BusStationKey);
            return time1.CompareTo(time2); //return comparaison of time
        }
        /// <summary>
        /// Implemantation of enumerator interface
        /// </summary>
        /// <returns>Returns enumarator of the list of stations</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return stations.GetEnumerator();
        }

    }
}
    
