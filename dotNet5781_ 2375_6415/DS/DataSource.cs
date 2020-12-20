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

        public static List<Station> stationList;
        public static List<LineStation> linestationList;
        public static List<BusLine> lineList;
        public static List<Bus> busList;
        public static List<User> userList;
        public static List<BusInTravel> busInTravelList = new List<BusInTravel> { };
        public static List<LineDeparting> lineDepartingList = new List<LineDeparting> { };
        public static List<PairStations> PairStationList = new List<PairStations> { };
        public static List<UserTrip> userTripList = new List<UserTrip> { };

        static DataSource()
        {
            busList = CreateBusList();
            stationList = CreateStations();
            lineList = CreateLines();
            linestationList = CreateLineStations();
            userList = CreateUsers();
        }

        private static List<Bus> CreateBusList()
        {
            List<Bus> busList = new List<Bus> { }; //list of buses 
            for (int i = 0; i < 17; i++)
            {
                DateTime tmpDate = new DateTime(2000 + r.Next(0, 19), r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                DateTime tmpTest = new DateTime(2020, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                if (tmpDate.Year < 2018)
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus {License = r.Next(1000000, 10000000),LicenseDate = tmpDate,Kilometrage = tmpKm, Fuel = r.Next(0, 1201), BusStatus = Status.READY, TestDate = tmpTest,KmFromTest = r.Next(0, 18000), Brand = "Volvo", AirConditionning = true };
                    busList.Add(tmpBus);
                }
                else
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus { License = r.Next(10000000, 100000000), LicenseDate = tmpDate, Kilometrage = tmpKm, Fuel = r.Next(0, 1201), BusStatus = Status.READY, TestDate = tmpTest, KmFromTest = r.Next(0, 18000), Brand = "Volvo", AirConditionning = true };
                    busList.Add(tmpBus);
                }
            }
            //Bus that next test time is passed
            DateTime tmpDate1 = new DateTime(2015, r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            DateTime tmpTest1 = new DateTime(2018, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            int tmpKm1 = r.Next(20500, 200000);
            Bus tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(1000000, 10000000), Fuel = r.Next(0, 1201), Kilometrage = tmpKm1, KmFromTest = r.Next(0, 18000), TestDate = tmpTest1, BusStatus = Status.READY, Brand = "Volvo", AirConditionning = true };
            busList.Add(tmpBus1);

            //bus close to test because of km
            tmpDate1 = new DateTime(2018, r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(10000000, 100000000),Fuel = r.Next(0, 1201), Kilometrage = tmpKm1, KmFromTest= r.Next(19900, 19998), TestDate = tmpTest1, BusStatus = Status.READY, Brand = "Volvo", AirConditionning = true };
            busList.Add(tmpBus1);

            //bus close to refuel
            tmpDate1 = new DateTime(2019, r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(10000000, 100000000), Fuel = r.Next(0, 50), Kilometrage = tmpKm1, KmFromTest = r.Next(19900, 19998), TestDate = tmpTest1, BusStatus = Status.READY, Brand = "Volvo", AirConditionning = true };
            busList.Add(tmpBus1);

            return busList;
        }

        private static List<Station> CreateStations()
        {
            List<string> addressList = new List<string> { "Tchernikowsky", "Veitzman", "Shakhal", "Heller", "Bazak", "Mea Shearim", "Geoula", "Begin", "Ouziel", "Romema", "Manitou", "Man", "Zangwill", "Bayit Vegan", "Hantke", "Bayit", "Mekor Baruh", "Rashi", "Pines", "Havaad Haleoumi", "Tora Vaavoda", "Michlin", "Herzl", "Ben Maimon", " Shaoulzon", "Hai Taieb", "Rashba" , "Ramban", "Raavad","Hakablan", "HaMemGuimel","Viznitz" };
            List<Station> myStationsList = new List<Station> { };
            for (int i = 1000; i < 1050; i++)
            {
                int tmpInt = r.Next(0, 31);  //to get adress in array
                Station tmpStation = new Station { StationId = i, Coordinates = new GeoCoordinate { Latitude = r.NextDouble() * 2.3 + 31, Longitude = r.NextDouble() * 1.2 + 34.3 }, Name = addressList[tmpInt], Address = r.Next(0, 100).ToString() + " " + addressList[tmpInt], DigitalPanel = true, Invalid = true, Roof = true };
                myStationsList.Add(tmpStation);
            }
            return myStationsList;
        }

        private static List<BusLine> CreateLines()
        {
            List<BusLine> busLines = new List<BusLine> { };
            for (int i = 1; i < 11; i++)
            {
                BusLine tmpLine = new BusLine { FirstStation = r.Next(1000, 1050), Key = Config.BusLineCounter, LastStation = r.Next(1000, 1050), LineArea = (Area)r.Next(0, 4), LineNumber = i };
                busLines.Add(tmpLine);
            }
            return busLines;
        }

        private static List<LineStation> CreateLineStations()
        {
            List<LineStation> lineStations = new List<LineStation> { };
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    lineStations.Add(new LineStation { Index = j, LineNumber = i, StationNumber = r.Next(1000, 1050) });
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
                users.Add(new User { UserName = names[i - 1], Password = "1234", Permission = Permit.USER });
            }
            return users;
        }
    }
}
