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
            DataSource.lineList.Add(tmpBusLine.Clone());
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
            return from busInTravel in DataSource.BusInTravelList
                   select busInTravel.Clone();
        }

        public IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate)
        {
            IEnumerable<BusInTravel> myBusInTravel = from busInTravel in DataSource.busInTravelList
                                                     where predicate(busInTravel)
                                                      select busInTravel.Clone();
            if (myBusInTravel != null)
                return myBusInTravel;
            throw new ReadDataException("No BusInTravel meets the conditions");
        }

        public BusInTravel GetBusInTravel(int license, int line, DateTime departureTime)
        {
            BusInTravel myBusInTravel = DataSource.busInTravelList.FirstOrDefault(
                busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.DepartureTime == departureTime);
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

        public void DeleteBusInTravel(int license, int lineNumber, DateTime departureTime)
        {
            BusInTravel tmpBusInTravel = DataSource.busInTravelList.FirstOrDefault(busInTravel => station.LineNumber == lineNumber && station.StationNumber == stationNumber);
            if (tmpLineStation != null)
                DataSource.linestationList.Remove(tmpLineStation);
            throw new BadLineStationException("Line Station doesn't exist", lineNumber, stationNumber);
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
        public void UpdateBusInTravel(int license, int lineNumber, DateTime departureTime, Action<BusInTravel> update)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
