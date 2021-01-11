using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace DL
{
    sealed class DalXml : IDL
    {
        #region singelton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }// static ctor to ensure instance init is done just before first usage
        DalXml() { } // default => private
        public static DalXml Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus
        public IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate)
        {
            IEnumerable<Bus> allBuses = from bus in XMLTools.LoadListFromXMLSerializer<Bus>(@"bin\Xml\Bus.xml")
                                        where bus.MyActivity == Activity.On
                                        where predicate(bus)
                                        select bus.Clone();
            if (allBuses != null)
            {
                return allBuses;
            }
            throw new ReadDataException("No Bus meets the conditions");
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from Bus in XMLTools.LoadListFromXMLSerializer<Bus>(@"bin\Xml\Bus.xml")
                   where Bus.MyActivity == Activity.On
                   select Bus.Clone();
        }

        public Bus GetBus(int license)
        {
            Bus tmpBus = XMLTools.LoadListFromXMLSerializer<Bus>(@"bin\Xml\Bus.xml").Find(bus => bus.License == license && bus.MyActivity == Activity.On);
            if (tmpBus != null)
            {
                return tmpBus.Clone();
            }
            throw new BadBusException("Bus doesn't exist", license);
        }

        public void AddBus(Bus myBus)
        {
            var busList = XMLTools.LoadListFromXMLSerializer<Bus>(@"bin\Xml\Bus.xml");
            if (busList.FirstOrDefault(bus => bus.License == myBus.License && bus.MyActivity == Activity.On) != null)
                throw new BadBusException("Bus already exist", myBus.License);
            busList.Add(myBus.Clone());
            XMLTools.SaveListToXMLSerializer<Bus>(busList, @"bin\Xml\Bus.xml");
        }

        public void UpdateBus(Bus busToUpdate)
        {
            Bus tmpBus = XMLTools.LoadListFromXMLSerializer<Bus>(@"bin\Xml\Bus.xml").FirstOrDefault(bus => bus.License == busToUpdate.License && bus.MyActivity == Activity.On);
            if (tmpBus == null)
                throw new BadBusException("Bus doesn't exist", busToUpdate.License);
            DeleteBus(tmpBus.License);
            AddBus(busToUpdate);
        }

        public void DeleteBus(int license)
        {
            var busList = XMLTools.LoadListFromXMLSerializer<Bus>(@"bin\Xml\Bus.xml");
            Bus tmpBus = busList.FirstOrDefault(bus => bus.License == license && bus.MyActivity == Activity.On);
            if (tmpBus == null)
                throw new BadBusException("Bus doesn't exist", license);
            tmpBus.MyActivity = Activity.Off;
            XMLTools.SaveListToXMLSerializer<Bus>(busList, @"bin\Xml\Bus.xml");
        }
        #endregion

        #region Line
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in XMLTools.LoadListFromXMLSerializer<BusLine>(@"bin\Xml\BusLine.xml")
                   where busLine.MyActivity == Activity.On
                   select busLine.Clone();
        }

        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            IEnumerable<BusLine> myLineList = from busLine in XMLTools.LoadListFromXMLSerializer<BusLine>(@"bin\Xml\BusLine.xml")
                                              where predicate(busLine)
                                              where busLine.MyActivity == Activity.On
                                              select busLine.Clone();
            if (myLineList != null)
                return myLineList;
            throw new ReadDataException("No Line meets the conditions");
        }

        public BusLine GetBusLine(int id)
        {
            BusLine myBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(@"bin\Xml\BusLine.xml").Find(line => line.LineNumber == id && line.MyActivity == Activity.On);
            if (myBusLine != null)
                return myBusLine.Clone();
            throw new BadLineException("the Line doesn't exist", id);
        }

        public void AddLine(BusLine tmpBusLine)
        {
            var lineList = XMLTools.LoadListFromXMLSerializer<BusLine>(@"bin\Xml\BusLine.xml");
            if (lineList.FirstOrDefault(Line => Line.LineNumber == tmpBusLine.LineNumber && Line.MyActivity == Activity.On) != null)
                throw new BadLineException("the Line already exist", tmpBusLine.LineNumber);
            BusLine myBusLine = tmpBusLine.Clone();
            myBusLine.Key = Config.BusLineCounter;
            lineList.Add(myBusLine);
            XMLTools.SaveListToXMLSerializer<BusLine>(lineList, @"bin\Xml\BusLine.xml");
        }

        public void UpdateLine(BusLine lineToUpdate)
        {
            var lineList = XMLTools.LoadListFromXMLSerializer<BusLine>(@"bin\Xml\BusLine.xml");
            BusLine tmpLine = lineList.FirstOrDefault(line => line.LineNumber == lineToUpdate.LineNumber && line.MyActivity == Activity.On);
            if (tmpLine == null)
                throw new BadLineException("the Line doesn't exist", lineToUpdate.LineNumber);
            DeleteLine(tmpLine.LineNumber);
            AddLine(lineToUpdate);
        }

        public void DeleteLine(int lineNumber)
        {
            var lineList = XMLTools.LoadListFromXMLSerializer<BusLine>(@"bin\Xml\BusLine.xml");
            BusLine tmpLine = lineList.FirstOrDefault(line => line.LineNumber == lineNumber && line.MyActivity == Activity.On);
            if (tmpLine == null)
                throw new BadLineException("the Line doesn't exist", lineNumber);
            tmpLine.MyActivity = Activity.Off;
            XMLTools.SaveListToXMLSerializer<BusLine>(lineList, @"bin\Xml\BusLine.xml");
        }
        #endregion

        #region Station
        public IEnumerable<Station> GetAllStations()
        {
            return from station in XMLTools.LoadListFromXMLSerializer<Station>(@"bin\Xml\Station.xml")
                   where station.MyActivity == Activity.On
                   select station.Clone();
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            IEnumerable<Station> myStationsList = from station in XMLTools.LoadListFromXMLSerializer<Station>(@"bin\Xml\Station.xml")
                                                  where station.MyActivity == Activity.On
                                                  where predicate(station)
                                                  select station.Clone();
            if (myStationsList == null)
                throw new ReadDataException("No Station meets the conditions");
            return myStationsList;
        }

        public Station GetStation(int id)
        {
            Station myStation = XMLTools.LoadListFromXMLSerializer<Station>(@"bin\Xml\Station.xml").Find(Station => Station.StationId == id && Station.MyActivity == Activity.On);
            if (myStation != null)
                return myStation.Clone();
            throw new BadStationException("Station doesn't exist", id);
        }

        public void AddStation(Station tmpStation)
        {
            var stationsList = XMLTools.LoadListFromXMLSerializer<Station>(@"bin\Xml\Station.xml");
            if (stationsList.FirstOrDefault(station => station.StationId == tmpStation.StationId && station.MyActivity == Activity.On) != null)
                throw new BadStationException("Station already exist", tmpStation.StationId);
            stationsList.Add(tmpStation.Clone());
            XMLTools.SaveListToXMLSerializer<Station>(stationsList, @"bin\Xml\Station.xml");
        }

        public void DeleteStation(int id)
        {
            var stationsList = XMLTools.LoadListFromXMLSerializer<Station>(@"bin\Xml\Station.xml");
            Station myStation = stationsList.FirstOrDefault(station => station.StationId == id && station.MyActivity == Activity.On);
            if (myStation == null)
                throw new BadStationException("Station doesn't exist", id);
            myStation.MyActivity = Activity.Off;
            XMLTools.SaveListToXMLSerializer<Station>(stationsList, @"bin\Xml\Station.xml");
        }
        #endregion

        #region User
        public IEnumerable<User> GetAllUsers()
        {
            return from user in XMLTools.LoadListFromXMLSerializer<User>(@"bin\Xml\User.xml")
                   where user.MyActivity == Activity.On
                   select user.Clone();
        }

        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            IEnumerable<User> myUsers = from user in XMLTools.LoadListFromXMLSerializer<User>(@"bin\Xml\User.xml")
                                        where user.MyActivity == Activity.On
                                        where predicate(user)
                                        select user.Clone();
            if (myUsers == null)
                throw new ReadDataException("No User meets the conditions");
            return myUsers;
        }

        public User GetUser(string userName)
        {
            User myUser = XMLTools.LoadListFromXMLSerializer<User>(@"User.xml").FirstOrDefault(user => user.UserName == userName && user.MyActivity == Activity.On);
            if (myUser != null)
                return myUser.Clone();
            throw new BadUserException("User doesn't exist", userName);
        }

        public void UpdateUser(User userToUpdate)
        {
            User tmpUser = XMLTools.LoadListFromXMLSerializer<User>(@"User.xml").FirstOrDefault(user => user.UserName == userToUpdate.UserName && user.MyActivity == Activity.On);
            if (tmpUser == null)
                throw new BadUserException("User doesn't exist", userToUpdate.UserName);
            DeleteUser(tmpUser.UserName);
            AddUser(userToUpdate);
        }

        public void AddUser(User tmpUser)
        {
            var userList = XMLTools.LoadListFromXMLSerializer<User>(@"User.xml");
            if (userList.FirstOrDefault(user => user.UserName == tmpUser.UserName && user.MyActivity == Activity.On) != null)
                throw new BadUserException("User already exist", tmpUser.UserName);
            userList.Add(tmpUser.Clone());
            XMLTools.SaveListToXMLSerializer<User>(userList, @"User.xml");
        }

        public void DeleteUser(string userName)
        {
            var userList = XMLTools.LoadListFromXMLSerializer<User>(@"User.xml");
            User myUser = userList.FirstOrDefault(user => user.UserName == userName && user.MyActivity == Activity.On);
            if (myUser != null)
            {
                myUser.MyActivity = Activity.Off;
                XMLTools.SaveListToXMLSerializer<User>(userList, @"User.xml");
            }
            else throw new BadUserException("User doesn't exist", userName);
        }
        #endregion

        #region LineStation
        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from lineStation in XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml")
                   where lineStation.MyActivity == Activity.On
                   select lineStation.Clone();
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            IEnumerable<LineStation> myLineStations = from lineStation in XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml")
                                                      where predicate(lineStation) && lineStation.MyActivity == Activity.On
                                                      select lineStation.Clone();
            if (myLineStations != null)
                return myLineStations;
            throw new ReadDataException("No LineStation meets the conditions");
        }

        public LineStation GetLineStation(int stationNumber, int lineNumber)
        {
            LineStation myLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(@"LineStation.xml").FirstOrDefault(
                station => station.LineNumber == lineNumber && station.StationNumber == stationNumber && station.MyActivity == Activity.On);
            if (myLineStation != null)
                return myLineStation.Clone();
            throw new BadLineStationException("Line Station doesn't exist", lineNumber, stationNumber);
        }

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
        public IEnumerable<BusInTravel> GetAllBusInTravel()
        {
            return from busInTravel in XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml")
                   where busInTravel.MyActivity == Activity.On
                   select busInTravel.Clone();
        }

        public IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate)
        {
            IEnumerable<BusInTravel> myBusInTravel = from busInTravel in XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml")
                                                     where busInTravel.MyActivity == Activity.On && predicate(busInTravel)
                                                     select busInTravel.Clone();
            if (myBusInTravel != null)
                return myBusInTravel;
            throw new ReadDataException("No BusInTravel meets the conditions");
        }

        public BusInTravel GetBusInTravel(int license, int line, DateTime departureTime)
        {
            BusInTravel myBusInTravel = XMLTools.LoadListFromXMLSerializer<BusInTravel>(@"BusInTravel.xml").FirstOrDefault(
                busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.DepartureTime == departureTime && busInTravel.MyActivity == Activity.On);
            if (myBusInTravel != null)
                return myBusInTravel.Clone();
            throw new BadBusInTravelException("Bus In Travel doesn't exist", license, line, departureTime);
        }

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
        public IEnumerable<LineDeparting> GetAllLineDeparting()
        {
            return from lineDeparting in XMLTools.LoadListFromXMLSerializer<LineDeparting>(@"LineDeparting.xml")
                   where lineDeparting.MyActivity == Activity.On
                   select lineDeparting.Clone();
        }

        public IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate)
        {
            IEnumerable<LineDeparting> mylineDeparting = from lineDeparting in XMLTools.LoadListFromXMLSerializer<LineDeparting>(@"LineDeparting.xml")
                                                         where lineDeparting.MyActivity == Activity.On && predicate(lineDeparting)
                                                         select lineDeparting.Clone();
            if (mylineDeparting != null)
                return mylineDeparting;
            throw new ReadDataException("No LineDeparting meets the conditions");
        }

        public LineDeparting GetLineDeparting(int lineNumber, TimeSpan startTime)
        {
            LineDeparting line = XMLTools.LoadListFromXMLSerializer<LineDeparting>(@"LineDeparting.xml").FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineNumber && lineDeparting.StartTime == startTime && lineDeparting.MyActivity == Activity.On);
            if (line != null)
                return line.Clone();
            throw new BadLineDepartingException("Linedeparting doesn't exist ", lineNumber, startTime);
        }

        public void AddLineDeparting(LineDeparting tmpLineDeparting)
        {
            var LineDepartingList = XMLTools.LoadListFromXMLSerializer<LineDeparting>(@"LineDeparting.xml");
            LineDeparting line = LineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == tmpLineDeparting.LineNumber && lineDeparting.StartTime == tmpLineDeparting.StartTime && lineDeparting.MyActivity == Activity.On);
            if (line != null)
                throw new BadLineDepartingException("Line Departing already exists", tmpLineDeparting.LineNumber, tmpLineDeparting.StartTime);
            LineDepartingList.Add(tmpLineDeparting.Clone());
            XMLTools.SaveListToXMLSerializer<LineDeparting>(LineDepartingList, @"LineDeparting.xml");
        }

        public void DeleteLineDeparting(int lineNumber, TimeSpan startTime)
        {
            LineDeparting line = XMLTools.LoadListFromXMLSerializer<LineDeparting>(@"LineDeparting.xml").FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineNumber && lineDeparting.StartTime == startTime && lineDeparting.MyActivity == Activity.On);
            if (line == null)
                throw new BadLineDepartingException("LineDeparture doesn't exist", lineNumber, startTime);
            line.MyActivity = Activity.Off;
        }

        public void UpdateLineDeparting(LineDeparting lineDepartingToUpdate)
        {
            LineDeparting tmpline = XMLTools.LoadListFromXMLSerializer<LineDeparting>(@"LineDeparting.xml").FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineDepartingToUpdate.LineNumber && lineDeparting.StartTime == lineDepartingToUpdate.StartTime && lineDeparting.MyActivity == Activity.On);
            if (tmpline == null)
                throw new BadLineDepartingException("LineDeparture doesn't exist", lineDepartingToUpdate.LineNumber, lineDepartingToUpdate.StartTime);
            DeleteLineDeparting(tmpline.LineNumber, tmpline.StartTime);
            AddLineDeparting(lineDepartingToUpdate);
        }
        #endregion

        #region PairStation
        public IEnumerable<PairStations> GetAllPairStations()
        {
            return from pairstation in XMLTools.LoadListFromXMLSerializer<PairStations>(@"PairStations.xml")
                   where pairstation.MyActivity == Activity.On
                   select pairstation.Clone();
        }

        public IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate)
        {
            IEnumerable<PairStations> pairStations = from pairstation in XMLTools.LoadListFromXMLSerializer<PairStations>(@"PairStations.xml")
                                                     where pairstation.MyActivity == Activity.On && predicate(pairstation)
                                                     select pairstation.Clone();
            if (pairStations != null)
                return pairStations;
            throw new ReadDataException("No Pair station meets the conditions");
        }

        public PairStations GetPairStations(int firstStation, int secondStation)
        {
            PairStations pair = XMLTools.LoadListFromXMLSerializer<PairStations>(@"PairStations.xml").FirstOrDefault
                (pairStations => pairStations.FirstStationNumber == firstStation && pairStations.LastStationNumber == secondStation && pairStations.MyActivity == Activity.On);
            if (pair != null)
                return pair.Clone();
            throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
        }

        public void AddPairStations(PairStations tmpPairStations)
        {
            var PairStationsList = XMLTools.LoadListFromXMLSerializer<PairStations>(@"PairStations .xml");
            PairStations pair = PairStationsList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == tmpPairStations.FirstStationNumber && pairStations.LastStationNumber == tmpPairStations.LastStationNumber && pairStations.MyActivity == Activity.On);
            if (pair != null)
                throw new BadPairStationException("Pair Station already exist", tmpPairStations.FirstStationNumber, tmpPairStations.LastStationNumber);
            PairStationsList.Add(tmpPairStations.Clone());
            XMLTools.SaveListToXMLSerializer<PairStations>(PairStationsList, @"PairStations .xml");
        }

        public void DeletePairStations(int firstStation, int secondStation)
        {
            var PairStationsList = XMLTools.LoadListFromXMLSerializer<PairStations>(@"~bin\Xml\PairStations .xml");
            PairStations pair = PairStationsList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == firstStation && pairStations.LastStationNumber == secondStation && pairStations.MyActivity == Activity.On);
            if (pair == null)
                throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
            pair.MyActivity = Activity.Off;
            XMLTools.SaveListToXMLSerializer<PairStations>(PairStationsList, @"~bin\Xml\PairStations .xml");
        }

        public void UpdatePairStations(PairStations pairStationsToUpdate)
        {
            PairStations tmpPair = XMLTools.LoadListFromXMLSerializer<PairStations>(@"~bin\Xml\PairStations.xml").FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == pairStationsToUpdate.FirstStationNumber && pairStations.LastStationNumber == pairStationsToUpdate.LastStationNumber && pairStations.MyActivity == Activity.On);
            if (tmpPair == null)
                throw new BadPairStationException("Pair Station doesn't exist", pairStationsToUpdate.FirstStationNumber, pairStationsToUpdate.LastStationNumber);
            DeletePairStations(tmpPair.FirstStationNumber, tmpPair.LastStationNumber);
            AddPairStations(pairStationsToUpdate);
        }
        #endregion

        #region UserTrip
        public IEnumerable<UserTrip> GetAllUserTrip()
        {
            return from userTrip in XMLTools.LoadListFromXMLSerializer<UserTrip>(@"~bin\Xml\UserTrip.xml")
                   where userTrip.MyActivity == Activity.On
                   select userTrip.Clone();
        }

        public IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate)
        {
            IEnumerable<UserTrip> myUserTrip = from UserTrip in XMLTools.LoadListFromXMLSerializer<UserTrip>(@"~bin\Xml\UserTrip.xml")
                                               where UserTrip.MyActivity == Activity.On && predicate(UserTrip)
                                               select UserTrip.Clone();
            if (myUserTrip != null)
                return myUserTrip;
            throw new ReadDataException("No UserTrip meets the conditions");
        }

        public UserTrip GetUserTrip(string name)
        {
            UserTrip myUserTrip = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"~bin\Xml\UserTrip.xml").FirstOrDefault(
                userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.On);
            if (myUserTrip != null)
                return myUserTrip.Clone();
            throw new BadUserTripException("User Trip doesn't exist", name);
        }

        public void AddUserTrip(UserTrip tmpUserTrip)
        {
            var userTripList = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"~bin\Xml\UserTrip.xml");
            if (userTripList.FirstOrDefault(
                            userTrip => userTrip.UserName == tmpUserTrip.UserName && userTrip.MyActivity == Activity.On) != null)
            {
                UserTrip myUserTrip = tmpUserTrip.Clone();
                myUserTrip.Key = Config.UserTripCounter;
                userTripList.Add(myUserTrip);
                XMLTools.SaveListToXMLSerializer<UserTrip>(userTripList, @"~bin\Xml\UserTrip.xml");
            }
            throw new BadUserTripException("User Trip already exist", tmpUserTrip.UserName);
        }

        public void DeleteUserTrip(string name)
        {
            var userTripList = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"~bin\Xml\UserTrip.xml");
            UserTrip tmpUserTrip = userTripList.FirstOrDefault(userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.On);
            if (tmpUserTrip != null) {
                tmpUserTrip.MyActivity = Activity.Off;
                XMLTools.SaveListToXMLSerializer<UserTrip>(userTripList, @"~bin\Xml\UserTrip.xml");
            }
            throw new BadUserTripException("User Trip doesn't exist", tmpUserTrip.UserName);

        }

        public void UpdateUserTrip(UserTrip userTripToUpdate)
        {
            UserTrip tmpUserTrip = XMLTools.LoadListFromXMLSerializer<UserTrip>(@"~bin\Xml\UserTrip.xml").FirstOrDefault(userTrip => userTrip.UserName == userTripToUpdate.UserName && userTrip.MyActivity == Activity.On);
            if (tmpUserTrip == null)
                throw new BadUserTripException("User Trip doesn't exist", userTripToUpdate.UserName);
            DeleteUserTrip(tmpUserTrip.UserName);
            AddUserTrip(userTripToUpdate);

        }
        #endregion
    }
}