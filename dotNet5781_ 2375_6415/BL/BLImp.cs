using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BLApi;
using BO;

namespace BL
{

    class BLImp : IBL
    {
        IDL dal = DalFactory.GetDal();

        #region Bus
        Bus BusDoBOAdapter(DO.Bus busDO)
        {
            Bus busBO = new Bus();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }

        public IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate)
        {
            IEnumerable<Bus> busList = from item in dal.GetAllBuses()
                                       let busBO = BusDoBOAdapter(item)
                                       where predicate((Bus)busBO)
                                       select (Bus)busBO;
            if (busList != null)
                return busList;
            throw new BOReadDataException("No Bus meets the conditions");
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from item in dal.GetAllBuses()
                   select BusDoBOAdapter(item);
        }
        public Bus GetBus(int license)
        {
            DO.Bus busDO;
            try
            {
                busDO = dal.GetBus(license);
                return BusDoBOAdapter(busDO);
            }
            catch (DO.BadBusException e)
            {
                throw new BOBadBusException(e.Message, license);
            }
        }

        public void AddBus(Bus myBus)
        {
            DO.Bus busDo = new DO.Bus();
            myBus.CopyPropertiesTo(busDo);
            try
            {
                dal.AddBus(busDo);
            }
            catch (DO.BadBusException e)
            {
                throw new BOBadBusException(e.Message, myBus.License);
            }
        }

        public void UpdateBus(Bus busToUpdate)
        {
            try
            {
                dal.UpdateBus((DO.Bus)busToUpdate.CopyPropertiesToNew(typeof(DO.Bus)));
            }
            catch (DO.BadBusException e)
            {
                throw new BOBadBusException(e.Message, busToUpdate.License);
            }
        }
        public void DeleteBus(int license)
        {
            try
            {
                dal.DeleteBus(license);
            }
            catch (DO.BadBusException e)
            {
                throw new BOBadBusException(e.Message, license);
            }
        }
        #endregion

        #region Line
        BusLine BusLineDoBOAdapter(DO.BusLine lineDO)
        {
            BusLine lineBO = new BusLine();
            lineDO.CopyPropertiesTo(lineBO);
            return lineBO;
        }
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in dal.GetAllBusLines()
                   select BusLineDoBOAdapter(busLine);
        }

        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            IEnumerable<BusLine> myLinesList = from busLine in dal.GetAllBusLines()
                                               let item = BusLineDoBOAdapter(busLine)
                                               where predicate(item)
                                               select item;
            if (myLinesList != null)
                return myLinesList;
            throw new BOReadDataException("No Line meets the conditions");
        }
        public BusLine GetBusLine(int id)
        {
            DO.BusLine busLineDO;
            try
            {
                busLineDO = dal.GetBusLine(id);
                return BusLineDoBOAdapter(busLineDO);
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, id);
            }
        }

        public void AddLine(BusLine tmpBusLine)
        {
            try
            {
                dal.AddLine((DO.BusLine)tmpBusLine.CopyPropertiesToNew(typeof(DO.BusLine)));
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, tmpBusLine.LineNumber);
            }
        }
        public void UpdateLine(BusLine lineToUpdate)
        {
            try
            {
                dal.UpdateLine((DO.BusLine)lineToUpdate.CopyPropertiesToNew(typeof(DO.BusLine)));
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, lineToUpdate.LineNumber);
            }
        }

        public void DeleteLine(int lineNumber)
        {
            try
            {
                dal.DeleteLine(lineNumber);
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, lineNumber);
            }
        }
        #endregion

        #region Station
        Station StationDoBOAdapter(DO.Station stationDO)
        {
            Station stationBO = new Station();
            stationDO.CopyPropertiesTo(stationBO);
            return stationBO;
        }
        public IEnumerable<Station> GetAllStations()
        {
            return from station in dal.GetAllStations()
                   select StationDoBOAdapter(station);
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            IEnumerable<Station> myStationsList = from station in dal.GetAllStations()
                                                  let item = StationDoBOAdapter(station)
                                                  where predicate(item)
                                                  select item;
            if (myStationsList == null)
                throw new BOReadDataException("No Station meets the conditions");
            return myStationsList;
        }
        public Station GetStation(int id)
        {
            DO.Station stationDO;
            try
            {
                stationDO = dal.GetStation(id);
                return StationDoBOAdapter(stationDO);
            }
            catch (DO.BadStationException e)
            {
                throw new BOBadStationException(e.Message, id);
            }
        }

        public void AddStation(Station tmpStation)
        {
            DO.Station stationDO = new DO.Station();
            tmpStation.CopyPropertiesTo(stationDO);
            try
            {
                dal.AddStation(stationDO);
            }
            catch (DO.BadStationException e)
            {
                throw new BOBadStationException(e.Message, tmpStation.StationId);
            }
        }
        public void DeleteStation(int id)
        {
            try
            {
                dal.DeleteStation(id);
            }
            catch (DO.BadStationException e)
            {
                throw new BOBadStationException(e.Message, id);
            }
        }
        #endregion

        #region User
        User UserDoBOAdapter(DO.User userDO)
        {
            User userBO = new User();
            userDO.CopyPropertiesTo(userBO);
            return userBO;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return from user in dal.GetAllUsers()
                   select UserDoBOAdapter(user);
        }

        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            IEnumerable<User> myUsers = from user in dal.GetAllUsers()
                                        let userBO = UserDoBOAdapter(user)
                                        where predicate(userBO)
                                        select userBO;
            if (myUsers == null)
                throw new BOReadDataException("No User meets the conditions");
            return myUsers;
        }
        public User GetUser(string userName)
        {
            DO.User userDO;
            try
            {
                userDO = dal.GetUser(userName);
                return UserDoBOAdapter(userDO);
            }
            catch (DO.BadUserException e)
            {
                throw new BOBadUserException(e.Message, userName);
            }
        }

        public void UpdateUser(User userToUpdate)
        {
            DO.User userDO = new DO.User();
            try
            {
                dal.UpdateUser((DO.User)userToUpdate.CopyPropertiesToNew(typeof(DO.User)));
            }
            catch (DO.BadUserException e)
            {
                throw new BOBadUserException(e.Message, userToUpdate.UserName);
            }
        }

        public void AddUser(User tmpUser)
        {
            DO.User userDO = new DO.User();
            try
            {
                dal.AddUser((DO.User)tmpUser.CopyPropertiesToNew(typeof(DO.User)));
            }
            catch (DO.BadUserException e)
            {
                throw new BOBadUserException(e.Message, tmpUser.UserName);
            }
        }
        public void DeleteUser(string userName)
        {
            try
            {
                dal.DeleteUser(userName);
            }
            catch (DO.BadUserException e)
            {
                throw new BOBadUserException(e.Message, userName);
            }
        }
        #endregion

        #region LineStation
        LineStation LineStationDoBOAdapter(DO.LineStation lineStationDO)
        {
            LineStation lineStationBO = new LineStation();
            lineStationDO.CopyPropertiesTo(lineStationBO);
            return lineStationBO;
        }
        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from lineStation in dal.GetAllLineStations()
                   select LineStationDoBOAdapter(lineStation);
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            IEnumerable<LineStation> myLineStations = from lineStation in dal.GetAllLineStations()
                                                      let lineStationBO = LineStationDoBOAdapter(lineStation)
                                                      where predicate(lineStationBO)
                                                      select lineStationBO;
            if (myLineStations != null)
                return myLineStations;
            throw new BOReadDataException("No LineStation meets the conditions");
        }

        public LineStation GetLineStation(int stationNumber, int lineNumber)
        {
            DO.LineStation lineStationDO;
            try
            {
                lineStationDO = dal.GetLineStation(stationNumber, lineNumber);
                return LineStationDoBOAdapter(lineStationDO);
            }
            catch (DO.BadLineStationException e)
            {
                throw new BOBadLineStationException(e.Message, lineNumber, stationNumber);
            }
        }

        public void AddLineStation(LineStation tmpLineStation)
        {
            DO.LineStation lineStationDO = new DO.LineStation();
            try
            {
                dal.AddLineStation((DO.LineStation)tmpLineStation.CopyPropertiesToNew(typeof(DO.LineStation)));
            }
            catch (DO.BadLineStationException e)
            {
                throw new BOBadLineStationException(e.Message, tmpLineStation.LineNumber, tmpLineStation.StationNumber);
            }
        }

        public void DeleteLineStation(int stationNumber, int lineNumber)
        {          
            try
            {
                dal.DeleteLineStation(stationNumber, lineNumber);
            }
            catch (DO.BadLineStationException e)
            {
                throw new BOBadLineStationException(e.Message, lineNumber, stationNumber);
            }
        }

        public void UpdateLineStation(LineStation lineStationToUpdate)
        {
            DO.LineStation lineStationDO = new DO.LineStation();
            try
            {
                dal.UpdateLineStation((DO.LineStation)lineStationToUpdate.CopyPropertiesToNew(typeof(DO.LineStation)));
            }
            catch (DO.BadLineStationException e)
            {
                throw new BOBadLineStationException(e.Message, lineStationToUpdate.LineNumber, lineStationToUpdate.StationNumber);
            }
        }

        #endregion

        #region BusInTravel
        BusInTravel BusInTravelDOBOAdapter(DO.BusInTravel busInTravelDO)
        {
            BusInTravel busInTravelBO = new BusInTravel();
            busInTravelDO.CopyPropertiesTo(busInTravelBO);
            return busInTravelBO;
        }
        public IEnumerable<BusInTravel> GetAllBusInTravel()
        {
            return from busInTravel in dal.GetAllBusInTravel()
                   select BusInTravelDOBOAdapter(busInTravel);
        }

        public IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate)
        {
            IEnumerable<BusInTravel> myBusInTravel = from busInTravel in dal.GetAllBusInTravel()
                                                     let busInTravelBO = BusInTravelDOBOAdapter(busInTravel)
                                                     where predicate(busInTravelBO)
                                                     select busInTravelBO;
            if (myBusInTravel != null)
                return myBusInTravel;
            throw new BOReadDataException("No BusInTravel meets the conditions");
        }

        public BusInTravel GetBusInTravel(int license, int line, DateTime departureTime)
        {
            DO.BusInTravel busInTravelDO;
            try
            {
                busInTravelDO = dal.GetBusInTravel(license, line,departureTime);
                return BusInTravelDOBOAdapter(busInTravelDO);
            }
            catch (DO.BadBusInTravelException e)
            {
                throw new BOBadBusInTravelException(e.Message, line, license, departureTime);
            }
        }

        public void AddBusInTravel(BusInTravel tmpBusInTravel)
        {
            DO.BusInTravel busInTravelDO = new DO.BusInTravel();
            try
            {
                dal.AddBusInTravel((DO.BusInTravel)tmpBusInTravel.CopyPropertiesToNew(typeof(DO.BusInTravel)));
            }
            catch (DO.BadBusInTravelException e)
            {
                throw new BOBadBusInTravelException(e.Message, tmpBusInTravel.Line, tmpBusInTravel.License, tmpBusInTravel.DepartureTime);
            }
        }

        public void DeleteBusInTravel(int license, int line, DateTime departureTime)
        {
            try
            {
                dal.DeleteBusInTravel(license, line, departureTime);
            }
            catch (DO.BadBusInTravelException e)
            {
                throw new BOBadBusInTravelException(e.Message, line, license, departureTime);
            }
        }

        public void UpdateBusInTravel(BusInTravel busInTravelToUpdate)
        {
            DO.BusInTravel busInTravelDO = new DO.BusInTravel();
            try
            {
                dal.AddBusInTravel((DO.BusInTravel)busInTravelToUpdate.CopyPropertiesToNew(typeof(DO.BusInTravel)));
            }
            catch (DO.BadBusInTravelException e)
            {
                throw new BOBadBusInTravelException(e.Message, busInTravelToUpdate.Line, busInTravelToUpdate.License, busInTravelToUpdate.DepartureTime);
            }
        }
        #endregion

        #region LineDeparting
        LineDeparting LineDepartingDOBOAdapter(DO.LineDeparting lineDepartingDO)
        {
            LineDeparting lineDepartingBO = new LineDeparting();
            lineDepartingDO.CopyPropertiesTo(lineDepartingBO);
            return lineDepartingBO;
        }

        public IEnumerable<LineDeparting> GetAllLineDeparting()
        {
            return from lineDeparting in dal.GetAllLineDeparting()                 
                   select LineDepartingDOBOAdapter(lineDeparting);
        }

        public IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate)
        {
            IEnumerable<LineDeparting> mylineDeparting = from lineDeparting in dal.GetAllLineDeparting()
                                                         let lineDepartingBO = LineDepartingDOBOAdapter(lineDeparting)
                                                         where predicate(lineDepartingBO)
                                                         select lineDepartingBO;
            if (mylineDeparting != null)
                return mylineDeparting;
            throw new BOReadDataException("No LineDeparting meets the conditions");
        }

        public LineDeparting GetLineDeparting(int lineNumber, DateTime startTime)
        {
            DO.LineDeparting lineDepartingDO;
            try
            {
                lineDepartingDO = dal.GetLineDeparting(lineNumber, startTime);
                return LineDepartingDOBOAdapter(lineDepartingDO);
            }
            catch (DO.BadLineDepartingException e)
            {
                throw new BOBadLineDepartingException(e.Message, lineNumber, startTime);
            }
        }

        public void AddLineDeparting(LineDeparting tmpLineDeparting)
        {
            DO.LineDeparting busInTravelDO = new DO.LineDeparting();
            try
            {
                dal.AddLineDeparting((DO.LineDeparting)tmpLineDeparting.CopyPropertiesToNew(typeof(DO.LineDeparting)));
            }
            catch (DO.BadLineDepartingException e)
            {
                throw new BOBadLineDepartingException(e.Message, tmpLineDeparting.LineNumber, tmpLineDeparting.StartTime);
            }
        }

        public void DeleteLineDeparting(int lineNumber, DateTime startTime)
        {
            try
            {
                dal.DeleteLineDeparting(lineNumber, startTime);
            }
            catch (DO.BadLineDepartingException e)
            {
                throw new BOBadLineDepartingException(e.Message, lineNumber, startTime);
            }
        }

        public void UpdateLineDeparting(LineDeparting lineDepartingToUpdate)
        {
            DO.LineDeparting busInTravelDO = new DO.LineDeparting();
            try
            {
                dal.UpdateLineDeparting((DO.LineDeparting)lineDepartingToUpdate.CopyPropertiesToNew(typeof(DO.LineDeparting)));
            }
            catch (DO.BadLineDepartingException e)
            {
                throw new BOBadLineDepartingException(e.Message, lineDepartingToUpdate.LineNumber, lineDepartingToUpdate.StartTime);
            }
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

        public void UpdatePairStations(PairStations pairStationsToUpdate)
        {
            throw new NotImplementedException();
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
            {
                UserTrip myUserTrip = tmpUserTrip.Clone();
                myUserTrip.Key = Config.UserTripCounter;
                DataSource.userTripList.Add(myUserTrip);
            }
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
        public void UpdateUserTrip(UserTrip userTripToUpdate)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
