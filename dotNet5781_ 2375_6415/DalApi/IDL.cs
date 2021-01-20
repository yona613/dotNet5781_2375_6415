using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDL
    {
        #region Bus
        /// <summary>
        /// get all bus
        /// </summary>
        /// <returns>IEnumerable implemented by buses</returns>
        IEnumerable<Bus> GetAllBuses();
        /// <summary>
        /// get all bus that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by buses satisfies the cindition</returns>
        IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate);
        /// <summary>
        /// get solid bus by condition
        /// throw BadBusException
        /// </summary>
        /// <param name="license"></param>
        /// <returns>the appropriate bus</returns>
        Bus GetBus(int license);
        /// <summary>
        /// adding bus to bus-list
        /// throw BadBusException
        /// </summary>
        /// <param name="myBus">bus to add</param>
        void AddBus(Bus myBus);
        /// <summary>
        /// update bus by deleting it and adding it with the news updates
        /// throw BadBusException
        /// </summary>
        /// <param name="busToUpdate">bus To Update</param>
        void UpdateBus(Bus busToUpdate);
        /// <summary>
        /// delete bus by turns its activity off
        /// throw BadBusException
        /// </summary>
        /// <param name="license"></param>
        void DeleteBus(int license);
        #endregion

        #region BusLine
        /// <summary>
        /// Get All Lines
        /// </summary>
        /// <returns>IEnumerable implemented by lines</returns>
        IEnumerable<BusLine> GetAllBusLines();
        /// <summary>
        /// get all lines that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by lines satisfies the cindition</returns>
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);
        /// <summary>
        /// get solid line by condition
        /// throw BadLineException
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the appropriate line</returns>
        BusLine GetBusLine(int id);
        /// <summary>
        /// adding line to line-list
        /// throw BadLineException
        /// </summary>
        /// <param name="tmpBusLine">line to add</param>
        void AddLine(BusLine tmpBusLine);
        /// <summary>
        /// update line by deleting it and adding it with the news updates
        /// throw BadLineException
        /// </summary>
        /// <param name="lineToUpdate">line To Update</param>
        void UpdateLine(BusLine lineToUpdate);
        /// <summary>
        /// delete line by turns its activity off
        /// throw BadLineException
        /// </summary>
        /// <param name="lineNumber"></param>
        void DeleteLine(int id);
        #endregion

        #region Station
        /// <summary>
        /// get all stations
        /// </summary>
        /// <returns>IEnumerable implemented by stations</returns>
        IEnumerable<Station> GetAllStations();
        /// <summary>
        /// get all stations that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by buses satisfies the cindition</returns>
        IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate);
        /// <summary>
        /// get solid station by condition
        /// throw BadStationException
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the appropriate station</returns>
        Station GetStation(int id);
        /// <summary>
        /// adding station to station-list
        /// throw BadStationException
        /// </summary>
        /// <param name="tmpStation">station to add</param>
        void AddStation(Station tmpStation);
        /// <summary>
        /// delete station by turns its activity to off
        /// throw BadStationException
        /// </summary>
        /// <param name="id">number of the station going to been deleted</param>
        void DeleteStation(int id);
        #endregion

        #region User
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns>IEnumerable implemented by users</returns>
        IEnumerable<User> GetAllUsers();
        /// <summary>
        /// get all users that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by users satisfies the cindition</returns>
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        /// <summary>
        /// get solid user by his name
        /// throw BadUserException
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        User GetUser(string userName);
        /// <summary>
        /// add user
        /// throw BadUserException
        /// </summary>
        /// <param name="tmpUser">user to add</param>
        void AddUser(User tmpUser);
        /// <summary>
        /// delete user
        /// throw BadUserException
        /// </summary>
        /// <param name="userName">name of user to delete</param>
        void DeleteUser(string name);
        /// <summary>
        /// update user (delete the old and add the new)
        /// throw BadUserException
        /// </summary>
        /// <param name="userToUpdate">user To Update</param>
        void UpdateUser(User userToUpdate);
        #endregion

        #region LineStation
        /// <summary>
        /// Get All Line Stations
        /// </summary>
        /// <returns>IEnumerable implements by lineStation</returns>
        IEnumerable<LineStation> GetAllLineStations();
        /// <summary>
        /// Get All Line Stations meets the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the bool condition</param>
        /// <returns></returns>
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate);
        /// <summary>
        /// Get solid Line Station
        /// throw BadLineStationException
        /// </summary>
        /// <param name="stationNumber"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        LineStation GetLineStation(int stationNumber, int lineNumber);
        /// <summary>
        /// Add Line Station
        /// throw BadLineStationException
        /// </summary>
        /// <param name="tmpLineStation">lineStation to add</param>
        void AddLineStation(LineStation tmpLineStation);
        /// <summary>
        /// Delete Line Station
        /// throw BadLineStationException
        /// </summary>
        /// <param name="stationNumber"></param>
        /// <param name="lineNumber"></param>
        void DeleteLineStation(int stationNumber, int lineNumber);
        /// <summary>
        /// Update Line Station (delete old and add new)
        /// throw BadLineStationException
        /// </summary>
        /// <param name="lineStationToUpdate"></param>
        void UpdateLineStation(LineStation lineStationToUpdate);
        #endregion

        #region BusInTravel
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        IEnumerable<BusInTravel> GetAllBusInTravel();
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        BusInTravel GetBusInTravel(int license, int lineNumber, DateTime departureTime);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        void AddBusInTravel(BusInTravel tmpBusInTravel);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        void DeleteBusInTravel(int license, int lineNumber, DateTime departureTime);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        void UpdateBusInTravel(BusInTravel busInTravelToUpdate);
        #endregion

        #region LineDeparting
        /// <summary>
        /// Get All Line Departing
        /// </summary>
        /// <returns>IEnumerable implemented by lineDeparting</returns>
        IEnumerable<LineDeparting> GetAllLineDeparting();
        /// <summary>
        /// Get All Line Departing meets the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by lineDeparting</returns>
        IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate);
        /// <summary>
        /// Get solid Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        LineDeparting GetLineDeparting(int lineNumber, TimeSpan startTime);
        /// <summary>
        /// Add Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="tmpLineDeparting">LineDeparting to add</param>
        void AddLineDeparting(LineDeparting tmpLineDeparting);
        /// <summary>
        /// Delete Line Departing
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineNumber">Unique entity ID</param>
        /// <param name="startTime">Unique entity ID</param>
        void DeleteLineDeparting(int lineNumber, TimeSpan startTime);
        /// <summary>
        /// Update Line Departing (delete old and add new)
        /// throw BadLineDepartingException
        /// </summary>
        /// <param name="lineDepartingToUpdate"></param>
        void UpdateLineDeparting(LineDeparting lineDepartingToUpdate);
        #endregion

        #region PairStations
        /// <summary>
        /// Get All PairStations
        /// </summary>
        /// <returns>IEnumerable implemented by pairStations</returns>
        IEnumerable<PairStations> GetAllPairStations();
        /// <summary>
        /// Get All PairStations meets the codition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the codition (bool)</param>
        /// <returns>IEnumerable implemented by pairStations</returns>
        IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate);
        /// <summary>
        /// Get solid PairStations
        /// throw BadPairStationException
        /// </summary>
        /// <param name="firstStation">Unique entity ID</param>
        /// <param name="secondStation">Unique entity ID</param>
        /// <returns></returns>
        PairStations GetPairStations(int firstStation, int secondStation);
        /// <summary>
        /// Add Pair Stations
        /// throw BadPairStationException
        /// </summary>
        /// <param name="tmpPairStations">PairStations to add</param>
        void AddPairStations(PairStations tmpPairStations);
        /// <summary>
        /// Delete PairStations
        /// throw BadPairStationException
        /// </summary>
        /// <param name="firstStation">Unique entity ID</param>
        /// <param name="secondStation">Unique entity ID</param>
        void DeletePairStations(int firstStation, int secondStation);
        /// <summary>
        /// Update PairStations (delete old and add new)
        /// throw BadPairStationException
        /// </summary>
        /// <param name="pairStationsToUpdate"></param>
        void UpdatePairStations(PairStations pairStationsToUpdate);
        #endregion

        #region UserTrip
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        IEnumerable<UserTrip> GetAllUserTrip();
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        UserTrip GetUserTrip(string name);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        void AddUserTrip(UserTrip tmpUserTrip);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        void DeleteUserTrip(string name);
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        void UpdateUserTrip(UserTrip userTripToUpdate);
        #endregion
    }
}