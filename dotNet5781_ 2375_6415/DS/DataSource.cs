using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using DO;

namespace DS
{
    public static class DataSource
    {
        /// <summary>
        /// Random number to be used in the whole namespace
        /// </summary>
        public static Random r = new Random(DateTime.Now.Millisecond);

        private static int stationNum = 1010;
        public static List<Station> stationList;
        public static List<LineStation> linestationList;
        public static List<BusLine> lineList;
        public static List<Bus> busList;
        public static List<User> userList;
        public static List<BusInTravel> busInTravelList;
        public static List<LineDeparting> lineDepartingList;
        public static List<PairStations> PairStationList = new List<PairStations> { };
        public static List<UserTrip> userTripList;

        static DataSource()
        {
            busList = CreateBusList();
            stationList = CreateStations();
            lineList = CreateLines();
            linestationList = CreateLineStations();
            userList = CreateUsers();
            lineDepartingList = CreateLineDeparting();
        }

        private static List<Bus> CreateBusList()
        {
            List<Bus> busList = new List<Bus> { }; //list of buses 
            for (int i = 0; i < 17; i++)
            {
                DateTime tmpDate = new DateTime(2000 + r.Next(0, 19), r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                DateTime tmpTest = new DateTime(2020, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                if (tmpDate.Year < 2018)
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus { License = r.Next(1000000, 10000000), LicenseDate = tmpDate, Kilometrage = tmpKm, Fuel = r.Next(0, 1201), BusStatus = Status.READY, TestDate = tmpTest, KmFromTest = r.Next(0, 18000), Brand = "Volvo", AirConditionning = true, MyActivity = Activity.ON };
                    busList.Add(tmpBus);
                }
                else
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus { License = r.Next(10000000, 100000000), LicenseDate = tmpDate, Kilometrage = tmpKm, Fuel = r.Next(0, 1201), BusStatus = Status.READY, TestDate = tmpTest, KmFromTest = r.Next(0, 18000), Brand = "Volvo", AirConditionning = true, MyActivity = Activity.ON };
                    busList.Add(tmpBus);
                }
            }
            //Bus that next test time is passed
            DateTime tmpDate1 = new DateTime(2015, r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            DateTime tmpTest1 = new DateTime(2018, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            int tmpKm1 = r.Next(20500, 200000);
            Bus tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(1000000, 10000000), Fuel = r.Next(0, 1201), Kilometrage = tmpKm1, KmFromTest = r.Next(0, 18000), TestDate = tmpTest1, BusStatus = Status.READY, Brand = "Volvo", AirConditionning = true, MyActivity = Activity.ON };
            busList.Add(tmpBus1);

            //bus close to test because of km
            tmpDate1 = new DateTime(2018, r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(10000000, 100000000),Fuel = r.Next(0, 1201), Kilometrage = tmpKm1, KmFromTest= r.Next(19900, 19998), TestDate = tmpTest1, BusStatus = Status.READY, Brand = "Volvo", AirConditionning = true, MyActivity = Activity.ON };
            busList.Add(tmpBus1);

            //bus close to refuel
            tmpDate1 = new DateTime(2019, r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(10000000, 100000000), Fuel = r.Next(0, 50), Kilometrage = tmpKm1, KmFromTest = r.Next(19900, 19998), TestDate = tmpTest1, BusStatus = Status.READY, Brand = "Volvo", AirConditionning = true, MyActivity = Activity.ON };
            busList.Add(tmpBus1);

            return busList;
        }

        private static List<Station> CreateStations()
        {
            List<string> addressList = new List<string> { "Tchernikowsky", "Veitzman", "Shakhal", "Heller", "Bazak", "Mea Shearim", "Geoula", "Begin", "Ouziel", "Romema", "Manitou", "Man", "Zangwill", "Bayit Vegan", "Hantke", "Bayit", "Mekor Baruh", "Rashi", "Pines", "Havaad Haleoumi", "Tora Vaavoda", "Michlin", "Herzl", "Ben Maimon", " Shaoulzon", "Hai Taieb", "Rashba" , "Ramban", "Raavad","Hakablan", "HaMemGuimel","Viznitz" };
            List<Station> myStationsList = new List<Station> { };
            for (int i = 1000; i < 1200; i++)
            {
                int tmpInt = r.Next(0, 31);  //to get adress in array
                Station tmpStation = new Station { StationId = i, Coordinates = new GeoCoordinate { Latitude = r.NextDouble() * 2.3 + 31, Longitude = r.NextDouble() * 1.2 + 34.3 }, Name = addressList[tmpInt], Address = addressList[tmpInt] + " " + r.Next(0, 100).ToString(), DigitalPanel = true, Invalid = true, Roof = true, MyActivity = Activity.ON };
                myStationsList.Add(tmpStation);
            }
            return myStationsList;
        }

        private static List<BusLine> CreateLines()
        {
            int i = 1;
            List<BusLine> busLines = new List<BusLine> { }; 
            busLines.Add(new BusLine { FirstStation = 1000, Key = Config.BusLineCounter, LastStation = 1009, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1001, Key = Config.BusLineCounter, LastStation = 1008, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1002, Key = Config.BusLineCounter, LastStation = 1007, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1003, Key = Config.BusLineCounter, LastStation = 1006, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1004, Key = Config.BusLineCounter, LastStation = 1005, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1001, Key = Config.BusLineCounter, LastStation = 1008, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1002, Key = Config.BusLineCounter, LastStation = 1006, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1004, Key = Config.BusLineCounter, LastStation = 1006, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1009, Key = Config.BusLineCounter, LastStation = 1006, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            busLines.Add(new BusLine { FirstStation = 1008, Key = Config.BusLineCounter, LastStation = 1007, LineArea = (Area)r.Next(0, 4), LineNumber = i++, MyActivity = Activity.ON });
            return busLines;
        }

        private static List<LineStation> CreateLineStations()
        {
            List<LineStation> lineStations = new List<LineStation> { };
            for (int i = 1; i < 11; i++)
            {
                for (int j = 2; j < 10; j++)
                {
                    lineStations.Add(new LineStation { Index = j, LineNumber = i, StationNumber = stationNum++, MyActivity = Activity.ON });
                }
            }
            lineStations.Add(new LineStation { Index = 1, LineNumber = 1, StationNumber = 1000, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 2, StationNumber = 1001, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 3, StationNumber = 1002, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 4, StationNumber = 1003, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 5, StationNumber = 1004, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 6, StationNumber = 1001, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 7, StationNumber = 1002, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 8, StationNumber = 1004, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 9, StationNumber = 1009, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 1, LineNumber = 10, StationNumber = 1008, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 1, StationNumber = 1009, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 2, StationNumber = 1008, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 3, StationNumber = 1007, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 4, StationNumber = 1006, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 5, StationNumber = 1005, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 6, StationNumber = 1008, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 7, StationNumber = 1006, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 8, StationNumber = 1006, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 9, StationNumber = 1006, MyActivity = Activity.ON });
            lineStations.Add(new LineStation { Index = 10, LineNumber = 10, StationNumber = 1007, MyActivity = Activity.ON });
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    int firstStationNumber;
                    IEnumerable<int> tmp = from station in lineStations
                                           where station.LineNumber == i && station.Index== j
                                           select station.StationNumber;
                    firstStationNumber = tmp.First();
                    int lastStationNumber;
                    tmp = from station in lineStations
                          where station.LineNumber == i && station.Index == j + 1
                          select station.StationNumber;
                    lastStationNumber = tmp.First();
                    GeoCoordinate firstStationLocation;
                    IEnumerable<GeoCoordinate> tmpCoo = from station in stationList
                                                        where station.StationId == firstStationNumber
                                                        select station.Coordinates;
                    firstStationLocation = tmpCoo.First();
                    GeoCoordinate lastStationLocation;
                    tmpCoo = from station in stationList
                             where station.StationId == lastStationNumber
                             select station.Coordinates;
                    lastStationLocation = tmpCoo.First();
                    double distance = firstStationLocation.GetDistanceTo(lastStationLocation);
                    PairStationList.Add(new PairStations() { FirstStationNumber = firstStationNumber, Distance = distance, LastStationNumber = lastStationNumber, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                }
            }
            return lineStations;
        }

        private static List<User> CreateUsers()
        {
            List<User> users = new List<User> { };
            List<string> names = new List<string> { "Yona", "Elyassaf", "Nathi", "Aharon", "David", "Dani", "Oshri", "Eliezer", "Avraham", "Itamar" };
            for (int i = 1; i < 11; i++)
            {
                users.Add(new User { UserName = names[i - 1], Password = "1234", Permission = Permit.USER, MyActivity = Activity.ON });
            }
            return users;
        }

        private static List<LineDeparting> CreateLineDeparting()
        {
            List<LineDeparting> myLineDepartings = new List<LineDeparting> { };
            for (int i = 1; i < 11; i++)
            {
                LineDeparting tmpLineDeparting = new LineDeparting() { LineNumber = i, StartTime = new TimeSpan(07, 00, 00), MyActivity = Activity.ON, StopTime = new TimeSpan(22, 00, 00), Frequency = new TimeSpan(00, 15, 00) };
                myLineDepartings.Add(tmpLineDeparting);
            }
            return myLineDepartings;
        }
    }
}
