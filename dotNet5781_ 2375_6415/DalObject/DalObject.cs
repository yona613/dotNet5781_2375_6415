using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using DS;


namespace DL
{
    sealed class DalObject : IDL
    {
        #region singelton
        /// <summary>
        /// new static read-only dalxml
        /// </summary>
        private static readonly DalObject instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        private DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
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
            IEnumerable<Bus> allBuses = from Bus in DataSource.busList
                                        where Bus.MyActivity == Activity.On
                                        where predicate(Bus)
                                        select Bus.Clone();
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
            return from Bus in DataSource.busList
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
            Bus tmpBus = DataSource.busList.Find(bus => bus.License == license && bus.MyActivity == Activity.On);
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
            if (DataSource.busList.FirstOrDefault(bus => bus.License == myBus.License && bus.MyActivity == Activity.On) != null)
                throw new BadBusException("Bus already exist", myBus.License);
            DataSource.busList.Add(myBus.Clone());
        }
        /// <summary>
        /// update bus by deleting it and adding it with the news updates
        /// throw BadBusException
        /// </summary>
        /// <param name="busToUpdate">bus To Update</param>
        public void UpdateBus(Bus busToUpdate)
        {
            Bus tmpBus = DataSource.busList.FirstOrDefault(bus => bus.License == busToUpdate.License && bus.MyActivity == Activity.On);
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
            Bus tmpBus = DataSource.busList.FirstOrDefault(bus => bus.License == license && bus.MyActivity == Activity.On);
            if (tmpBus == null)
                throw new BadBusException("Bus doesn't exist", license);
            tmpBus.MyActivity = Activity.Off;
        }
        #endregion

        #region Line
        /// <summary>
        /// Get All Lines
        /// </summary>
        /// <returns>IEnumerable implemented by lines</returns>
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in DataSource.lineList
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
            IEnumerable<BusLine> myLineList = from busLine in DataSource.lineList
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
            BusLine myBusLine = DataSource.lineList.Find(line => line.LineNumber == id && line.MyActivity == Activity.On);
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
            if (DataSource.lineList.FirstOrDefault(Line => Line.LineNumber == tmpBusLine.LineNumber && Line.MyActivity == Activity.On) != null)
                throw new BadLineException("the Line already exist", tmpBusLine.LineNumber);
            BusLine myBusLine = tmpBusLine.Clone();
            myBusLine.Key = Config.BusLineCounter;
            DataSource.lineList.Add(myBusLine);
        }
        /// <summary>
        /// update line by deleting it and adding it with the news updates
        /// throw BadLineException
        /// </summary>
        /// <param name="lineToUpdate">line To Update</param>
        public void UpdateLine(BusLine lineToUpdate)
        {
            BusLine tmpLine = DataSource.lineList.FirstOrDefault(line => line.LineNumber == lineToUpdate.LineNumber && line.MyActivity == Activity.On);
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
            BusLine tmpLine = DataSource.lineList.FirstOrDefault(line => line.LineNumber == lineNumber && line.MyActivity == Activity.On);
            if (tmpLine == null)
                throw new BadLineException("the Line doesn't exist", lineNumber);
            tmpLine.MyActivity = Activity.Off;
        }
        #endregion

        #region Station
        /// <summary>
        /// get all stations
        /// </summary>
        /// <returns>IEnumerable implemented by stations</returns>
        public IEnumerable<Station> GetAllStations()
        {
            return from station in DataSource.stationList
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
            IEnumerable<Station> myStationsList = from station in DataSource.stationList
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
            Station myStation = DataSource.stationList.Find(Station => Station.StationId == id && Station.MyActivity == Activity.On);
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
            if (DataSource.stationList.FirstOrDefault(station =>station.StationId == tmpStation.StationId && station.MyActivity == Activity.On) != null)
                throw new BadStationException("Station already exist", tmpStation.StationId);
            DataSource.stationList.Add(tmpStation.Clone());
        }
        /// <summary>
        /// delete station by turns its activity to off
        /// throw BadStationException
        /// </summary>
        /// <param name="id">number of the station going to been deleted</param>
        public void DeleteStation(int id)
        {
            Station myStation = DataSource.stationList.FirstOrDefault(station => station.StationId == id && station.MyActivity == Activity.On);
            if (myStation == null)
                throw new BadStationException("Station doesn't exist", id);
            myStation.MyActivity = Activity.Off;
        }
        #endregion

        #region User
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns>IEnumerable implemented by users</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return from user in DataSource.userList
                   where user.MyActivity == Activity.On
                   select user.Clone();
        }
        /// <summary>
        /// get all users that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by users satisfies the cindition</returns>
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            IEnumerable<User> myUsers = from user in DataSource.userList
                                        where user.MyActivity == Activity.On
                                        where predicate(user)
                                        select user.Clone();
            if(myUsers == null)
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
            User myUser = DataSource.userList.FirstOrDefault(user => user.UserName == userName && user.MyActivity == Activity.On);
            if (myUser != null)
                return myUser.Clone();
            throw new BadUserException("User doesn't exist", userName);
        }
        /// <summary>
        /// update user (delete the old and add the new)
        /// throw BadUserException
        /// </summary>
        /// <param name="userToUpdate">user To Update</param>
        public void UpdateUser(User userToUpdate)
        {
            User tmpUser = DataSource.userList.FirstOrDefault(user => user.UserName == userToUpdate.UserName && user.MyActivity == Activity.On);
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
            if (DataSource.userList.FirstOrDefault(user => user.UserName == tmpUser.UserName && user.MyActivity == Activity.On) != null)
                throw new BadUserException("User already exist", tmpUser.UserName);
            DataSource.userList.Add(tmpUser.Clone());
        }
        /// <summary>
        /// delete user
        /// throw BadUserException
        /// </summary>
        /// <param name="userName">name of user to delete</param>
        public void DeleteUser(string userName)
        {
            User myUser = DataSource.userList.FirstOrDefault(user => user.UserName == userName && user.MyActivity == Activity.On);
            if (myUser != null)
                myUser.MyActivity = Activity.Off;
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
            return from lineStation in DataSource.linestationList
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
            IEnumerable<LineStation> myLineStations = from lineStation in DataSource.linestationList
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
            LineStation myLineStation = DataSource.linestationList.FirstOrDefault(
                station =>station.LineNumber == lineNumber && station.StationNumber == stationNumber && station.MyActivity==Activity.On);
            if (myLineStation != null)
                return myLineStation.Clone();
            throw new BadLineStationException("Line Station doesn't exist", lineNumber,stationNumber);
        }
        /// <summary>
        /// Add Line Station
        /// throw BadLineStationException
        /// </summary>
        /// <param name="tmpLineStation">lineStation to add</param>
        public void AddLineStation(LineStation tmpLineStation)
        { LineStation tmp = DataSource.linestationList.FirstOrDefault(station => station.LineNumber == tmpLineStation.LineNumber && station.StationNumber == tmpLineStation.StationNumber && station.MyActivity == Activity.On);
            if ( tmp == null)
                DataSource.linestationList.Add(tmpLineStation.Clone());
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
            LineStation tmpLineStation = DataSource.linestationList.FirstOrDefault(station => station.LineNumber == lineNumber && station.StationNumber == stationNumber && station.MyActivity == Activity.On) ;
            if (tmpLineStation != null)
                tmpLineStation.MyActivity = Activity.Off;
            else throw new BadLineStationException("Line Station doesn't exist", lineNumber, stationNumber);
        }
        /// <summary>
        /// Update Line Station (delete old and add new)
        /// throw BadLineStationException
        /// </summary>
        /// <param name="lineStationToUpdate"></param>
        public void UpdateLineStation(LineStation lineStationToUpdate)
        {
            LineStation tmpLineStation = DataSource.linestationList.FirstOrDefault(station => station.LineNumber == lineStationToUpdate.LineNumber && station.StationNumber == lineStationToUpdate.StationNumber && station.MyActivity==Activity.On);
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
            return from busInTravel in DataSource.busInTravelList
                   where busInTravel.MyActivity == Activity.On
                   select busInTravel.Clone();
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate)
        {
            IEnumerable<BusInTravel> myBusInTravel = from busInTravel in DataSource.busInTravelList
                                                     where busInTravel.MyActivity == Activity.On && predicate(busInTravel)
                                                     select busInTravel.Clone();
            if (myBusInTravel != null)
                return myBusInTravel;
            throw new ReadDataException("No BusInTravel meets the conditions");
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public BusInTravel GetBusInTravel(int license, int line, DateTime departureTime)
        {
            BusInTravel myBusInTravel = DataSource.busInTravelList.FirstOrDefault(
                busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.DepartureTime == departureTime && busInTravel.MyActivity == Activity.On);
            if (myBusInTravel != null)
                return myBusInTravel.Clone();
            throw new BadBusInTravelException("Bus In Travel doesn't exist", license, line, departureTime);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void AddBusInTravel(BusInTravel tmpBusInTravel)
        {
            if (DataSource.busInTravelList.FirstOrDefault(
                busInTravel => busInTravel.License == tmpBusInTravel.License && busInTravel.Line == tmpBusInTravel.Line && busInTravel.DepartureTime == tmpBusInTravel.DepartureTime) != null)
            {
                BusInTravel myBusInTravel = tmpBusInTravel.Clone();
                myBusInTravel.Key = Config.BusInTravelCounter;
                DataSource.busInTravelList.Add(myBusInTravel);
            }        
            throw new BadBusInTravelException("Bus In Travel already exist", tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void DeleteBusInTravel(int license, int line, DateTime departureTime)
        {
            BusInTravel tmpBusInTravel = DataSource.busInTravelList.FirstOrDefault(busInTravel => busInTravel.License == license && busInTravel.Line == line && busInTravel.MyActivity == Activity.On);
            if (tmpBusInTravel != null)
                tmpBusInTravel.MyActivity = Activity.Off;
            throw new BadBusInTravelException("Bus In Travel doesn't exist", tmpBusInTravel.License, tmpBusInTravel.Line, tmpBusInTravel.DepartureTime);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void UpdateBusInTravel(BusInTravel busInTravelToUpdate)
        {
            BusInTravel tmpBusInTravel = DataSource.busInTravelList.FirstOrDefault(busInTravel => busInTravel.License == busInTravelToUpdate.License && busInTravel.Line == busInTravelToUpdate.Line && busInTravel.MyActivity == Activity.On);
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
            
            return from lineDeparting in DataSource.lineDepartingList
                   where lineDeparting.MyActivity == Activity.On
                   select lineDeparting.Clone();
        }
        /// <summary>
        /// Get All Line Departing meets the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by lineDeparting</returns>
        public IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate)
        {
            IEnumerable<LineDeparting> mylineDeparting = from lineDeparting in DataSource.lineDepartingList
                                                         where lineDeparting.MyActivity == Activity.On && predicate(lineDeparting)
                                                         select lineDeparting.Clone();
            if (mylineDeparting != null)
                return mylineDeparting;
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
            LineDeparting line = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineNumber && lineDeparting.StartTime == startTime && lineDeparting.MyActivity == Activity.On);
            if (line != null)
                return line.Clone();
            throw new BadLineDepartingException("Linedeparting doesn't exist ", lineNumber, startTime);
        }
        /// <summary>
        /// Add Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="tmpLineDeparting">LineDeparting to add</param>
        public void AddLineDeparting(LineDeparting tmpLineDeparting)
        {
            LineDeparting line = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == tmpLineDeparting.LineNumber && lineDeparting.StartTime == tmpLineDeparting.StartTime && lineDeparting.MyActivity == Activity.On);
            if (line != null)
                throw new BadLineDepartingException("Line Departing already exists", tmpLineDeparting.LineNumber, tmpLineDeparting.StartTime);
            DataSource.lineDepartingList.Add(tmpLineDeparting.Clone());           
        }
        /// <summary>
        /// Delete Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineNumber">Unique entity ID</param>
        /// <param name="startTime">Unique entity ID</param>
        public void DeleteLineDeparting(int lineNumber, TimeSpan startTime)
        {
            LineDeparting line = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineNumber && lineDeparting.StartTime == startTime && lineDeparting.MyActivity == Activity.On);
            if (line == null)
                throw new BadLineDepartingException("LineDeparture doesn't exist", lineNumber, startTime);
            line.MyActivity = Activity.Off;
            
        }
        /// <summary>
        /// Update Line Departing (delete old and add new)
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineDepartingToUpdate"></param>
        public void UpdateLineDeparting(LineDeparting lineDepartingToUpdate)
        {
            LineDeparting tmpline = DataSource.lineDepartingList.FirstOrDefault
                (lineDeparting => lineDeparting.LineNumber == lineDepartingToUpdate.LineNumber && lineDeparting.StartTime == lineDepartingToUpdate.StartTime && lineDeparting.MyActivity == Activity.On);
            if (tmpline == null)
                throw new BadLineDepartingException("LineDeparture doesn't exist", lineDepartingToUpdate.LineNumber, lineDepartingToUpdate.StartTime);
            DeleteLineDeparting(tmpline.LineNumber, tmpline.StartTime);
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
            return from pairstation in DataSource.PairStationList
                   where pairstation.MyActivity == Activity.On
                   select pairstation.Clone();
        }
        /// <summary>
        /// Get All PairStations meets the codition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the codition (bool)</param>
        /// <returns>IEnumerable implemented by pairStations</returns>
        public IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate)
        {
            IEnumerable<PairStations> pairStations = from pairstation in DataSource.PairStationList
                                                     where pairstation.MyActivity == Activity.On && predicate(pairstation)
                                                     select pairstation.Clone();
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
            PairStations pair = DataSource.PairStationList.FirstOrDefault
                (pairStations => pairStations.FirstStationNumber == firstStation && pairStations.LastStationNumber == secondStation && pairStations.MyActivity == Activity.On);
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
            PairStations pair = DataSource.PairStationList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == tmpPairStations.FirstStationNumber && pairStations.LastStationNumber == tmpPairStations.LastStationNumber && pairStations.MyActivity == Activity.On);
            if (pair != null)
                throw new BadPairStationException("Pair Station already exist", tmpPairStations.FirstStationNumber, tmpPairStations.LastStationNumber);
            DataSource.PairStationList.Add(tmpPairStations.Clone());
           
        }
        /// <summary>
        /// Delete PairStations
        /// throw BadPairStationException
        /// </summary>
        /// <param name="firstStation">Unique entity ID</param>
        /// <param name="secondStation">Unique entity ID</param>
        public void DeletePairStations(int firstStation, int secondStation)
        {
            PairStations pair = DataSource.PairStationList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == firstStation && pairStations.LastStationNumber == secondStation && pairStations.MyActivity == Activity.On);
            if (pair == null)
                throw new BadPairStationException("Pair Station doesn't exist", firstStation, secondStation);
            pair.MyActivity = Activity.Off;
            
        }
        /// <summary>
        /// Update PairStations (delete old and add new)
        /// throw BadPairStationException
        /// </summary>
        /// <param name="pairStationsToUpdate"></param>
        public void UpdatePairStations(PairStations pairStationsToUpdate)
        {
            PairStations tmpPair = DataSource.PairStationList.FirstOrDefault
               (pairStations => pairStations.FirstStationNumber == pairStationsToUpdate.FirstStationNumber && pairStations.LastStationNumber == pairStationsToUpdate.LastStationNumber && pairStations.MyActivity == Activity.On);
            if (tmpPair == null)
                throw new BadPairStationException("Pair Station doesn't exist", pairStationsToUpdate.FirstStationNumber, pairStationsToUpdate.LastStationNumber);
            DeletePairStations(tmpPair.FirstStationNumber, tmpPair.LastStationNumber);
            AddPairStations(pairStationsToUpdate);
        }
        #endregion

        #region UserTrip
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<UserTrip> GetAllUserTrip()
        {
            return from userTrip in DataSource.userTripList
                   where userTrip.MyActivity == Activity.On
                   select userTrip.Clone();
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate)
        {
            IEnumerable<UserTrip> myUserTrip = from UserTrip in DataSource.userTripList
                                               where UserTrip.MyActivity == Activity.On && predicate(UserTrip)
                                               select UserTrip.Clone();
            if (myUserTrip != null)
                return myUserTrip;
            throw new ReadDataException("No UserTrip meets the conditions");
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public UserTrip GetUserTrip(string name)
        {
            UserTrip myUserTrip = DataSource.userTripList.FirstOrDefault(
                userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.On);
            if (myUserTrip != null)
                return myUserTrip.Clone();
            throw new BadUserTripException("User Trip doesn't exist", name);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void AddUserTrip(UserTrip tmpUserTrip)
        {
            if (DataSource.userTripList.FirstOrDefault(
                            userTrip => userTrip.UserName == tmpUserTrip.UserName && userTrip.MyActivity == Activity.On) != null)
            {
                UserTrip myUserTrip = tmpUserTrip.Clone();
                myUserTrip.Key = Config.UserTripCounter;
                DataSource.userTripList.Add(myUserTrip);
            }
            throw new BadUserTripException("User Trip already exist", tmpUserTrip.UserName);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void DeleteUserTrip(string name)
        {
            UserTrip tmpUserTrip = DataSource.userTripList.FirstOrDefault(userTrip => userTrip.UserName == name && userTrip.MyActivity == Activity.On);
            if (tmpUserTrip != null)
                tmpUserTrip.MyActivity = Activity.Off;
            throw new BadUserTripException("User Trip doesn't exist", tmpUserTrip.UserName);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public void UpdateUserTrip(UserTrip userTripToUpdate)
        {
            UserTrip tmpUserTrip = DataSource.userTripList.FirstOrDefault(userTrip => userTrip.UserName == userTripToUpdate.UserName && userTrip.MyActivity == Activity.On);
            if (tmpUserTrip == null)
                throw new BadUserTripException("User Trip doesn't exist", userTripToUpdate.UserName);
            DeleteUserTrip(tmpUserTrip.UserName);
            AddUserTrip(userTripToUpdate);

        }
        #endregion
    }
}