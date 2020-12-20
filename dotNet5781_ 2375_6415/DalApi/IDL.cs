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
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate);
        Bus GetBus(int license);
        void AddBus(Bus myBus);
        void UpdateBus(int license, Action<Bus> update);
        void DeleteBus(int license);
        #endregion

        #region BusLine
        IEnumerable<BusLine> GetAllBusLines();
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);
        BusLine GetBusLine(int id);
        void AddLine(BusLine tmpBusLine);
        void UpdateLine(int lineNumber, Action<BusLine> update);
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
        void UpdateUser(string name, Action<User> update);
        #endregion

        #region LineStation
        IEnumerable<LineStation> GetAllLineStations();
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate);
        LineStation GetLineStation(int stationNumber, int lineNumber);
        void AddLineStation(LineStation tmpLineStation);
        void DeleteLineStation(int stationNumber, int lineNumber);
        void UpdateLineStation(int stationNumber, int lineNumber, Action<LineStation> update);
        #endregion

        #region BusInTravel

    }
}
