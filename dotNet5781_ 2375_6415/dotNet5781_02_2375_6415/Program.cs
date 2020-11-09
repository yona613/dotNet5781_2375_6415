using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2375_6415
{
    using static Math;
    /// <summary>
    /// A new exception class for cases where we didn't find the product that the user entered
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// A class that represents a bus stop
    /// </summary>
    class BusStop
    {
        /// <summary>
        /// empty CTOR
        /// </summary>
        /// <param name="number"></param>
        /// <param name="tmpAddress"></param>
        public BusStop(int number = 0, string tmpAddress = "")
        {
            busStationKey = number;
            address = tmpAddress;
            Random r = new Random(DateTime.Now.Millisecond);
            longitude = ((float)r.NextDouble() * ((float)1.2)) + (float)34.3;
            latitude = ((float)r.NextDouble() * ((float)2.3)) + (float)31;
        }

        protected int busStationKey;

        public int BusStationKey
        {
            get { return busStationKey; }
            set { busStationKey = value; }
        }

        protected float latitude;

        public float Latitude
        {
            get { return latitude; }
        }

        protected float longitude;

        public float Longitude
        {
            get { return longitude; }
        }

        protected string address;

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

        public BusLineStop(int number = 0, string tmpAddress = "") : base(number, tmpAddress) { }

        /// <summary>
        /// Calculates distance using coordinates (found on internet)
        /// </summary>
        /// <param name="tmpBus"></param>
        public double Distance(BusLineStop tmpBus)
        {
            double distance = 0;
            double theta = tmpBus.longitude - longitude;
            distance = (Math.Sin((tmpBus.latitude * Math.PI) / 180.0) * Math.Sin((latitude * Math.PI) / 180.0) + Math.Cos((tmpBus.longitude * Math.PI) / 180.0) * Math.Cos((longitude * Math.PI) / 180.0) * Math.Cos((theta * Math.PI) / 180.0));
            distance = Math.Acos(distance);
            distance = (distance * 60 * 1.1515 * 1.609344);
            return distance;
        }

        public TimeSpan TravelTime(BusLineStop tmpBus)
        {
            double distance = Distance(tmpBus);
            TimeSpan tmpTime = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0)));
            return tmpTime;
        }

    }

    enum Area { General, North, South, Center, Jerusalem };

    class Line : IComparable<Line>, IEnumerable
    {
        public Line(int number = 0, Area tmpArea = Area.General)
        {
            lineNumber = number;
            busArea = tmpArea;
            stations = new List<BusLineStop>();
        }

        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
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

        List<BusLineStop> stations;

        public override string ToString()
        {
            string tmpString = "BusLine:  " + lineNumber.ToString() + ", " + busArea.ToString() + " / ";
            for (int i = 0; i < stations.Count; i++)
            {
                tmpString += stations[i].BusStationKey.ToString();
                tmpString += " ";
            }

            return tmpString;
        }

        public void AddStation(int index, int stationNum, string tmpAdress = "")
        {
            if (index > stations.Count + 1)
            {
                ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("index");
                throw ex;
            }
            if (!CheckStation(stationNum))
            {
                BusLineStop tmpStation = new BusLineStop(stationNum, tmpAdress);
                if (index <= stations.Count)
                {
                    if (index == 1)
                    {
                        firstStation = tmpStation;
                    }
                    stations.Insert(index - 1, tmpStation);
                }
                else if (index == stations.Count + 1)
                {
                    if (index != 1)
                    {
                        stations.Add(tmpStation);
                        lastStation = tmpStation;
                    }
                    else
                    {
                        firstStation = tmpStation;
                        stations.Add(tmpStation);
                        lastStation = tmpStation;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Bus station already exists, bus can pass by the same station only once ");
            }
        }

        public void AddStation(int index, BusLineStop newStop)
        {
            try
            {
                if (index > stations.Count + 1)
                {
                    ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("index");
                    throw ex;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($" ERROR : {ex.ToString()}");
            }
            if (!CheckStation(newStop.BusStationKey))
            {
                if (index <= stations.Count)
                {
                    if (index == 1)
                    {
                        firstStation = newStop;
                    }
                    stations.Insert(index - 1, newStop);
                }
                else if (index == stations.Count + 1)
                {
                    if (index != 1)
                    {
                        stations.Add(newStop);
                        lastStation = newStop;
                    }
                    else
                    {
                        firstStation = newStop;
                        stations.Add(newStop);
                        lastStation = newStop;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Bus station already exists, bus can pass by the same station only once ");
            }
        }

        public void DeleteStation(int stationNum)
        {
            int i = 0;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stationNum)
                {
                    break;
                }
            }
            if (i >= stations.Count)
            {
                throw new NotFoundException("ERROR : Bus stop not found !!");
            }
            stations.RemoveAt(i);
            if (i == 0)
            {
                firstStation = stations[0];
            }
            if ((i - 1) == stations.Count)
            {
                lastStation = stations[i - 1];
            }
        }

        public bool CheckStation(int stationNum)
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

        public double Distance(int stop1, int stop2)
        {
            int i = 0;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stop1)
                {
                    break;
                }
            }
            if (stations[i].BusStationKey != stop1 || !CheckStation(stop2))
            {
            }
            double distance = 0;
            do
            {
                i++;
                distance += stations[i].Distance(stations[i - 1]);
            } while (stations[i].BusStationKey != stop2);
            return distance;
        }

        public TimeSpan Time(int stop1, int stop2)
        {
            int i = 0;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stop1)
                {
                    break;
                }
            }
            if ((i == stations.Count) || !CheckStation(stop2))
            {
                throw new NotFoundException("Cannot calculate duration, route doesn't exist in line !!");
            }
            TimeSpan time = new TimeSpan();
            do
            {
                time += stations[++i].TravelTime(stations[i - 1]);
            } while (stations[i].BusStationKey != stop2);
            return time;
        }

        public Line SubLine(int stop1, int stop2)
        {
            int i = 0;
            bool flag = false;
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stop1)
                {
                    flag = true;
                    break;
                }
            }
            if (flag == true)
            {
                for (int j = i; j < (stations.Count); j++)
                {
                    if (stations[j].BusStationKey == stop2)
                    {
                        flag = true;
                        break;
                    }
                    flag = false;
                }
            }
            Line subLine = new Line();
            if (flag == false)
            {
                subLine = null;
                return subLine;
            }
            subLine.firstStation = stations[i];
            do
            {
                subLine.stations.Add(stations[i]);
            } while (stations[i++].BusStationKey != stop2);
            subLine.LastStation = stations[i - 1];
            return subLine;
        }

        public BusLineStop FindStation(int number)
        {
            BusLineStop tmpStop = new BusLineStop(-1);
            foreach (BusLineStop item in stations)
            {
                if (item.BusStationKey == number)
                {
                    return item;
                }
            }
            return tmpStop;
        }


        public int CompareTo(Line line2)
        {
            TimeSpan time1 = Time(firstStation.BusStationKey, LastStation.BusStationKey);
            TimeSpan time2 = line2.Time(line2.firstStation.BusStationKey, line2.lastStation.BusStationKey);
            return time1.CompareTo(time2);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return stations.GetEnumerator();
        }

    }

    class BusLinesList : IEnumerable
    {
        private List<Line> myList = new List<Line> { };

        public bool AddLine(int lineNumber, Area tmpArea = Area.General)
        {
            int i = 0;
            foreach (var Line in myList)
            {
                if (Line.LineNumber == lineNumber)
                {
                    i++;
                }

            }
            if (i < 2)
            {
                Line tmpLine = new Line(lineNumber, tmpArea);
                myList.Add(tmpLine);
            }
            else
            {
                throw new ArgumentException("Line already exists, cannot build more than 2 routes for the same line !!");
            }
            return true;
        }

        public void AddStation(int lineNumber, int index, int stationNumber, string stationAddress = "")
        {
            BusLineStop tmpStop = FindStop(stationNumber);
            if (tmpStop.BusStationKey != -1)
            {
                bool flag = false;
                foreach (Line item in myList)
                {
                    if (item.LineNumber == lineNumber)
                    {
                        flag = true;
                        item.AddStation(index, tmpStop);
                        break;
                    }
                }
                if (!flag)
                {
                    throw new NotFoundException("Bus line not found !!");
                }
            }
            else
            {
                bool flag = false;
                foreach (Line item in myList)
                {
                    if (item.LineNumber == lineNumber)
                    {
                        flag = true;
                        item.AddStation(index, stationNumber);
                        break;
                    }
                }
                if (!flag)
                {
                    throw new NotFoundException("Bus line not found !!");
                }
            }
        }

        public void DeleteLine(int tmpLine)
        {
            int i = 0;
            for (; i < myList.Count; i++)
            {
                if (myList[i].LineNumber == tmpLine)
                {
                    myList.RemoveAt(i);
                    break;
                }
            }
            if (i == myList.Count)
            {
                throw new NotFoundException("Cannot delete line that doesn't exists !!");
            }
        }

        public BusLineStop FindStop(int number)
        {
            BusLineStop tmpStop = new BusLineStop();
            foreach (Line item in myList)
            {
                tmpStop = item.FindStation(number);
                if (tmpStop.BusStationKey != -1)
                {
                    return tmpStop;
                }
            }
            return tmpStop;
        }

        public List<Line> FindStation(int tmpStation)
        {
            List<Line> subList = new List<Line> { };
            foreach (Line line in myList)
            {
                foreach (BusLineStop busStation in line)
                {
                    if (tmpStation == busStation.BusStationKey)
                    {
                        subList.Add(line);
                        break;
                    }
                }
            }
            if (subList.Count == 0)
            {
                throw new NotFoundException("cannot create list of lines, no line deserves that station !!");
            }
            return subList;
        }

        public BusLinesList CreateSubList(int stn1, int stn2)
        {
            BusLinesList subList = new BusLinesList();
            foreach (Line line in myList)
            {
                Line tmpLine = line.SubLine(stn1, stn2);
                if (tmpLine != null)
                {
                    tmpLine.LineNumber = line.LineNumber;
                    subList.myList.Add(tmpLine);
                }
            }
            return subList;
        }

        public void SortList()
        {
            myList.Sort();
        }

        public Line this[int tmpLine]
        {
            get
            {
                for (int i = 0; i < myList.Count; i++)
                {
                    if (myList[i].LineNumber == tmpLine)
                    {
                        return myList[i];
                    }
                }
                throw new NotFoundException("Bus line not Found !!");
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return myList.GetEnumerator();
        }
    }



    class Program
    {
        enum Choice { ADD = 1, DELETE, FIND, PRINT, EXIT }

        static void Main(string[] args)
        {
            BusLinesList myList = new BusLinesList();
            for (int i = 0; i < 10; i++)
            {
                myList.AddLine(i + 1);
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    try
                    {
                        myList.AddStation(i + 1, 1, r.Next(1, 40));
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                    }
                    catch (NotFoundException ex1)
                    {
                    }
                    catch (ArgumentException ex2)
                    {
                    }
                }
            }
            int myChoice;
            int innerChoice;
            do
            {
                Console.WriteLine("Enter your choice :");
                Console.WriteLine("Add - 1 / Delete - 2 / Find - 3 / Print - 4 / Exit - 5 ");
                Console.WriteLine();
                myChoice = getIntInput();
                switch ((Choice)myChoice)
                {
                    case Choice.ADD:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("New line - 1 / New BusStop - 2");
                        innerChoice = getIntInput();
                        if (innerChoice == 1)
                        {
                            Console.WriteLine("Enter Number of Line :");
                            innerChoice = getIntInput();
                            try
                            {
                                myList.AddLine(innerChoice);
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                        }
                        if (innerChoice == 2)
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum;
                            tmpNum = getIntInput();
                            Console.WriteLine("Enter Index of Stop in Line :");
                            int index = getIntInput();
                            Console.WriteLine("Enter number of station :");
                            int stnNum = getIntInput();
                            try
                            {
                                myList.AddStation(tmpNum, index, stnNum);
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                            catch (NotFoundException ex1)
                            {
                                Console.WriteLine($" ERROR : {ex1.ToString()}");
                            }
                            catch (ArgumentException ex2)
                            {
                                Console.WriteLine($" ERROR : {ex2.ToString()}");
                            }
                        }
                        break;
                    case Choice.DELETE:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Delete line - 1 / Delete BusStop - 2");
                        innerChoice = getIntInput();
                        if (innerChoice == 1)
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum = getIntInput();
                            try
                            {
                                myList.DeleteLine(tmpNum);
                            }
                            catch (NotFoundException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }

                        }
                        if (innerChoice == 2)
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum = getIntInput();
                            bool flag = false;
                            foreach (Line line in myList)
                            {
                                if (line.LineNumber == tmpNum)
                                {
                                    flag = true;
                                    Console.WriteLine("Enter number of bus stop in line :");
                                    tmpNum = getIntInput();
                                    try
                                    {
                                        line.DeleteStation(tmpNum);
                                    }
                                    catch (NotFoundException ex)
                                    {
                                        Console.WriteLine($" ERROR : {ex.ToString()}");
                                    }
                                    break;
                                }
                            }
                            try
                            {
                                if (!flag)
                                    throw new NotFoundException("Bus line not found !!");
                            }
                            catch (NotFoundException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                        }
                        break;
                    case Choice.FIND:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Find Lines for station - 1 / Find bus Line - 2");
                        innerChoice = getIntInput();
                        if (innerChoice == 1)
                        {
                            Console.WriteLine("Enter Bus station :");
                            int tmpNum = getIntInput();
                            try
                            {
                                List<Line> subList = myList.FindStation(tmpNum);
                                foreach (Line item in subList)
                                {
                                    Console.WriteLine($"Line #{item.LineNumber}");
                                }
                            }
                            catch (NotFoundException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }

                        }
                        if (innerChoice == 2)
                        {
                            Console.WriteLine("Enter first station :");
                            int stn1 = getIntInput();
                            Console.WriteLine("Enter second station :");
                            int stn2 = getIntInput();
                            BusLinesList subList = myList.CreateSubList(stn1, stn2);
                            try
                            {
                                subList.SortList();
                                foreach (Line item in subList)
                                {
                                    Console.WriteLine($"Line #{item.LineNumber}");
                                }
                            }
                            catch (NotFoundException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                        }
                        break;
                    case Choice.PRINT:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Print all lines - 1 / Print all stops - 2");
                        Console.WriteLine();
                        innerChoice = getIntInput();
                        if (innerChoice == 1)
                        {
                            foreach (Line item in myList)
                            {
                                Console.WriteLine($"Line #{item.LineNumber}");
                            }
                        }
                        if (innerChoice == 2)
                        {
                            List<int> stationList = new List<int> { };
                            foreach (Line line in myList)
                            {
                                foreach (BusLineStop stop in line)
                                {
                                    bool flag = true;
                                    foreach (int num in stationList)
                                    {
                                        if (num == stop.BusStationKey)
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (flag == true)
                                    {
                                        List<Line> subList = myList.FindStation(stop.BusStationKey);
                                        Console.WriteLine(stop.ToString());
                                        foreach (Line item in subList)
                                        {
                                            Console.Write($"Line #{item.LineNumber} / ");
                                        }
                                        Console.WriteLine();
                                        stationList.Add(stop.BusStationKey);
                                    }
                                }
                            }
                        }
                        break;
                    case Choice.EXIT:
                        Console.WriteLine("GoodBye");
                        break;
                    default:
                        break;
                }
            } while (myChoice != 5);
        }

        static int getIntInput()
        {
            string tmpString;
            int tmpNum;
            do
            {
                tmpString = Console.ReadLine();
                try
                {
                    if (!int.TryParse(tmpString, out tmpNum))
                    {
                        throw new InvalidCastException("Invalid input !!");
                    }
                    break;
                }
                catch (InvalidCastException ex)
                {
                    Console.WriteLine($" ERROR : {ex.ToString()}");
                }
            } while (true);
            return tmpNum;
        }

        static Random r = new Random(DateTime.Now.Millisecond);
    }


}


