using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2375_6415
{
    

    class BusStop
    {
        protected int busStationKey;

        public int BusStationKey
        {
            get { return busStationKey; }
        }

        //public void SetBusStationKey(Random random)
        //{
        //    busStationKey = random.Next(1000000);
        //}

        public void SetBusStationKey(int number)
        {
            if (number == 0)
            {
                Console.WriteLine("enter bus stop code");
                string tmpCode = Console.ReadLine();
                while (!(int.TryParse(tmpCode, out busStationKey)))
                {
                    Console.WriteLine("enter bus stop code");
                    tmpCode = Console.ReadLine();
                }
            }
            else
            {
                busStationKey = number;
            }
        }

        protected float latitude;

        public float Latitude
        {
            get { return latitude; }
        }

        public void SetLatitude(Random random)
        {
            latitude = ((float)random.NextDouble() * ((float)2.3)) + (float)31;
        }

        protected float longitude;

        public float Longitude
        {
            get { return longitude; }
        }

        public void SetLongitude(Random random)
        {
            longitude = ((float)random.NextDouble()*((float)1.2))+(float)34.3;
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
        private float distance;
        
        public float Distance
        {
            get { return distance; }
            //set { distance = value; }
        }

        void setDistance(BusLineStop tmpBus)
        {
            distance = (float)Math.Sqrt(Math.Pow((double)(latitude - tmpBus.Latitude), (double)2) + Math.Pow((double)(longitude - tmpBus.Longitude), (double)2));
        }

        private TimeSpan travelTime;

        public TimeSpan TravelTime
        {
            get { return travelTime; }
            set { travelTime = value; }
        }
    
    }
 

    enum Area { General , North , South , Center , Jerusalem};

    class Line : IComparable , IEnumerable
    {
        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
        }

        public void SetLineNumber(int number)
        {
            if (number == 0)
            {
                Console.WriteLine("enter line Number");
                string tmpCode = Console.ReadLine();
                while (!(int.TryParse(tmpCode, out lineNumber)))
                {
                    Console.WriteLine("enter line Number");
                    tmpCode = Console.ReadLine();
                }
            }
            else
            {
                lineNumber = number;
            }
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

        List<BusLineStop> stations = new List<BusLineStop> { };

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

        public void AddStation(int index, BusLineStop newStop)
        {
            if (!CheckStation(newStop.BusStationKey))
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

        public float Distance(int stop1, int stop2)
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
            float distance = 0;
            do
            {
                distance += stations[++i].Distance;
            } while (stations[i].BusStationKey!=stop2);
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
            if (stations[i].BusStationKey != stop1 || !CheckStation(stop2))
            {
            }
            TimeSpan time = new TimeSpan();
            do
            {
                time += stations[++i].TravelTime;
            } while (stations[i].BusStationKey != stop2);
            return time;
        }

        public Line SubLine (int stop1, int stop2)
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
            for (; i < stations.Count; i++)
            {
                if (stations[i].BusStationKey == stop2)
                {
                    flag = true;
                    break;
                }
            }
            Line subLine = new Line();
            if (flag == false)
            {
                subLine = null;
            }
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
            TimeSpan time1 = Time(firstStation.BusStationKey, LastStation.BusStationKey);
            TimeSpan time2 = Time(((Line)line2).firstStation.BusStationKey, ((Line)line2).lastStation.BusStationKey);
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
        
        public bool AddLine(Line tmpLine)
        {
            foreach (var Line in myList)
            {
                if (Line.LineNumber == tmpLine.LineNumber)
                {
                    if ((Line.FirstStation == tmpLine.FirstStation) && (Line.LastStation == tmpLine.LastStation))
                    {
                        return false;
                    }
                    if (!((Line.LastStation == tmpLine.FirstStation) && (Line.FirstStation == tmpLine.LastStation)))
                    {
                        return false;
                    }
                }
               
            }
            myList.Add(tmpLine);
            return true;
        }

        public bool DeleteLine(int tmpLine)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i].LineNumber == tmpLine)
                {
                    myList.RemoveAt(i);
                    return true;
                }
            }
            return false;
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
                Exception stationNotFound = new Exception() ;
                throw stationNotFound;
            }
            return subList;
        }

        public List<Line> SortList()
        {
            List<Line> sortedList = new List<Line> { };
            foreach (Line line in myList)
            {
                sortedList.Add(line);
            }
            sortedList.Sort();
            return sortedList;
        }

        public Line this[int tmpLine]
        {
            get
            {
                for (int i = 0; i < myList.Count; i++)
                {
                    return myList[i];
                }
                Exception lineNotFound = new Exception();
                throw lineNotFound;
            }

        }
       

        IEnumerator IEnumerable.GetEnumerator()
        {
            return myList.GetEnumerator();
        }
    }



    class Program
    {
        enum Choice { ADD = 1, DELETE, FIND , PRINT , EXIT }
        static void Main(string[] args)
        {
            BusLinesList myList = new BusLinesList();
            BusLineStop[] stopsArray = new BusLineStop[40];
            for (int i = 0; i < 40; i++)
            {
                BusLineStop tmpStop = new BusLineStop();
                bool flag = true;
                do
                {
                    tmpStop.SetBusStationKey(i+1);
                    for (int j = 0; j < i; j++)
                    {
                        if (tmpStop.BusStationKey == stopsArray[j].BusStationKey)
                        {
                            flag = false;
                            break;
                        }
                    }
                } while (flag == false);
                tmpStop.SetLatitude(r);
                tmpStop.SetLongitude(r);
                stopsArray[i] = tmpStop;
            }

            Line[] linesArray = new Line[10];
            for (int i = 0; i < 8; i++)
            {
                Line tmpLine1 = new Line();
                tmpLine1.SetLineNumber(i+1);
                for (int j = 0; j < 4; j++)
                {
                    tmpLine1.AddStation(j+1, stopsArray[r.Next(40)]);
                }
                linesArray[i] = tmpLine1;
            }
            for (int i = 0; i < 2; i++)
            {
                Line tmpLine1 = new Line();
                tmpLine1.SetLineNumber(i + 9);
                for (int j = 20*i; j < 20*(i+1); j++)
                {
                    tmpLine1.AddStation(j-(20*i)+1,stopsArray[j]);
                }
                linesArray[i + 8] = tmpLine1;
            }
            int myChoice;
            int innerChoice;
            do
            {
                Console.WriteLine(" Enter your choice :");
                Console.WriteLine(" Add - 1 / Delete - 2 / Find - 3 / Print - 4 / Exit - 5 ");
                Console.WriteLine();
                string tmpChoice;
                do
                {
                    tmpChoice = Console.ReadLine();
                } while (!int.TryParse(tmpChoice , out myChoice));
                switch ((Choice)myChoice)
                {
                    case Choice.ADD:
                        Console.WriteLine("Enter your choice :" );
                        Console.WriteLine("New line - 1 / New BusStop - 2");
                        do
                        {
                            tmpChoice = Console.ReadLine();
                        } while (!int.TryParse(tmpChoice, out innerChoice));
                        if (innerChoice == 1)
                        {
                            myList.AddLine(linesArray[r.Next(10)]);
                        }
                        if (innerChoice == 2)
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum;
                            string tmpNumber;
                            do
                            {
                                tmpNumber = Console.ReadLine();
                            } while (!int.TryParse(tmpNumber, out tmpNum));
                            foreach (Line line in myList)
                            {
                                if (line.LineNumber == tmpNum)
                                {
                                    Console.WriteLine("Enter index of bus stop in line :");
                                    do
                                    {
                                        tmpNumber = Console.ReadLine();
                                    } while (!int.TryParse(tmpNumber, out tmpNum));
                                }
                                line.AddStation(tmpNum, stopsArray[r.Next(40)]);
                                break;
                            }
                        }
                        break;
                    case Choice.DELETE:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Delete line - 1 / Delete BusStop - 2");
                        do
                        {
                            tmpChoice = Console.ReadLine();
                        } while (!int.TryParse(tmpChoice, out innerChoice));
                        if (innerChoice == 1)
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum;
                            string tmpNumber;
                            do
                            {
                                tmpNumber = Console.ReadLine();
                            } while (!int.TryParse(tmpNumber, out tmpNum));
                            myList.DeleteLine(tmpNum);
                        }
                        if (innerChoice == 2)
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum;
                            string tmpNumber;
                            do
                            {
                                tmpNumber = Console.ReadLine();
                            } while (!int.TryParse(tmpNumber, out tmpNum));
                            foreach (Line line in myList)
                            {
                                if (line.LineNumber == tmpNum)
                                {
                                    Console.WriteLine("Enter index of bus stop in line :");
                                    do
                                    {
                                        tmpNumber = Console.ReadLine();
                                    } while (!int.TryParse(tmpNumber, out tmpNum));
                                }
                                line.DeleteStation(tmpNum);
                                break;
                            }
                        }
                        break;
                    case Choice.FIND:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Find Lines for station - 1 / Find bus Line - 2");
                        do
                        {
                            tmpChoice = Console.ReadLine();
                        } while (!int.TryParse(tmpChoice, out innerChoice));
                        if (innerChoice == 1)
                        {
                            Console.WriteLine("Enter Bus station :");
                            int tmpNum;
                            string tmpNumber;
                            do
                            {
                                tmpNumber = Console.ReadLine();
                            } while (!int.TryParse(tmpNumber, out tmpNum));
                            List<Line> subList = myList.FindStation(tmpNum);
                            foreach (Line item in subList)
                            {
                                Console.WriteLine($"Line #{item.LineNumber}");
                            }
                        }
                        if (innerChoice == 2)
                        {
                            Console.WriteLine("Enter first station :");
                            int stn1;
                            string tmpNumber;
                            do
                            {
                                tmpNumber = Console.ReadLine();
                            } while (!int.TryParse(tmpNumber, out stn1));
                            Console.WriteLine("Enter second station :");
                            int stn2;
                            do
                            {
                                tmpNumber = Console.ReadLine();
                            } while (!int.TryParse(tmpNumber, out stn2));
                            BusLinesList subList = new BusLinesList();
                            foreach (Line line in myList)
                            {
                                Line tmpLine = line.SubLine(stn1, stn2);
                                if (tmpLine != null)
                                {
                                    subList.AddLine(tmpLine);
                                }
                            }
                            subList.SortList();
                            foreach (Line item in subList)
                            {
                                Console.WriteLine($"Line #{item.LineNumber}");
                            }
                        }
                        break;
                    case Choice.PRINT:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Print all lines - 1 / Print all stops - 2");
                        Console.WriteLine();
                        do
                        {
                            tmpChoice = Console.ReadLine();
                        } while (!int.TryParse(tmpChoice, out innerChoice));
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
                                    foreach ( int num in stationList)
                                    {
                                        if (num == stop.BusStationKey)
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (flag  == true)
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


        static Random r = new Random(DateTime.Now.Millisecond);
    }

    
}
