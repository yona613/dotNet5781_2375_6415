﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
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
        public static List<BusInTravel> busInTravelList;
        public static List<LineDeparting> lineDepartingList;
        public static List<PairStations> PairStationList = new List<PairStations> { };
        public static List<UserTrip> userTripList;
        /// <summary>
        /// static CTOR
        /// </summary>
        static DataSource()
        {
            busList = CreateBusList();
            CreateStations();
            CreateLines();
            CreateLineStations();
            userList = CreateUsers();
            lineDepartingList = CreateLineDeparting();
        }
        /// <summary>
        /// Create Bus List
        /// </summary>
        /// <returns>list of buses</returns>
        private static List<Bus> CreateBusList()
        {
            List<Bus> busList = new List<Bus> { }; //list of buses
            // initalizing 20 buses
            for (int i = 0; i < 17; i++) // 17 ordinary buses
            {
                DateTime tmpDate = new DateTime(2000 + r.Next(0, 19), r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                DateTime tmpTest = new DateTime(2020, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                if (tmpDate.Year < 2018)
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus { License = r.Next(1000000, 10000000), LicenseDate = tmpDate, Kilometrage = tmpKm, Fuel = r.Next(0, 1201), BusStatus = Status.Ready, TestDate = tmpTest, KmFromTest = r.Next(0, 18000), Brand = "Volvo", AirConditionning = true, MyActivity = Activity.On };
                    busList.Add(tmpBus);
                }
                else
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus { License = r.Next(10000000, 100000000), LicenseDate = tmpDate, Kilometrage = tmpKm, Fuel = r.Next(0, 1201), BusStatus = Status.Ready, TestDate = tmpTest, KmFromTest = r.Next(0, 18000), Brand = "Volvo", AirConditionning = true, MyActivity = Activity.On };
                    busList.Add(tmpBus);
                }
            }
            //Bus that next test time is passed
            DateTime tmpDate1 = new DateTime(2015, r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            DateTime tmpTest1 = new DateTime(2018, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            int tmpKm1 = r.Next(20500, 200000);
            Bus tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(1000000, 10000000), Fuel = r.Next(0, 1201), Kilometrage = tmpKm1, KmFromTest = r.Next(0, 18000), TestDate = tmpTest1, BusStatus = Status.Ready, Brand = "Volvo", AirConditionning = true, MyActivity = Activity.On };
            busList.Add(tmpBus1);

            //bus close to test because of km
            tmpDate1 = new DateTime(2018, r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(10000000, 100000000),Fuel = r.Next(0, 1201), Kilometrage = tmpKm1, KmFromTest= r.Next(19900, 19998), TestDate = tmpTest1, BusStatus = Status.Ready, Brand = "Volvo", AirConditionning = true, MyActivity = Activity.On };
            busList.Add(tmpBus1);

            //bus close to refuel
            tmpDate1 = new DateTime(2019, r.Next(1, 13), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 28), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus { LicenseDate = tmpDate1, License = r.Next(10000000, 100000000), Fuel = r.Next(0, 50), Kilometrage = tmpKm1, KmFromTest = r.Next(19900, 19998), TestDate = tmpTest1, BusStatus = Status.Ready, Brand = "Volvo", AirConditionning = true, MyActivity = Activity.On };
            busList.Add(tmpBus1);

            return busList;
        }
        /// <summary>
        /// initializing from 'stops' file
        /// </summary>
        private static void CreateStations()
        {
            int[] Codes = { 38831,
                            38832,
                            38833,
                            38834,
                            38836,
                            38837,
                            38838,
                            38839,
                            38840,
                            38841,
                            38842,
                            38844,
                            38845,
                            38846,
                            38847,
                            38848,
                            38849,
                            38852,
                            38854,
                            38855,
                            38856,
                            38859,
                            38860,
                            38861,
                            38862,
                            38863,
                            38864,
                            38865,
                            38866,
                            38867,
                            38869,
                            38870,
                            38872,
                            38873,
                            38875,
                            38876,
                            38877,
                            38878,
                            38879,
                            38880,
                            38881,
                            38883,
                            38884,
                            38885,
                            38886,
                            38887,
                            38888,
                            38889,
                            38890,
                            38891 };

            string[] Names =
            {
                "Bar Lev School/ Ben Yehuda",
                "Herzl / Bilu Junction",
                "Hanachshol / Hadayagim",
                "Fried / The Six Days",
                "Tachana Merkazit Lod / Horada",
                "Hannah Avrech / Vulcani",
                "Herzl / Moshe Sharet",
                "Habanim / Eli Cohen",
                "Weizmann / Habanim",
                "Ha'irus / Ha'kalanit",
                "Ha'kalanit / Ha'narkis",
                "Eli Cohen / Lo'hame Hagetaot",
                "Shabazi / Shevet Achim",
                "Shabazi / Weizmann",
                "Haim Bar Lev / Yitzhak Rabin Boulevard",
                "Lev Hasharon Mental Health Center",
                "Lev Hasharon Mental Health Center",
                "Holtzman / Hamada",
                "Zrifin Camp / Club",
                "Herzl / Golani",
                "Rotem / Deganiot",
                "Ha'arava",
                "Mevo Hagefen / Morad Hate'ena",
                "Mevo Hagefen / Haharchava",
                "Haharchava a",
                "Haharchava b",
                "Haharchava / Havatikim",
                "Airports / Ha'aliyah",
                "Kanaf/ Brosh",
                "Hachavura / Dov Hoz",
                "Beit Halevi e",
                "Harishonim / Kvish 5700",
                "Ben Ish Chai / Tse'elon",
                "Okashi / Levi Eshkol",
                "Menuch Venachala/ Yehuda Gorodisky",
                "Gorodsky / Yechiel Paldi",
                "Derech Menachem Begin / Yaakov Hazan",
                "Derech Hapark / Rabbi Neria",
                "Hate'ena / Hagefen",
                "Hagefen / Ha'alon",
                "Derech Haprachim / Yasmin",
                "Yitzhak Rabin / Pinchas Sapir",
                "Menachem Begin / Yitzhak Rabin",
                "Haim Herzog / Dolev",
                "Gvanim School / Erez",
                "Derech Ha'ilanot / Ha'alon",
                "Derech Ha'ilanot / Menachem Begin",
                "Ha'atsmaut / Weizmann",
                "Weizmann / Marvad Haksamim",
                "Tza'ala / Coral"
            };
            string[] Adresses =
            {
                "Ben Yehuda 76",
                "Herzel",
                "Hanachshol 30",
                "Moshe Fried 9",
                "Merkazit",
                "Hanna Avrech 9",
                "Herzel 20",
                "Habanim 4",
                "Weizmann 11",
                "Hairus 13",
                "Habalanit",
                "Eli Cohen 62",
                "Shabazi 51",
                "Shabazi 31",
                "Haim BarLev",
                "Tzur Moshe",
                "Tzur Moshe",
                "Haim Heltzman 2",
                "Tzrifin",
                "Hertzel 4",
                "Harotem 3",
                "Ha'arava",
                "Mevo Hagefen",
                "Mevo Hagefen",
                "Harchava",
                "Harchava",
                "Harchava",
                "Sderot Ha'aliya",
                "Kanaf",
                "Hachavura 24",
                "Bet Halevi",
                "Hamigdal 13",
                "Rechovot",
                "Israel Okashi 4",
                "Menucha Venachala 31",
                "Yehuda Grodiski 35",
                "Menachem Begin 30",
                "Derech Hapark 20",
                "Hate'ena",
                "Hate'ena",
                "Derech Haprachim 46",
                "Derech Yitschak Rabin",
                "Sderot Menachem Begin 4",
                "Haim Hertzog 12",
                "Erez 2",
                "Derech Ha'ilanot 13",
                "Ha'atsmaout 1",
                "Veitzman 19",
                "Tza'ala 25",
                "Tza'ala 15"
            };
            // longitude
            double[] Longt =
            {
                34.917806,
                34.819541,
                34.782828,
                34.790904,
                34.898098,
                34.796071,
                34.824106,
                34.821857,
                34.822237,
                34.818957,
                34.818392,
                34.827023,
                34.828702,
                34.827102,
                34.763896,
                34.912708,
                34.912602,
                34.807944,
                34.836363,
                34.825249,
                34.81249 ,
                34.910842,
                34.948647,
                34.943393,
                34.940529,
                34.939512,
                34.938705,
                34.8976  ,
                34.879725,
                34.818708,
                34.926837,
                34.899465,
                34.775083,
                34.807039,
                34.816752,
                34.823461,
                34.904907,
                34.878765,
                34.859437,
                34.864555,
                34.784347,
                34.778239,
                34.782985,
                34.785069,
                34.786735,
                34.786623,
                34.785098,
                34.782252,
                34.779753,
                34.787199

            };
            // latitude
            double[] Latd =
            {
                32.183921,
                31.870034,
                31.984553,
                31.88855 ,
                31.956392,
                31.892166,
                31.857565,
                31.862305,
                31.865085,
                31.865222,
                31.867597,
                31.86244 ,
                31.863501,
                31.865348,
                31.977409,
                32.300345,
                32.301347,
                31.914255,
                31.963668,
                31.856115,
                31.874963,
                32.300035,
                32.305234,
                32.304022,
                32.302957,
                32.300264,
                32.298171,
                31.990876,
                31.998767,
                31.883019,
                32.349776,
                32.352953,
                31.897286,
                31.883941,
                31.896762,
                31.898463,
                32.076535,
                32.299994,
                31.865457,
                31.866772,
                31.809325,
                31.80037 ,
                31.799224,
                31.800334,
                31.802319,
                31.804595,
                31.805041,
                31.816751,
                31.816579,
                31.801182
            };
            // fueling the list of lines
            stationList = new List<Station>();
            for (int i = 0; i < 50; i++)
            {
                Station NewStation = new Station
                {
                    StationId = Codes[i],
                    Name = Names[i],
                    Address = Adresses[i],
                    Roof = true,
                    DigitalPanel = true,
                    Invalid = true,                   
                    Coordinates = new Location() { Longitude = Longt[i], Latitude = Latd[i] }
                };
                stationList.Add(NewStation);
            }
        }
        /// <summary>
        /// Create Lines
        /// </summary>
        private static void CreateLines()
        {
            int[] LineNumbers =
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10
            };
            lineList = new List<BusLine>();
            for (int i = 0; i < 10; i++)
            {
                BusLine NewLine = new BusLine()
                {
                    LineNumber = LineNumbers[i],
                    LineArea = Area.Center,
                    FirstStation = stationList[3 * i].StationId,
                    LastStation = stationList[3 * i + 9].StationId
                };
                lineList.Add(NewLine);
            }
        }
        /// <summary>
        /// Create Line Stations
        /// </summary>
        private static void CreateLineStations()
        {
            linestationList = new List<LineStation> { };
            
            for (int i = 0; i < lineList.Count; i++) // all lines
            {
                for (int j = 0; j < 10; j++) // all stations in line
                {
                    LineStation NewLineStation = new LineStation()
                    {
                        LineNumber = lineList[i].LineNumber,
                        StationNumber = stationList[i * 3 + j].StationId,
                        Index = j+1
                    };                  
                    linestationList.Add(NewLineStation);
                }
            }
            // add distances between linestations
            for (int i = 1; i < 11; i++) // all lines
            {
                for (int j = 1; j < 10; j++) // linestations
                {
                    int firstStationNumber;
                    IEnumerable<int> tmp = from station in linestationList
                                           where station.LineNumber == i && station.Index== j
                                           select station.StationNumber;
                    firstStationNumber = tmp.First();
                    int lastStationNumber;
                    tmp = from station in linestationList
                          where station.LineNumber == i && station.Index == j + 1 //next linestation
                          select station.StationNumber;
                    lastStationNumber = tmp.First();
                    // searching the locations
                    Location firstStationLocation;
                    IEnumerable<Location> tmpCoo = from station in stationList
                                                        where station.StationId == firstStationNumber
                                                        select station.Coordinates;
                    firstStationLocation = tmpCoo.First();
                    Location lastStationLocation;
                    tmpCoo = from station in stationList
                             where station.StationId == lastStationNumber
                             select station.Coordinates;
                    lastStationLocation = tmpCoo.First();
                    // calculate the distance
                    double distance = firstStationLocation.GetDistanceTo(lastStationLocation);
                    // checks existence of the pair
                    bool flag = true;
                    foreach (var item in PairStationList)
                    {
                        // pairstation already exist
                        if (item.FirstStationNumber == firstStationNumber && item.LastStationNumber == lastStationNumber)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag == true) // new pair-station
                    {
                        PairStationList.Add(new PairStations() { FirstStationNumber = firstStationNumber, Distance = distance, LastStationNumber = lastStationNumber, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                    }                 
                }
            }
        }
        /// <summary>
        /// create list of all users
        /// </summary>
        /// <returns>list of all users</returns>
        private static List<User> CreateUsers()
        {
            List<User> users = new List<User> { };
            List<string> names = new List<string> { "Yona", "Elyassaf", "Nathi", "Aharon", "David", "Dani", "Oshri", "Eliezer", "Avraham", "Itamar" };
            for (int i = 1; i < 11; i++)
            {
                users.Add(new User { UserName = names[i - 1], Password = "1234", Permission = Permit.User, MyActivity = Activity.On });
            }
            users.Add(new User() { UserName = "yona", Password = "yona", Permission = Permit.Admin, MyActivity = Activity.On });
            users.Add(new User() { UserName = "elyassaf", Password = "elyassaf", Permission = Permit.Admin, MyActivity = Activity.On });
            return users;
        }
        /// <summary>
        /// create list of all LineDeparting
        /// </summary>
        /// <returns>list of all LineDeparting</returns>
        private static List<LineDeparting> CreateLineDeparting()
        {
            List<LineDeparting> myLineDepartings = new List<LineDeparting> { };
            for (int i = 1; i < 11; i++)
            {
                LineDeparting tmpLineDeparting = new LineDeparting() { LineNumber = i, StartTime = new TimeSpan(07, 00, 00), MyActivity = Activity.On, StopTime = new TimeSpan(22, 00, 00), Frequency = new TimeSpan(00, 15, 00) };
                myLineDepartings.Add(tmpLineDeparting);
            }
            return myLineDepartings;
        }
    }
}