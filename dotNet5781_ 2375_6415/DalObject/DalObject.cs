using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using DS;


namespace DalObject
{
    sealed class DalObject : IDL
    {
        #region singelton
        static readonly DalObject instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus
        public IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate)
        {
            IEnumerable<Bus> allBuses = from Bus in DataSource.busList
                                        where predicate(Bus)
                                        select Bus.Clone();
            if (allBuses != null)
            {
                return allBuses;
            }
            throw new ReadDataException("No Bus meets the conditions");
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from Bus in DataSource.busList
                   select Bus.Clone();
        }
        public Bus GetBus(int license)
        {
            Bus tmpBus = DataSource.busList.Find(bus => bus.License == license);
            if (tmpBus != null)
            {
                return tmpBus.Clone();
            }
            throw new BadBusException("Bus doesn't exist", license);
        }

        public void AddBus(Bus myBus)
        {
            if (DataSource.busList.FirstOrDefault(bus => bus.License == myBus.License) != null)
                throw new BadBusException("Bus already exist", myBus.License);
            DataSource.busList.Add(myBus.Clone());
        }

        public void UpdateBus(int license, Action<Bus> update)
        {
            Bus tmpBus = DataSource.busList.FirstOrDefault(bus => bus.License == license);
            if (tmpBus == null)
                throw new BadBusException("Bus doesn't exist", license);
            update(tmpBus);
        }
        public void DeleteBus(int license)
        {
            Bus tmpBus = DataSource.busList.FirstOrDefault(bus => bus.License == license);
            if (tmpBus == null)
                throw new BadBusException("Bus doesn't exist", license);
            DataSource.busList.Remove(tmpBus);
        }
        #endregion

        #region Line
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in DataSource.lineList
                   select busLine.Clone();
        }

        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            IEnumerable<BusLine> myBusList = from busLine in DataSource.lineList
                                             where predicate(busLine)
                                             select busLine.Clone();
            if (myBusList != null)
                return myBusList;
            throw new ReadDataException("No Line meets the conditions");
        }
        public BusLine GetBusLine(int id)
        {
            BusLine myBusLine = DataSource.lineList.Find(line => line.LineNumber == id);
            if (myBusLine != null)
                return myBusLine.Clone();
            throw new BadLineException("the Line doesn't exist", id);
        }

        public void AddLine(BusLine tmpBusLine)
        {
            if (DataSource.lineList.FirstOrDefault(Line => Line.LineNumber == tmpBusLine.LineNumber) != null)
                throw new BadLineException("the Line already exist", tmpBusLine.LineNumber);
            BusLine myBusLine = tmpBusLine.Clone();
            myBusLine.Key = Config.BusLineCounter;
            DataSource.lineList.Add(myBusLine);
        }
        public void UpdateLine(int lineNumber, Action<BusLine> update)
        {
            BusLine tmpLine = DataSource.lineList.FirstOrDefault(line => line.LineNumber == lineNumber);
            if (tmpLine == null)
                throw new BadLineException("the Line doesn't exist", lineNumber);
            update(tmpLine);
        }

        public void DeleteLine(int lineNumber)
        {
            BusLine tmpLine = DataSource.lineList.FirstOrDefault(line => line.LineNumber == lineNumber);
            if (tmpLine == null)
                throw new BadLineException("the Line doesn't exist", lineNumber);
            DataSource.lineList.Remove(tmpLine);
        }
        #endregion

        #region Station
        public IEnumerable<Station> GetAllStations()
        {
            return from station in DataSource.stationList
                   select station.Clone();
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            IEnumerable<Station> myStationsList = from station in DataSource.stationList
                                                  where predicate(station)
                                                  select station.Clone();
            if (myStationsList == null)
                throw new ReadDataException("No Station meets the conditions");
            return myStationsList;
        }
        public Station GetStation(int id)
        {
            Station myStation = DataSource.stationList.Find(Station => Station.StationId == id);
            if (myStation != null)
                return myStation.Clone();
            throw new BadStationException("Station doesn't exist", id);
        }

        public void AddStation(Station tmpStation)
        {
            if (DataSource.stationList.FirstOrDefault(station =>station.StationId == tmpStation.StationId) != null)
                throw new BadStationException("Station already exist", tmpStation.StationId);
            DataSource.stationList.Add(tmpStation.Clone());
        }
        public void DeleteStation(int id)
        {
            Station myStation = DataSource.stationList.FirstOrDefault(station => station.StationId == id);
            if (myStation == null)
                throw new BadStationException("Station doesn't exist", id);
            DataSource.stationList.Remove(myStation);
        }
        #endregion

        #region User
        public IEnumerable<User> GetAllUsers()
        {
            return from user in DataSource.userList
                   select user.Clone();
        }

        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            IEnumerable<User> myUsers = from user in DataSource.userList
                                        where predicate(user)
                                        select user.Clone();
            if(myUsers == null)
                throw new ReadDataException("No User meets the conditions");
            return myUsers;
        }
        public User GetUser(string userName)
        {
            User myUser = DataSource.userList.FirstOrDefault(user => user.UserName == userName);
            if (myUser != null)
                return myUser.Clone();
            throw new BadUserException("User doesn't exist", userName);
        }

        public void UpdateUser(string userName, Action<User> update)
        {
            User myUser = DataSource.userList.FirstOrDefault(user => user.UserName == userName);
            if (myUser != null)
                update(myUser);
            throw new BadUserException("User doesn't exist", userName);
        }

        public void AddUser(User tmpUser)
        {
            if (DataSource.userList.FirstOrDefault(user => user.UserName == tmpUser.UserName) != null)
                throw new BadUserException("User already exist", tmpUser.UserName);
            DataSource.userList.Add(tmpUser.Clone());
        }
        public void DeleteUser(string userName)
        {
            User myUser = DataSource.userList.FirstOrDefault(user => user.UserName == userName);
            if (myUser != null)
                DataSource.userList.Remove(myUser);
            throw new BadUserException("User doesn't exist", userName);
        }
        #endregion

        #region LineStation
        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from lineStation in DataSource.linestationList
                   select lineStation.Clone();
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            IEnumerable<LineStation> myLineStations = from lineStation in DataSource.linestationList
                                                      where predicate(lineStation)
                                                      select lineStation.Clone();
            if (myLineStations != null)
                return myLineStations;
            throw new ReadDataException("No LineStation meets the conditions");
        }

        public LineStation GetLineStation(int stationNumber, int lineNumber)
        {
            LineStation myLineStation = DataSource.linestationList.FirstOrDefault(
                station =>station.LineNumber == lineNumber && station.StationNumber == stationNumber);
            if (myLineStation != null)
                return myLineStation.Clone();
            throw new BadLineStationException("Line Station doesn't exist", lineNumber,stationNumber);
        }

        public void AddLineStation(LineStation tmpLineStation)
        {
            if (DataSource.linestationList.FirstOrDefault(station => station.LineNumber == tmpLineStation.LineNumber && station.StationNumber == tmpLineStation.StationNumber) != null)
                DataSource.linestationList.Add(tmpLineStation.Clone());
            throw new BadLineStationException("Line Station already exist", tmpLineStation.LineNumber, tmpLineStation.StationNumber);
        }

        public void DeleteLineStation(int stationNumber, int lineNumber)
        {
            LineStation tmpLineStation = DataSource.linestationList.FirstOrDefault(station => station.LineNumber == lineNumber && station.StationNumber == stationNumber);
            if (tmpLineStation != null)
                DataSource.linestationList.Remove(tmpLineStation);
            throw new BadLineStationException("Line Station doesn't exist", lineNumber, stationNumber);
        }

        public void UpdateLineStation(int stationNumber, int lineNumber, Action<LineStation> update)
        {
            LineStation tmpLineStation = DataSource.linestationList.FirstOrDefault(station => station.LineNumber == lineNumber && station.StationNumber == stationNumber);
            if (tmpLineStation != null)
                update(tmpLineStation);
            throw new BadLineStationException("Line Station doesn't exist", lineNumber, stationNumber);
        }
        #endregion

        #region BusInTravel
        public IEnumerable<BusInTravel> GetAllBusInTravel()
        {
            return from busInTravel in DataSource.busInTravelList
                   where busInTravel.MyActivity == Activity.ON
                   select busInTravel.Clone();
        }

        public IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate)
        {
            IEnumerable<BusInTravel> myBusInTravel = from busInTravel in DataSource.busInTravelList
                                                     where busInTravel.MyActivity == Activity.ON && predicate(busInTravel)
                                                     select busInTravel.Clone();
            if (myBusInTravel != null)
                return myBusInTravel;
            throw new ReadDataException("No BusInTravel meets the conditions");
        }

        public BusInTravel GetBusInTravel(int license, int line, DateTime departureTime)
        {
            BusInTravel myBusInTravel = DataSource.busInTravelList.FirstOrDefault(
                busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.DepartureTime == departureTime && busInTravel.MyActivity == Activity.ON);
            if (myBusInTravel != null)
                return myBusInTravel.Clone();
            throw new BadBusInTravelException("Bus In Travel doesn't exist", license, line, departureTime);
        }

        public void AddBusInTravel(BusInTravel tmpBusInTravel)
        {
            if (DataSource.busInTravelList.FirstOrDefault(
                busInTravel => busInTravel.License == tmpBusInTravel.License && busInTravel.Line == tmpBusInTravel.Line && busInTravel.DepartureTime == tmpBusInTravel.DepartureTime) != null)
                DataSource.busInTravelList.Add(tmpBusInTravel.Clone());
            throw new BadBusInTravelException("Bus In Travel already exist", tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
        }

        public void DeleteBusInTravel(int license, int line, DateTime departureTime)
        {
            BusInTravel tmpBusInTravel = DataSource.busInTravelList.FirstOrDefault(busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.MyActivity == Activity.ON);
            if (tmpBusInTravel != null)
                tmpBusInTravel.MyActivity = Activity.OFF;
            throw new BadBusInTravelException("Bus In Travel doesn't exist", tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
        }

        public void UpdateBusInTravel(int license, int line, DateTime departureTime, Action<BusInTravel> update)
        {
            BusInTravel tmpBusInTravel = DataSource.busInTravelList.FirstOrDefault(busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.MyActivity == Activity.ON);
            if (tmpBusInTravel != null)
                update(tmpBusInTravel);
            throw new BadBusInTravelException("Bus In Travel doesn't exist", tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
        }
        #endregion

        #region LineDeparting
        public IEnumerable<LineDeparting> GetAllLineDeparting()
        {
            return from lineDeparting in DataSource.lineDepartingList
                   where lineDeparting.MyActivity == Activity.ON
                   select lineDeparting.Clone();
        }

        public IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate)
        {
            IEnumerable<LineDeparting> mylineDeparting = from lineDeparting in DataSource.lineDepartingList
                                                         where lineDeparting.MyActivity == Activity.ON && predicate(lineDeparting)
                                                         select lineDeparting.Clone();
            if (mylineDeparting != null)
                return mylineDeparting;
            throw new ReadDataException("No LineDeparting meets the conditions");
        }

        public LineDeparting GetLineDeparting(int lineNumber, DateTime startTime)
        {
            LineDeparting line = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineNumber && lineDeparting.StartTime == startTime && lineDeparting.MyActivity == Activity.ON);
            if (line != null)
                return line.Clone();
            throw new BadLineDepartingException("Linedeparting doesn't exist ", lineNumber, startTime);
        }

        public void AddLineDeparting(LineDeparting tmpLineDeparting)
        {
            LineDeparting line = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == tmpLineDeparting.LineNumber && lineDeparting.StartTime == tmpLineDeparting.StartTime && lineDeparting.MyActivity == Activity.ON);
            if (line == null)
                DataSource.lineDepartingList.Add(tmpLineDeparting.Clone());
            throw new BadLineDepartingException("Line Departing already exists" , tmpLineDeparting.LineNumber, tmpLineDeparting.StartTime);
        }

        public void DeleteLineDeparting(int lineNumber, DateTime startTime)
        {
            LineDeparting line = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineNumber && lineDeparting.StartTime == startTime && lineDeparting.MyActivity == Activity.ON);
            if (line != null)
                line.MyActivity = Activity.OFF;
            throw new BadLineDepartingException("LineDeparture doesn't exist" , lineNumber, startTime);
        }

        public void UpdateLineDeparting(int lineNumber, DateTime startTime, Action<LineDeparting> update)
        {
            LineDeparting line = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineNumber && lineDeparting.StartTime == startTime && lineDeparting.MyActivity == Activity.ON);
            if (line != null)
                update(line);
            throw new BadLineDepartingException("LineDeparture doesn't exist", lineNumber, startTime);
        }
        #endregion

        #region PairStation
        public IEnumerable<PairStations> GetAllPairStations()
        {
            return from pairstation in DataSource.PairStationList
                   where pairstation.MyActivity == Activity.ON
                   select pairstation.Clone();
        }

        public IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate)
        {
            IEnumerable<PairStations> pairStations = from pairstation in DataSource.PairStationList
                                                     where pairstation.MyActivity == Activity.ON && predicate(pairstation)
                                                     select pairstation.Clone();
            if (pairStations != null)
                return pairStations;
            throw new ReadDataException("No Pair station meets the conditions");
        }

        public PairStations GetPairStations(int firstStation, int secondStation)
        {
            PairStations pair = DataSource.PairStationList.FirstOrDefault
                (pairStations => pairStations.FirstStationNumber == firstStation && pairStations.LastStationNumber == secondStation && pairStations.MyActivity == Activity.ON);
            if (pair != null)
                return pair.Clone();
            throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
        }

        public void AddPairStations(PairStations tmpPairStations)
        {
            PairStations pair = DataSource.PairStationList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == tmpPairStations.FirstStationNumber && pairStations.LastStationNumber == tmpPairStations.LastStationNumber && pairStations.MyActivity == Activity.ON);
            if (pair == null)
                DataSource.PairStationList.Add(tmpPairStations.Clone());
            throw new BadPairStationException("Pair Station already exist", tmpPairStations.FirstStationNumber, tmpPairStations.LastStationNumber);
        }

        public void DeletePairStations(int firstStation, int secondStation)
        {
            PairStations pair = DataSource.PairStationList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == firstStation && pairStations.LastStationNumber == secondStation && pairStations.MyActivity == Activity.ON);
            if (pair != null)
                pair.MyActivity = Activity.OFF;
            throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
        }

        public void UpdatePairStations(int firstStation, int secondStation, Action<PairStations> update)
        {
            PairStations pair = DataSource.PairStationList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == firstStation && pairStations.LastStationNumber == secondStation && pairStations.MyActivity == Activity.ON);
            if (pair != null)
                update(pair);
            throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
        }

        #endregion

        #region UserTrip
        public IEnumerable<UserTrip> GetAllUserTrip()
        {
            return from userTrip in DataSource.userTripList
                   where userTrip.MyActivity == Activity.ON
                   select userTrip.Clone();
        }

        public IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate)
        {
            IEnumerable<UserTrip> myUserTrip = from UserTrip in DataSource.userTripList
                                               where UserTrip.MyActivity == Activity.ON && predicate(UserTrip)
                                               select UserTrip.Clone();
            if (myUserTrip != null)
                return myUserTrip;
            throw new ReadDataException("No UserTrip meets the conditions");
        }

        public UserTrip GetUserTrip(string name)
        {
            UserTrip myUserTrip = DataSource.userTripList.FirstOrDefault(
                userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.ON);
            if (myUserTrip != null)
                return myUserTrip.Clone();
            throw new BadUserTripException("User Trip doesn't exist", name);
        }

        public void AddUserTrip(UserTrip tmpUserTrip)
        {
            if (DataSource.userTripList.FirstOrDefault(
                            userTrip => userTrip.UserName == tmpUserTrip.UserName && userTrip.MyActivity == Activity.ON) != null)
                DataSource.userTripList.Add(tmpUserTrip.Clone());
            throw new BadUserTripException("User Trip already exist", tmpUserTrip.UserName);
        }

        public void DeleteUserTrip(string name)
        {
            UserTrip tmpUserTrip = DataSource.userTripList.FirstOrDefault(userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.ON);
            if (tmpUserTrip != null)
                tmpUserTrip.MyActivity = Activity.OFF;
            throw new BadUserTripException("User Trip doesn't exist", tmpUserTrip.UserName);
        }

        public void UpdateUserTrip(string name, Action<UserTrip> update)
        {
            UserTrip tmpUserTrip = DataSource.userTripList.FirstOrDefault(userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.ON);
            if (tmpUserTrip != null)
                update(tmpUserTrip);
            throw new BadUserTripException("User Trip doesn't exist", tmpUserTrip.UserName);
        }
        #endregion
    }
}
