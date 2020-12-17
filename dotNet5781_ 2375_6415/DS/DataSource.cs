using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using DO;

namespace DS
{

    public class DataSource
    {
        /// <summary>
        /// Random number to be used in the whole namespace
        /// </summary>
        public static Random r = new Random(DateTime.Now.Millisecond);

        List<Station> stationList;
        List<BusLine> lineList;
        List<Bus> busList;
        List<User> userList;

        public DataSource()
        {
            busList = CreateBusList();
            stationList = CreateStations();
        }

        private List<Bus> CreateBusList()
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

        private List<Station> CreateStations()
        {
            List<string> addressList = new List<string> { "Tchernikowsky", "Veitzman", "Shakhal", "Heller", "Bazak", "Mea Shearim", "Geoula", "Begin", "Ouziel", "Romema", "Manitou", "Man", "Zangwill", "Bayit Vegan", "Hantke", "Bayit", "Mekor Baruh", "Rashi", "Pines", "Havaad Haleoumi", "Tora Vaavoda", "Michlin", "Herzl", "Ben Maimon", " Shaoulzon", "Hai Taieb", "Rashba" , "Ramban", "Raavad","Hakablan", "HaMemGuimel","Viznitz" };
            List<Station> myStationsList = new List<Station> { };
            for (int i = 1000; i < 1050; i++)
            {
                int tmpInt = r.Next(0, 31);  //to get adress in array
                Station tmpStation = new Station { StationId = i, Coordinates = new GeoCoordinate { Latitude = r.NextDouble() * 2.3 + 31, Longitude = r.NextDouble() * 1.2 + 34.3 }, Name = addressList[tmpInt], Address = r.Next(0, 100).ToString() + " " + addressList[tmpInt], DigitalPanel = true, Invalid = true, Roof = true };
            }
            return myStationsList;
        }

        private List<BusLine> CreateLines()
        {
            List<BusLine> busLines = new List<BusLine> { };
            for (int i = 0; i < 10; i++)
            {

            }
            return busLines;
        }
    }
}
