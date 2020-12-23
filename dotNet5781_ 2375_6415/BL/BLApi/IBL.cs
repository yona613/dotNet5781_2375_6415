using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BLApi
{
    public interface IBL
    {

        #region Bus
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate);
        Bus GetBus(int license);
        void AddBus(Bus myBus);
        void UpdateBus(Bus busToUpdate);
        void DeleteBus(int license);
        #endregion

        #region BusLine
        IEnumerable<BusLine> GetAllBusLines();
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);
        BusLine GetBusLine(int id);
        void AddLine(BusLine tmpBusLine);
        void UpdateLine(BusLine lineToUpdate);
        void DeleteLine(int id);
        #endregion

        #region Station
        IEnumerable<Station> GetAllStations();
        IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate);
        Station GetStation(int id);
        void AddStation(Station tmpStation);
        void DeleteStation(int id);
        #endregion

        #region User
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        User GetUser(string userName);
        void AddUser(User tmpUser);
        void DeleteUser(string name);
        void UpdateUser(User userToUpdate);
        #endregion

        #region LineStation
        IEnumerable<LineStation> GetAllLineStations();
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate);
        LineStation GetLineStation(int stationNumber, int lineNumber);
        void AddLineStation(LineStation tmpLineStation);
        void DeleteLineStation(int stationNumber, int lineNumber);
        void UpdateLineStation(LineStation lineStationToUpdate);
        #endregion

        #region BusInTravel
        IEnumerable<BusInTravel> GetAllBusInTravel();
        IEnumerable<BusInTravel> GetAllBusInTravelBy(Predicate<BusInTravel> predicate);
        BusInTravel GetBusInTravel(int license, int lineNumber, DateTime departureTime);
        void AddBusInTravel(BusInTravel tmpBusInTravel);
        void DeleteBusInTravel(int license, int lineNumber, DateTime departureTime);
        void UpdateBusInTravel(BusInTravel busInTravelToUpdate);
        #endregion

        #region LineDeparting
        IEnumerable<LineDeparting> GetAllLineDeparting();
        IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate);
        LineDeparting GetLineDeparting(int lineNumber, DateTime startTime);
        void AddLineDeparting(LineDeparting tmpLineDeparting);
        void DeleteLineDeparting(int lineNumber, DateTime startTime);
        void UpdateLineDeparting(LineDeparting lineDepartingToUpdate);
        #endregion

        #region PairStations
        IEnumerable<PairStations> GetAllPairStations();
        IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate);
        PairStations GetPairStations(int firstStation, int secondStation);
        void AddPairStations(PairStations tmpPairStations);
        void DeletePairStations(int firstStation, int secondStation);
        void UpdatePairStations(PairStations pairStationsToUpdate);
        #endregion

        #region UserTrip
        IEnumerable<UserTrip> GetAllUserTrip();
        IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate);
        UserTrip GetUserTrip(string name);
        void AddUserTrip(UserTrip tmpUserTrip);
        void DeleteUserTrip(string name);
        void UpdateUserTrip(UserTrip userTripToUpdate);
        #endregion

    }
}
