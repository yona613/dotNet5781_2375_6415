using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using System.Xml;

namespace DL
{
    sealed class DalXml : IDL
    {
        #region singelton
        /// <summary>
        /// new static read-only dalxml
        /// </summary>
        private static readonly DalXml instance = new DalXml();
        static DalXml() { }// static ctor to ensure instance init is done just before first usage
        private DalXml() { } // default => private
        public static DalXml Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus
        /// <summary>
        /// get all bus that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by buses satisfies the cindition</returns>
        public IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate)
        {
            IEnumerable<Bus> allBuses = from bus in XMLTools.LoadListFromXMLSerializer<Bus>(@"Bus.xml")
                                        where bus.MyActivity == Activity.On
                                        where predicate(bus)
                                        select bus.Clone();
            if (allBuses != null)
            {
                return allBuses;
            }
            throw new ReadDataException("No Bus meets the conditions");
        }
        /// <summary>
        /// get all bus
        /// </summary>
        /// <returns>IEnumerable implemented by buses</returns>
        public IEnumerable<Bus> GetAllBuses()
        {
            return from Bus in XMLTools.LoadListFromXMLSerializer<Bus>(@"Bus.xml")
                   where Bus.MyActivity == Activity.On
                   select Bus.Clone();
        }
        /// <summary>
        /// get solid bus by condition
        /// throw BadBusException
        /// </summary>
        /// <param name="license"></param>
        /// <returns>the appropriate bus</returns>
        public Bus GetBus(int license)
        {
            Bus tmpBus = XMLTools.LoadListFromXMLSerializer<Bus>(@"Bus.xml").Find(bus => bus.License == license && bus.MyActivity == Activity.On);
            if (tmpBus != null)
            {
                return tmpBus.Clone();
            }
            throw new BadBusException("Bus doesn't exist", license);
        }
        /// <summary>
        /// adding bus to bus-list
        /// throw BadBusException
        /// </summary>
        /// <param name="myBus">bus to add</param>
        public void AddBus(Bus myBus)
        {
            var busList = XMLTools.LoadListFromXMLSerializer<Bus>(@"Bus.xml");
            if (busList.FirstOrDefault(bus => bus.License == myBus.License && bus.MyActivity == Activity.On) != null)
                throw new BadBusException("Bus already exist", myBus.License);
            busList.Add(myBus.Clone());
            XMLTools.SaveListToXMLSerializer<Bus>(busList, @"Bus.xml");
        }
        /// <summary>
        /// update bus by deleting it and adding it with the news updates
        /// throw BadBusException
        /// </summary>
        /// <param name="busToUpdate">bus To Update</param>
        public void UpdateBus(Bus busToUpdate)
        {
            Bus tmpBus = XMLTools.LoadListFromXMLSerializer<Bus>(@"Bus.xml").FirstOrDefault(bus => bus.License == busToUpdate.License && bus.MyActivity == Activity.On);
            if (tmpBus == null)
                throw new BadBusException("Bus doesn't exist", busToUpdate.License);
            DeleteBus(tmpBus.License);
            AddBus(busToUpdate);
        }
        /// <summary>
        /// delete bus by turns its activity off
        /// throw BadBusException
        /// </summary>
        /// <param name="license"></param>
        public void DeleteBus(int license)
        {
            var busList = XMLTools.LoadListFromXMLSerializer<Bus>(@"Bus.xml");
            Bus tmpBus = busList.FirstOrDefault(bus => bus.License == license && bus.MyActivity == Activity.On);
            if (tmpBus == null)
                throw new BadBusException("Bus doesn't exist", license);
            tmpBus.MyActivity = Activity.Off;
            XMLTools.SaveListToXMLSerializer<Bus>(busList, @"Bus.xml");
        }
        #endregion

        #region Line
        /// <summary>
        /// Get All Lines
        /// </summary>
        /// <returns>IEnumerable implemented by lines</returns>
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in XMLTools.LoadListFromXMLSerializer<BusLine>(@"BusLine.xml")
                   where busLine.MyActivity == Activity.On
                   select busLine.Clone();
        }
        /// <summary>
        /// get all lines that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by lines satisfies the cindition</returns>
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            IEnumerable<BusLine> myLineList = from busLine in XMLTools.LoadListFromXMLSerializer<BusLine>(@"BusLine.xml")
                                              where predicate(busLine)
                                              where busLine.MyActivity == Activity.On
                                              select busLine.Clone();
            if (myLineList != null)
                return myLineList;
            throw new ReadDataException("No Line meets the conditions");
        }
        /// <summary>
        /// get solid line by condition
        /// throw BadLineException
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the appropriate line</returns>
        public BusLine GetBusLine(int id)
        {
            BusLine myBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(@"BusLine.xml").Find(line => line.LineNumber == id && line.MyActivity == Activity.On);
            if (myBusLine != null)
                return myBusLine.Clone();
            throw new BadLineException("the Line doesn't exist", id);
        }
        /// <summary>
        /// adding line to line-list
        /// throw BadLineException
        /// </summary>
        /// <param name="tmpBusLine">line to add</param>
        public void AddLine(BusLine tmpBusLine)
        {
            var lineList = XMLTools.LoadListFromXMLSerializer<BusLine>(@"BusLine.xml");
            var keys = XMLTools.LoadListFromXMLElement(@"ConfigKeys.xml");
            int key = XmlConvert.ToInt32(keys.Element("Lines").Element("Key").Value);
            if (lineList.FirstOrDefault(Line => Line.LineNumber == tmpBusLine.LineNumber && Line.MyActivity == Activity.On) != null)
                throw new BadLineException("the Line already exist", tmpBusLine.LineNumber);
            BusLine myBusLine = tmpBusLine.Clone();
            myBusLine.Key = key;
            lineList.Add(myBusLine);
            key++;
            keys.Element("Lines").Element("Key").SetValue(key);
            XMLTools.SaveListToXMLElement(keys, @"ConfigKeys.xml");
            XMLTools.SaveListToXMLSerializer<BusLine>(lineList, @"BusLine.xml");
        }
        /// <summary>
        /// update line by deleting it and adding it with the news updates
        /// throw BadLineException
        /// </summary>
        /// <param name="lineToUpdate">line To Update</param>
        public void UpdateLine(BusLine lineToUpdate)
        {
            var lineList = XMLTools.LoadListFromXMLSerializer<BusLine>(@"BusLine.xml");
            BusLine tmpLine = lineList.FirstOrDefault(line => line.LineNumber == lineToUpdate.LineNumber && line.MyActivity == Activity.On);
            if (tmpLine == null)
                throw new BadLineException("the Line doesn't exist", lineToUpdate.LineNumber);
            DeleteLine(tmpLine.LineNumber);
            AddLine(lineToUpdate);
        }
        /// <summary>
        /// delete line by turns its activity off
        /// throw BadLineException
        /// </summary>
        /// <param name="lineNumber"></param>
        public void DeleteLine(int lineNumber)
        {
            var lineList = XMLTools.LoadListFromXMLSerializer<BusLine>(@"BusLine.xml");
            BusLine tmpLine = lineList.FirstOrDefault(line => line.LineNumber == lineNumber && line.MyActivity == Activity.On);
            if (tmpLine == null)
                throw new BadLineException("the Line doesn't exist", lineNumber);
            tmpLine.MyActivity = Activity.Off;
            XMLTools.SaveListToXMLSerializer<BusLine>(lineList, @"BusLine.xml");
        }
        #endregion

        #region Station
        /// <summary>
        /// get all stations
        /// </summary>
        /// <returns>IEnumerable implemented by stations</returns>
        public IEnumerable<Station> GetAllStations()
        {
            return from station in XMLTools.LoadListFromXMLSerializer<Station>(@"Station.xml")
                   where station.MyActivity == Activity.On
                   select station.Clone();
        }
        /// <summary>
        /// get all stations that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by buses satisfies the cindition</returns>
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            IEnumerable<Station> myStationsList = from station in XMLTools.LoadListFromXMLSerializer<Station>(@"Station.xml")
                                                  where station.MyActivity == Activity.On
                                                  where predicate(station)
                                                  select station.Clone();
            if (myStationsList == null)
                throw new ReadDataException("No Station meets the conditions");
            return myStationsList;
        }
        /// <summary>
        /// get solid station by condition
        /// throw BadStationException
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the appropriate station</returns>
        public Station GetStation(int id)
        {
            Station myStation = XMLTools.LoadListFromXMLSerializer<Station>(@"Station.xml").Find(Station => Station.StationId == id && Station.MyActivity == Activity.On);
            if (myStation != null)
                return myStation.Clone();
            throw new BadStationException("Station doesn't exist", id);
        }
        /// <summary>
        /// adding station to station-list
        /// throw BadStationException
        /// </summary>
        /// <param name="tmpStation">station to add</param>
        public void AddStation(Station tmpStation)
        {
            var stationsList = XMLTools.LoadListFromXMLSerializer<Station>(@"Station.xml");
            if (stationsList.FirstOrDefault(station => station.StationId == tmpStation.StationId && station.MyActivity == Activity.On) != null)
                throw new BadStationException("Station already exist", tmpStation.StationId);
            stationsList.Add(tmpStation.Clone());
            XMLTools.SaveListToXMLSerializer<Station>(stationsList, @"Station.xml");
        }
        /// <summary>
        /// delete station by turns its activity to off
        /// throw BadStationException
        /// </summary>
        /// <param name="id">number of the station going to been deleted</param>
        public void DeleteStation(int id)
        {
            var stationsList = XMLTools.LoadListFromXMLSerializer<Station>(@"Station.xml");
            Station myStation = stationsList.FirstOrDefault(station => station.StationId == id && station.MyActivity == Activity.On);
            if (myStation == null)
                throw new BadStationException("Station doesn't exist", id);
            myStation.MyActivity = Activity.Off;
            XMLTools.SaveListToXMLSerializer<Station>(stationsList, @"Station.xml");
        }
        #endregion

        #region User
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns>IEnumerable implemented by users</returns>
        public IEnumerable<User> GetAllUsers()
        {
            var userXel = XMLTools.LoadListFromXMLElement(@"User.xml");
            return from user in userXel.Elements()
                   where user.Element("MyActivity").Value == "On"
                   select new User()
                   {
                       UserName = user.Element("UserName").Value,
                       Password = user.Element("Password").Value,
                       MyActivity = (Activity)Enum.Parse(typeof(Activity), user.Element("MyActivity").Value, true),
                       Permission = (Permit)Enum.Parse(typeof(Permit), user.Element("Permission").Value, true)
                   };
        }
        /// <summary>
        /// get all users that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by users satisfies the cindition</returns>
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            var userXel = XMLTools.LoadListFromXMLElement(@"User.xml");
            IEnumerable<User> myUsers = from user in userXel.Elements()
                                        where user.Element("MyActivity").Value == "On"
                                        let tmpUser = new User()
                                        {
                                            UserName = user.Element("UserName").Value,
                                            Password = user.Element("Password").Value,
                                            MyActivity = (Activity)Enum.Parse(typeof(Activity), user.Element("MyActivity").Value, true),
                                            Permission = (Permit)Enum.Parse(typeof(Permit), user.Element("Permission").Value, true)
                                        }
                                        where predicate(tmpUser)
                                        select tmpUser;
            if (myUsers == null)
                throw new ReadDataException("No User meets the conditions");
            return myUsers;
        }
        /// <summary>
        /// get solid user by his name
        /// throw BadUserException
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            var userXel = XMLTools.LoadListFromXMLElement(@"User.xml");
            User myUser = (from user in userXel.Elements()
                           where user.Element("MyActivity").Value == "On"
                           where user.Element("UserName").Value == userName
                           select new User()
                           {
                               UserName = user.Element("UserName").Value,
                               Password = user.Element("Password").Value,
                               MyActivity = (Activity)Enum.Parse(typeof(Activity), user.Element("MyActivity").Value, true),
                               Permission = (Permit)Enum.Parse(typeof(Permit), user.Element("Permission").Value, true)
                           }).FirstOrDefault();

            if (myUser != null)
                return myUser;
            throw new BadUserException("User doesn't exist", userName);
        }
        /// <summary>
        /// update user (delete the old and add the new)
        /// throw BadUserException
        /// </summary>
        /// <param name="userToUpdate">user To Update</param>
        public void UpdateUser(User userToUpdate)
        {
            var userXel = XMLTools.LoadListFromXMLElement(@"User.xml");
            User tmpUser = (from user in userXel.Elements()
                            where user.Element("MyActivity").Value == "On"
                            where user.Element("UserName").Value == userToUpdate.UserName
                            select new User()
                            {
                                UserName = user.Element("UserName").Value,
                                Password = user.Element("Password").Value,
                                MyActivity = (Activity)Enum.Parse(typeof(Activity), user.Element("MyActivity").Value, true),
                                Permission = (Permit)Enum.Parse(typeof(Permit), user.Element("Permission").Value, true)
                            }).FirstOrDefault();
            if (tmpUser == null)
                throw new BadUserException("User doesn't exist", userToUpdate.UserName);
            DeleteUser(tmpUser.UserName);
            AddUser(userToUpdate);
        }
        /// <summary>
        /// add user
        /// throw BadUserException
        /// </summary>
        /// <param name="tmpUser">user to add</param>
        public void AddUser(User tmpUser)
        {
            var userXel = XMLTools.LoadListFromXMLElement(@"User.xml");
            User userTmp = (from user in userXel.Elements()
                            where user.Element("MyActivity").Value == "On"
                            where user.Element("UserName").Value == tmpUser.UserName
                            select new User()
                            {
                                UserName = user.Element("UserName").Value,
                                Password = user.Element("Password").Value,
                                MyActivity = (Activity)Enum.Parse(typeof(Activity), user.Element("MyActivity").Value, true),
                                Permission = (Permit)Enum.Parse(typeof(Permit), user.Element("Permission").Value, true)
                            }).FirstOrDefault();
            if (userTmp != null)
                throw new BadUserException("User already exist", tmpUser.UserName);
            XElement userToAdd = new XElement("User",
                new XElement("UserName", tmpUser.UserName),
                new XElement("Password", tmpUser.Password),
                new XElement("Permission", tmpUser.Permission),
                new XElement("MyActivity", tmpUser.MyActivity)
                );
            userXel.Add(userToAdd);
            XMLTools.SaveListToXMLElement(userXel, @"User.xml");
        }
        /// <summary>
        /// delete user
        /// throw BadUserException
        /// </summary>
        /// <param name="userName">name of user to delete</param>
        public void DeleteUser(string userName)
        {
            var userXel = XMLTools.LoadListFromXMLElement(@"User.xml");
            XElement myUser = (from user in userXel.Elements()
                               where user.Element("MyActivity").Value == "On"
                               where user.Element("UserName").Value == userName
                               select user).FirstOrDefault();
            if (myUser != null)
            {
                myUser.Element("MyActivity").Value = Activity.Off.ToString();
                XMLTools.SaveListToXMLElement(userXel, @"User.xml");
            }
            else throw new BadUserException("User doesn't exist", userName);
        }
        #endregion

        #region LineStation
        /// <summary>
        /// Get All Line Stations
        /// </summary>
        /// <returns>IEnumerable implements by lineStation</returns>
        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from lineStation in XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml")
                   where lineStation.MyActivity == Activity.On
                   select lineStation.Clone();
        }
        /// <summary>
        /// Get All Line Stations meets the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the bool condition</param>
        /// <returns></returns>
        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            IEnumerable<LineStation> myLineStations = from lineStation in XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml")
                                                      where predicate(lineStation) && lineStation.MyActivity == Activity.On
                                                      select lineStation.Clone();
            if (myLineStations != null)
                return myLineStations;
            throw new ReadDataException("No LineStation meets the conditions");
        }
        /// <summary>
        /// Get solid Line Station
        /// throw BadLineStationException
        /// </summary>
        /// <param name="stationNumber"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public LineStation GetLineStation(int stationNumber, int lineNumber)
        {
            LineStation myLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml").FirstOrDefault(
                station => station.LineNumber == lineNumber && station.StationNumber == stationNumber && station.MyActivity == Activity.On);
            if (myLineStation != null)
                return myLineStation.Clone();
            throw new BadLineStationException("Line Station doesn't exist", lineNumber, stationNumber);
        }
        /// <summary>
        /// Add Line Station
        /// throw BadLineStationException
        /// </summary>
        /// <param name="tmpLineStation">lineStation to add</param>
        public void AddLineStation(LineStation tmpLineStation)
        {
            var lineStationList = XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml");
            LineStation tmp = lineStationList.FirstOrDefault(station => station.LineNumber == tmpLineStation.LineNumber && station.StationNumber == tmpLineStation.StationNumber && station.MyActivity == Activity.On);
            if (tmp == null)
            {
                lineStationList.Add(tmpLineStation.Clone());
                XMLTools.SaveListToXMLSerializer<LineStation>(lineStationList, @"LineStation.xml");
            }
            else throw new BadLineStationException("Line Station already exist", tmpLineStation.LineNumber, tmpLineStation.StationNumber);
        }
        /// <summary>
        /// Delete Line Station
        /// throw BadLineStationException
        /// </summary>
        /// <param name="stationNumber"></param>
        /// <param name="lineNumber"></param>
        public void DeleteLineStation(int stationNumber, int lineNumber)
        {
            var lineStationList = XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml");
            LineStation tmpLineStation = lineStationList.FirstOrDefault(station => station.LineNumber == lineNumber && station.StationNumber == stationNumber && station.MyActivity == Activity.On);
            if (tmpLineStation != null)
            {
                tmpLineStation.MyActivity = Activity.Off;
                XMLTools.SaveListToXMLSerializer<LineStation>(lineStationList, @"LineStation.xml");
            }
            else throw new BadLineStationException("Line Station doesn't exist", lineNumber, stationNumber);
        }
        /// <summary>
        /// Update Line Station (delete old and add new)
        /// throw BadLineStationException
        /// </summary>
        /// <param name="lineStationToUpdate"></param>
        public void UpdateLineStation(LineStation lineStationToUpdate)
        {
            LineStation tmpLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml").FirstOrDefault(station => station.LineNumber == lineStationToUpdate.LineNumber && station.StationNumber == lineStationToUpdate.StationNumber && station.MyActivity == Activity.On);
            if (tmpLineStation == null)
                throw new BadLineStationException("Line Station doesn't exist", lineStationToUpdate.LineNumber, lineStationToUpdate.StationNumber);
            DeleteLineStation(tmpLineStation.StationNumber, tmpLineStation.LineNumber);
            AddLineStation(lineStationToUpdate);
        }
        #endregion

        #region BusInTravel
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<BusInTravel> GetAllBusInTravel()
        {
            return from busInTravel in XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml")
                   where busInTravel.MyActivity == Activity.On
                   select busInTravel.Clone();
        }
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate)
        {
            IEnumerable<BusInTravel> myBusInTravel = from busInTravel in XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml")
                                                     where busInTravel.MyActivity == Activity.On && predicate(busInTravel)
                                                     select busInTravel.Clone();
            if (myBusInTravel != null)
                return myBusInTravel;
            throw new ReadDataException("No BusInTravel meets the conditions");
        }
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public BusInTravel GetBusInTravel(int license, int line, DateTime departureTime)
        {
            BusInTravel myBusInTravel = XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml").FirstOrDefault(
                busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.DepartureTime == departureTime && busInTravel.MyActivity == Activity.On);
            if (myBusInTravel != null)
                return myBusInTravel.Clone();
            throw new BadBusInTravelException("Bus In Travel doesn't exist", license, line, departureTime);
        }
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void AddBusInTravel(BusInTravel tmpBusInTravel)
        {
            var busInTravelList = XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml");
            if (busInTravelList.FirstOrDefault(
                busInTravel => busInTravel.License == tmpBusInTravel.License && busInTravel.Line == tmpBusInTravel.Line && busInTravel.DepartureTime == tmpBusInTravel.DepartureTime) != null)
            {
                BusInTravel myBusInTravel = tmpBusInTravel.Clone();
                myBusInTravel.Key = Config.BusInTravelCounter;
                busInTravelList.Add(myBusInTravel);
                XMLTools.SaveListToXMLSerializer<BusInTravel>(busInTravelList, @"BusInTravel.xml");
            }
            throw new BadBusInTravelException("Bus In Travel already exist", tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
        }
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void DeleteBusInTravel(int license, int line, DateTime departureTime)
        {
            var busInTravelList = XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml");
            BusInTravel tmpBusInTravel = busInTravelList.FirstOrDefault(busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.MyActivity == Activity.On);
            if (tmpBusInTravel != null)
            {
                tmpBusInTravel.MyActivity = Activity.Off;
                XMLTools.SaveListToXMLSerializer<BusInTravel>(busInTravelList, @"BusInTravel.xml");
            }
            throw new BadBusInTravelException("Bus In Travel doesn't exist", tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
        }
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void UpdateBusInTravel(BusInTravel busInTravelToUpdate)
        {
            BusInTravel tmpBusInTravel = XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml").FirstOrDefault(busInTravel => busInTravel.License == busInTravelToUpdate.License && busInTravel.Line == busInTravelToUpdate.Line && busInTravel.MyActivity == Activity.On);
            if (tmpBusInTravel == null)
                throw new BadBusInTravelException("Bus In Travel doesn't exist", busInTravelToUpdate.License, busInTravelToUpdate.Line, busInTravelToUpdate.DepartureTime);
            DeleteBusInTravel(tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
            AddBusInTravel(busInTravelToUpdate);
        }
        #endregion

        #region LineDeparting
        /// <summary>
        /// Get All Line Departing
        /// </summary>
        /// <returns>IEnumerable implemented by lineDeparting</returns>
        public IEnumerable<LineDeparting> GetAllLineDeparting()
        {
            var lineDepartingXel = XMLTools.LoadListFromXMLElement(@"LineDeparting.xml");
            return from myLineDeparting in lineDepartingXel.Elements()
                   where myLineDeparting.Element("MyActivity").Value == "On"
                   select new LineDeparting()
                   {
                       LineNumber = int.Parse(myLineDeparting.Element("LineNumber").Value),
                       StartTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StartTime").Value),
                       Frequency = XmlConvert.ToTimeSpan(myLineDeparting.Element("Frequency").Value),
                       StopTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StopTime").Value),
                       MyActivity = (Activity)Enum.Parse(typeof(Activity), myLineDeparting.Element("MyActivity").Value, true)
                   };
        }
        /// <summary>
        /// Get All Line Departing meets the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by lineDeparting</returns>
        public IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate)
        {
            var lineDepartingXel = XMLTools.LoadListFromXMLElement(@"LineDeparting.xml");
            IEnumerable<LineDeparting> myLineDeparting = from lineDeparting in lineDepartingXel.Elements()
                                                         where lineDepartingXel.Element("MyActivity").Value == "On"
                                                         let tmpLineDeparting = new LineDeparting()
                                                         {
                                                             LineNumber = int.Parse(lineDeparting.Element("LineNumber").Value),
                                                             StartTime = XmlConvert.ToTimeSpan(lineDeparting.Element("StartTime").Value),
                                                             Frequency = XmlConvert.ToTimeSpan(lineDeparting.Element("Frequency").Value),
                                                             StopTime = XmlConvert.ToTimeSpan(lineDeparting.Element("StopTime").Value),
                                                             MyActivity = (Activity)Enum.Parse(typeof(Activity), lineDeparting.Element("MyActivity").Value, true)
                                                         }
                                                         where predicate(tmpLineDeparting)
                                                         select tmpLineDeparting;
            if (myLineDeparting != null)
                return myLineDeparting;
            throw new ReadDataException("No LineDeparting meets the conditions");
        }
        /// <summary>
        /// Get solid Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public LineDeparting GetLineDeparting(int lineNumber, TimeSpan startTime)
        {
            var lineDepartingXel = XMLTools.LoadListFromXMLElement(@"LineDeparting.xml");
            LineDeparting line = (from myLineDeparting in lineDepartingXel.Elements()
                                  where myLineDeparting.Element("MyActivity").Value == "On"
                                  where int.Parse(myLineDeparting.Element("LineNumber").Value) == lineNumber
                                  where XmlConvert.ToTimeSpan(myLineDeparting.Element("startTime").Value) == startTime
                                  select new LineDeparting()
                                  {
                                      LineNumber = int.Parse(myLineDeparting.Element("LineNumber").Value),
                                      StartTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StartTime").Value),
                                      Frequency = XmlConvert.ToTimeSpan(myLineDeparting.Element("Frequency").Value),
                                      StopTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StopTime").Value),
                                      MyActivity = (Activity)Enum.Parse(typeof(Activity), myLineDeparting.Element("MyActivity").Value, true)
                                  }).FirstOrDefault();
            if (line != null)
                return line;
            throw new BadLineDepartingException("Linedeparting doesn't exist ", lineNumber, startTime);
        }
        /// <summary>
        /// Add Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="tmpLineDeparting">LineDeparting to add</param>
        public void AddLineDeparting(LineDeparting tmpLineDeparting)
        {
            var lineDepartingXel = XMLTools.LoadListFromXMLElement(@"LineDeparting.xml");
            LineDeparting line = (from myLineDeparting in lineDepartingXel.Elements()
                                  where myLineDeparting.Element("MyActivity").Value == "On"
                                  where int.Parse(myLineDeparting.Element("LineNumber").Value) == tmpLineDeparting.LineNumber
                                  where XmlConvert.ToTimeSpan(myLineDeparting.Element("StartTime").Value) == tmpLineDeparting.StartTime
                                  select new LineDeparting()
                                  {
                                      LineNumber = int.Parse(myLineDeparting.Element("LineNumber").Value),
                                      StartTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StartTime").Value),
                                      Frequency = XmlConvert.ToTimeSpan(myLineDeparting.Element("Frequency").Value),
                                      StopTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StopTime").Value),
                                      MyActivity = (Activity)Enum.Parse(typeof(Activity), myLineDeparting.Element("MyActivity").Value, true)
                                  }).FirstOrDefault();
            if (line != null)
                throw new BadLineDepartingException("Line Departing already exists", tmpLineDeparting.LineNumber, tmpLineDeparting.StartTime);
            XElement lineDepartingToAdd = new XElement("LineDeparting",
               new XElement("LineNumber", tmpLineDeparting.LineNumber),
               new XElement("StartTime", tmpLineDeparting.StartTime),
               new XElement("Frequency", tmpLineDeparting.Frequency),
               new XElement("StopTime", tmpLineDeparting.StopTime),
               new XElement("MyActivity", tmpLineDeparting.MyActivity)
               );
            lineDepartingXel.Add(lineDepartingToAdd);
            XMLTools.SaveListToXMLElement(lineDepartingXel, @"LineDeparting.xml");
        }
        /// <summary>
        /// Delete Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineNumber">Unique entity ID</param>
        /// <param name="startTime">Unique entity ID</param>
        public void DeleteLineDeparting(int lineNumber, TimeSpan startTime)
        {
            var lineDepartingXel = XMLTools.LoadListFromXMLElement(@"LineDeparting.xml");
            XElement line = (from myLineDeparting in lineDepartingXel.Elements()
                                  where myLineDeparting.Element("MyActivity").Value == "On"
                                  where int.Parse(myLineDeparting.Element("LineNumber").Value) == lineNumber
                                  where XmlConvert.ToTimeSpan(myLineDeparting.Element("StartTime").Value) == startTime
                                  select myLineDeparting).FirstOrDefault();
            if (line == null)
                throw new BadLineDepartingException("LineDeparture doesn't exist", lineNumber, startTime);
            line.Element("MyActivity").Value = Activity.Off.ToString();
            XMLTools.SaveListToXMLElement(lineDepartingXel, @"LineDeparting.xml");
        }
        /// <summary>
        /// Update Line Departing (delete old and add new)
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineDepartingToUpdate"></param>
        public void UpdateLineDeparting(LineDeparting lineDepartingToUpdate)
        {
            var lineDepartingXel = XMLTools.LoadListFromXMLElement(@"LineDeparting.xml");
            LineDeparting line = (from myLineDeparting in lineDepartingXel.Elements()
                                  where myLineDeparting.Element("MyActivity").Value == "On"
                                  where int.Parse(myLineDeparting.Element("LineNumber").Value) == lineDepartingToUpdate.LineNumber
                                  where XmlConvert.ToTimeSpan(myLineDeparting.Element("StartTime").Value) == lineDepartingToUpdate.StartTime
                                  select new LineDeparting()
                                  {
                                      LineNumber = int.Parse(myLineDeparting.Element("LineNumber").Value),
                                      StartTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StartTime").Value),
                                      Frequency = XmlConvert.ToTimeSpan(myLineDeparting.Element("Frequency").Value),
                                      StopTime = XmlConvert.ToTimeSpan(myLineDeparting.Element("StopTime").Value),
                                      MyActivity = (Activity)Enum.Parse(typeof(Activity), myLineDeparting.Element("MyActivity").Value, true)
                                  }).FirstOrDefault();
            if (line == null)
                throw new BadLineDepartingException("LineDeparture doesn't exist", lineDepartingToUpdate.LineNumber, lineDepartingToUpdate.StartTime);
            DeleteLineDeparting(line.LineNumber, line.StartTime);
            AddLineDeparting(lineDepartingToUpdate);
        }
        #endregion

        #region PairStation
        /// <summary>
        /// Get All PairStations
        /// </summary>
        /// <returns>IEnumerable implemented by pairStations</returns>
        public IEnumerable<PairStations> GetAllPairStations()
        {
            var pairStationsXel = XMLTools.LoadListFromXMLElement(@"PairStations.xml");
            return from pairStation in pairStationsXel.Elements()
                   where pairStation.Element("MyActivity").Value == "On"
                   select new PairStations()
                   {
                       FirstStationNumber = int.Parse(pairStation.Element("FirstStationNumber").Value),
                       LastStationNumber = int.Parse(pairStation.Element("LastStationNumber").Value),
                       Distance = XmlConvert.ToDouble(pairStation.Element("Distance").Value),
                       Time = XmlConvert.ToTimeSpan(pairStation.Element("Time").Value),
                       MyActivity = (Activity)Enum.Parse(typeof(Activity), pairStation.Element("MyActivity").Value, true),
                   };
        }
        /// <summary>
        /// Get All PairStations meets the codition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the codition (bool)</param>
        /// <returns>IEnumerable implemented by pairStations</returns>
        public IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate)
        {
            var pairStationsXel = XMLTools.LoadListFromXMLElement(@"PairStations.xml");
            IEnumerable<PairStations> pairStations = from pairStation in pairStationsXel.Elements()
                                                     where pairStation.Element("MyActivity").Value == "On"
                                                     let tmpPairStations = new PairStations()
                                                     {
                                                         FirstStationNumber = int.Parse(pairStation.Element("FirstStationNumber").Value),
                                                         LastStationNumber = int.Parse(pairStation.Element("LastStationNumber").Value),
                                                         Distance = XmlConvert.ToDouble(pairStation.Element("Distance").Value),
                                                         Time = XmlConvert.ToTimeSpan(pairStation.Element("Time").Value),
                                                         MyActivity = (Activity)Enum.Parse(typeof(Activity), pairStation.Element("MyActivity").Value, true),
                                                     }
                                                     where predicate(tmpPairStations)
                                                     select tmpPairStations;
            if (pairStations != null)
                return pairStations;
            throw new ReadDataException("No Pair station meets the conditions");
        }
        /// <summary>
        /// Get solid PairStations
        /// throw BadPairStationException
        /// </summary>
        /// <param name="firstStation">Unique entity ID</param>
        /// <param name="secondStation">Unique entity ID</param>
        /// <returns></returns>
        public PairStations GetPairStations(int firstStation, int secondStation)
        {
            var pairStationsXel = XMLTools.LoadListFromXMLElement(@"PairStations.xml");
            PairStations pair = (from pairStation in pairStationsXel.Elements()
                                 where pairStation.Element("MyActivity").Value == "On"
                                 where int.Parse(pairStation.Element("FirstStationNumber").Value) == firstStation
                                 where int.Parse(pairStation.Element("LastStationNumber").Value) == secondStation
                                 select new PairStations()
                                 {
                                     FirstStationNumber = int.Parse(pairStation.Element("FirstStationNumber").Value),
                                     LastStationNumber = int.Parse(pairStation.Element("LastStationNumber").Value),
                                     Distance = XmlConvert.ToDouble(pairStation.Element("Distance").Value),
                                     Time = XmlConvert.ToTimeSpan(pairStation.Element("Time").Value),
                                     MyActivity = (Activity)Enum.Parse(typeof(Activity), pairStation.Element("MyActivity").Value),
                                 }).FirstOrDefault();

            if (pair != null)
                return pair.Clone();
            throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
        }
        /// <summary>
        /// Add Pair Stations
        /// throw BadPairStationException
        /// </summary>
        /// <param name="tmpPairStations">PairStations to add</param>
        public void AddPairStations(PairStations tmpPairStations)
        {
            var pairStationsXel = XMLTools.LoadListFromXMLElement(@"PairStations.xml");
            PairStations pair = (from pairStation in pairStationsXel.Elements()
                                 where pairStation.Element("MyActivity").Value == "On"
                                 where int.Parse(pairStation.Element("FirstStationNumber").Value) == tmpPairStations.FirstStationNumber
                                 where int.Parse(pairStation.Element("LastStationNumber").Value) == tmpPairStations.LastStationNumber
                                 select new PairStations()
                                 {
                                     FirstStationNumber = int.Parse(pairStation.Element("FirstStationNumber").Value),
                                     LastStationNumber = int.Parse(pairStation.Element("LastStationNumber").Value),
                                     Distance = XmlConvert.ToDouble(pairStation.Element("Distance").Value),
                                     Time = XmlConvert.ToTimeSpan(pairStation.Element("Time").Value),
                                     MyActivity = (Activity)Enum.Parse(typeof(Activity), pairStation.Element("MyActivity").Value, true),
                                 }).FirstOrDefault();
            if (pair != null)
                throw new BadPairStationException("Pair Station already exist", tmpPairStations.FirstStationNumber, tmpPairStations.LastStationNumber);
            XElement pairStationToAdd = new XElement("PairStation",
                new XElement("FirstStationNumber", tmpPairStations.FirstStationNumber),
                new XElement("LastStationNumber", tmpPairStations.LastStationNumber),
                new XElement("Distance", tmpPairStations.Distance),
                new XElement("Time", tmpPairStations.Time),
                new XElement("MyActivity", tmpPairStations.MyActivity)
                );
            pairStationsXel.Add(pairStationToAdd);
            XMLTools.SaveListToXMLElement(pairStationsXel, @"PairStations.xml");
        }
        /// <summary>
        /// Delete PairStations
        /// throw BadPairStationException
        /// </summary>
        /// <param name="firstStation">Unique entity ID</param>
        /// <param name="secondStation">Unique entity ID</param>
        public void DeletePairStations(int firstStation, int secondStation)
        {
            var pairStationsXel = XMLTools.LoadListFromXMLElement(@"PairStations.xml");
            XElement pair = (from pairStation in pairStationsXel.Elements()
                             where pairStation.Element("MyActivity").Value == "On"
                             where int.Parse(pairStation.Element("FirstStationNumber").Value) == firstStation
                             where int.Parse(pairStation.Element("LastStationNumber").Value) == secondStation
                             select pairStation).FirstOrDefault();
            if (pair == null)
                throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
            pair.Element("MyActivity").Value = Activity.Off.ToString();
            XMLTools.SaveListToXMLElement(pairStationsXel, @"PairStations.xml");
        }
        /// <summary>
        /// Update PairStations (delete old and add new)
        /// throw BadPairStationException
        /// </summary>
        /// <param name="pairStationsToUpdate"></param>
        public void UpdatePairStations(PairStations pairStationsToUpdate)
        {
            var pairStationsXel = XMLTools.LoadListFromXMLElement(@"PairStations.xml");
            PairStations pair = (from pairStation in pairStationsXel.Elements()
                                 where pairStation.Element("MyActivity").Value == "On"
                                 where int.Parse(pairStation.Element("FirstStationNumber").Value) == pairStationsToUpdate.FirstStationNumber
                                 where int.Parse(pairStation.Element("LastStationNumber").Value) == pairStationsToUpdate.LastStationNumber
                                 select new PairStations()
                                 {
                                     FirstStationNumber = int.Parse(pairStation.Element("FirstStationNumber").Value),
                                     LastStationNumber = int.Parse(pairStation.Element("LastStationNumber").Value),
                                     Distance = XmlConvert.ToDouble(pairStation.Element("Distance").Value),
                                     Time = XmlConvert.ToTimeSpan(pairStation.Element("Time").Value),
                                     MyActivity = (Activity)Enum.Parse(typeof(Activity), pairStation.Element("MyActivity").Value, true),
                                 }).FirstOrDefault();
            if (pair == null)
                throw new BadPairStationException("Pair Station doesn't exist", pairStationsToUpdate.FirstStationNumber, pairStationsToUpdate.LastStationNumber);
            DeletePairStations(pair.FirstStationNumber, pair.LastStationNumber);
            AddPairStations(pairStationsToUpdate);
        }
        #endregion

        #region UserTrip
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<UserTrip> GetAllUserTrip()
        {
            return from userTrip in XMLTools.LoadListFromXMLSerializer<UserTrip>(@"UserTrip.xml")
                   where userTrip.MyActivity == Activity.On
                   select userTrip.Clone();
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate)
        {
            IEnumerable<UserTrip> myUserTrip = from UserTrip in XMLTools.LoadListFromXMLSerializer<UserTrip>(@"UserTrip.xml")
                                               where UserTrip.MyActivity == Activity.On && predicate(UserTrip)
                                               select UserTrip.Clone();
            if (myUserTrip != null)
                return myUserTrip;
            throw new ReadDataException("No UserTrip meets the conditions");
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public UserTrip GetUserTrip(string name)
        {
            UserTrip myUserTrip = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"UserTrip.xml").FirstOrDefault(
                userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.On);
            if (myUserTrip != null)
                return myUserTrip.Clone();
            throw new BadUserTripException("User Trip doesn't exist", name);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void AddUserTrip(UserTrip tmpUserTrip)
        {
            var userTripList = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"UserTrip.xml");
            if (userTripList.FirstOrDefault(
                            userTrip => userTrip.UserName == tmpUserTrip.UserName && userTrip.MyActivity == Activity.On) != null)
            {
                UserTrip myUserTrip = tmpUserTrip.Clone();
                myUserTrip.Key = Config.UserTripCounter;
                userTripList.Add(myUserTrip);
                XMLTools.SaveListToXMLSerializer<UserTrip>(userTripList, @"UserTrip.xml");
            }
            throw new BadUserTripException("User Trip already exist", tmpUserTrip.UserName);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void DeleteUserTrip(string name)
        {
            var userTripList = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"UserTrip.xml");
            UserTrip tmpUserTrip = userTripList.FirstOrDefault(userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.On);
            if (tmpUserTrip != null)
            {
                tmpUserTrip.MyActivity = Activity.Off;
                XMLTools.SaveListToXMLSerializer<UserTrip>(userTripList, @"UserTrip.xml");
            }
            throw new BadUserTripException("User Trip doesn't exist", tmpUserTrip.UserName);

        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void UpdateUserTrip(UserTrip userTripToUpdate)
        {
            UserTrip tmpUserTrip = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"UserTrip.xml").FirstOrDefault(userTrip => userTrip.UserName == userTripToUpdate.UserName && userTrip.MyActivity == Activity.On);
            if (tmpUserTrip == null)
                throw new BadUserTripException("User Trip doesn't exist", userTripToUpdate.UserName);
            DeleteUserTrip(tmpUserTrip.UserName);
            AddUserTrip(userTripToUpdate);

        }
        #endregion
    }
}