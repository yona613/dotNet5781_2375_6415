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

        /// <summary>
        /// Returns all buses
        /// </summary>
        /// <returns></returns>
        IEnumerable<Bus> GetAllBuses();

        /// <summary>
        /// Returns Ienumerable of all Buses that satisfies the condiction
        /// 
        /// Throws BOReadDataException if no bus
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate);
        
        /// <summary>
        /// Returns the Bus that has this id
        /// 
        /// Throws BOReadBusException if not found
        /// </summary>
        /// <param name="license">Bus License</param>
        /// <returns>Bus</returns>
        Bus GetBus(int license);
       
        /// <summary>
        /// Adds bus to data
        /// Checks that the data of the bus is good else throws :
        /// 
        /// BOArgumentLicenseException
        /// BOArgumentLicenseDateException
        /// BOArgumentTestDateException
        /// </summary>
        /// <param name="myBus"></param>
        void AddBus(Bus myBus);
       
        /// <summary>
        /// Updates bus in data
        /// 
        /// Throws BOBadBusException
        /// </summary>
        /// <param name="busToUpdate">Bus to be Updated</param>
        void UpdateBus(Bus busToUpdate);
       
        /// <summary>
        /// Deletes bus from Data
        /// 
        /// Throws BOBadBusException if not found
        /// </summary>
        /// <param name="license">License of bus to be deleted</param>
        void DeleteBus(int license);
        #endregion

        #region BusLine

        /// <summary>
        /// </summary>
        /// <returns>all lines in data</returns>
        IEnumerable<BusLine> GetAllBusLines();

        /// <summary>
        /// Returns all lines that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>All lines that meet condition</returns>
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);

        /// <summary>
        /// Returns Bus Line with this number
        /// 
        /// Throws BOBadLineException if not found
        /// </summary>
        /// <param name="id">Line Number</param>
        /// <returns>Bus Line</returns>
        BusLine GetBusLine(int id);

        /// <summary>
        /// Returns Bus Line with this number
        /// 
        /// Throws BOBadLineException if not found
        /// </summary>
        /// <param name="id">Line Number</param>
        /// <returns>Bus Line</returns>
        void AddLine(LineToShow tmpBusLine, List<BO.Station> stations, List<BO.LineStationToShow> stationsToShow);

        /// <summary>
        /// Update Line 
        /// checks all data of line and appeal to update line private
        /// 
        /// throws BOBadLineException
        /// </summary>
        /// <param name="lineToUpdate">Line to update</param>
        /// <param name="lineNumber">line number</param>
        void UpdateLine(LineToShow lineToUpdate, int lineNumber);

        /// <summary>
        /// Adds station to line
        /// 
        /// Throws BOBadLineStationException if already exists
        /// </summary>
        /// <param name="station">Station to add</param>
        void AddStationToLine(LineStation station);

        /// <summary>
        /// Deletes line from data
        /// delets all stations of that line
        /// 
        /// Throws BOBadLineException
        /// </summary>
        /// <param name="lineNumber">Number of line to delete</param>
        void DeleteLine(int id);

        /// <summary>
        /// Returns all indexes where we can add station to line
        /// </summary>
        /// <param name="lineNumber"Number of line></param>
        /// <returns></returns>
        IEnumerable<int> GetAllIndexesToAdd(int lineNumber);
        #endregion

        #region Station

        /// <summary>
        /// </summary>
        /// <returns>all stations</returns>
        IEnumerable<Station> GetAllStations();

        /// <summary>
        /// Returns all stations that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <returns></returns>
        IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate);

        /// <summary>
        /// Returns all lines that pass by a station
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns>ordered list of lines</returns>
        Station GetStation(int id);

        /// <summary>
        /// Updates station in data
        /// checks data of station 
        /// if not good throws :
        /// 
        /// BOBadStationCoordinatesLongitudeException
        /// BOBadStationCoordinatesLatitudeException
        /// BOBadStationNameException
        /// BOBadStationAddressException
        /// BOBadStationNumberException
        /// </summary>
        /// <param name="tmpStation">Station to update</param>
        void UpdateStation(Station tmpStation);

        /// <summary>
        /// Adds station to data
        /// checks data of station 
        /// if not good throws :
        /// 
        /// BOBadStationCoordinatesLongitudeException
        /// BOBadStationCoordinatesLatitudeException
        /// BOBadStationNameException
        /// BOBadStationAddressException
        /// BOBadStationNumberException
        /// </summary>
        /// <param name="tmpStation">Station to add</param>
        void AddStation(Station tmpStation);

        /// <summary>
        /// Deletes station from data
        /// updates all lines
        /// 
        /// Throws BOLineDeleteException if line was deleted
        /// </summary>
        /// <param name="id">Number of station to delete</param>
        void DeleteStation(int id);

        /// <summary>
        /// Returns all lines that pass by a station
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns>ordered list of lines</returns>
        IEnumerable<int> GetAllLinesOfStation(int id);

        /// <summary>
        /// Checks if new station has got good data
        /// 
        /// if not good throws :
        /// 
        /// BOBadStationCoordinatesLongitudeException
        /// BOBadStationCoordinatesLatitudeException
        /// BOBadStationNameException
        /// BOBadStationAddressException
        /// BOBadStationNumberException
        /// 
        /// </summary>
        /// <param name="tmpStation"></param>
        /// <returns>is good ?</returns>
        bool CheckNewStation(Station tmpStation);
        #endregion

        #region User

        /// <summary>
        /// Returns all users 
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Returns all users that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);

        /// <summary>
        /// Returns User that has that name
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <returns></returns>
        User GetUser(string userName);

        /// <summary>
        /// Adds User to data
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="tmpUser"></param>
        void AddUser(User tmpUser);

        /// <summary>
        /// Deletes user from data
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userName"></param>
        void DeleteUser(string name);

        /// <summary>
        /// Updates user in data
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userToUpdate">User to update</param>
        void UpdateUser(User userToUpdate);
        #endregion

        #region LineStation

        /// <summary>
        /// Returns all line stations
        /// </summary>
        /// <returns></returns>
        IEnumerable<LineStation> GetAllLineStations();

        /// <summary>
        /// Returns all line stations that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate);

        /// <summary>
        /// Returns line station in this line with this number
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="stationNumber">Station Number</param>
        /// <param name="lineNumber">Line Number</param>
        /// <returns></returns>
        LineStation GetLineStation(int stationNumber, int lineNumber);

        /// <summary>
        /// Adds station to line in data
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="tmpLineStation">Station to Add</param>
        void AddLineStation(LineStation tmpLineStation);

        /// <summary>
        /// Delete station straight from data
        /// Private use of the logic class
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="stationNumber">Number of station to delete</param>
        /// <param name="lineNumber">Line Number</param>
        void DeleteLineStation(int stationNumber, int lineNumber);

        /// <summary>
        /// Deletes station from line
        /// Updates all indexes of stations after it in line
        /// Creates pair stations if needed
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="stationNumber">Number of station to delete</param>
        /// <param name="lineNumber">Line Number</param>
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
        /// Returns all Line departings
        /// </summary>
        /// <returns></returns>
        IEnumerable<LineDeparting> GetAllLineDeparting();

        /// <summary>
        /// Returns all line departings thet meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        IEnumerable<LineDeparting> GetAllLineDepartingBy(Predicate<LineDeparting> predicate);

        /// <summary>
        /// Returns line departing that meets that data
        /// 
        /// Throws BOBadLineDepartingException
        /// </summary>
        /// <param name="lineNumber">Number of Line</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        LineDeparting GetLineDeparting(int lineNumber, TimeSpan startTime);

        /// <summary>
        /// Adds line departing to data 
        /// checks data , if not good throws
        /// 
        /// BOStopTimeException
        /// BOFrequencyException
        /// BOBadLineDepartingException
        /// 
        /// </summary>
        /// <param name="tmpLineDeparting"></param>
        void AddLineDeparting(LineDeparting tmpLineDeparting);

        /// <summary>
        /// Deletes line departing from data
        /// 
        /// throws BOBadLineDepartingException
        /// </summary>
        /// <param name="lineNumber">Number of line</param>
        /// <param name="startTime">Starting time</param>
        void DeleteLineDeparting(int lineNumber, TimeSpan startTime);

        /// <summary>
        /// Update line departing
        /// checks data , if not good throws :
        /// 
        /// BOStopTimeException
        /// BOFrequencyException
        /// BOBadLineDepartingException
        /// </summary>
        /// <param name="lineDepartingToUpdate"></param>
        void UpdateLineDeparting(LineDeparting lineDepartingToUpdate);
        #endregion

        #region PairStations

        /// <summary>
        /// Returns all pair stations 
        /// </summary>
        /// <returns></returns>
        IEnumerable<PairStations> GetAllPairStations();

        /// <summary>
        /// Returns all pair stations that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate);

        /// <summary>
        /// Returns Pair station that meets the data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="firstStations">First Station Number</param>
        /// <param name="secondStation">Last Station Number</param>
        /// <returns></returns>
        PairStations GetPairStations(int firstStation, int secondStation);

        /// <summary>
        /// Adds pair station ton data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="tmpPairStations">Pair station to add</param>
        void AddPairStations(PairStations tmpPairStations);

        /// <summary>
        /// Deletes pair station from data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="firstStation">First station number</param>
        /// <param name="secondStation">Last station number</param>
        void DeletePairStations(int firstStation, int secondStation);

        /// <summary>
        /// Updates pair station in data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="pairStationsToUpdate">Pair Station to update</param>
        void UpdatePairStations(PairStations pairStationsToUpdate);
        #endregion

        #region UserTrip
        
        IEnumerable<UserTrip> GetAllUserTrip();
        [Obsolete("Class not used in program, needs more implementation")]
        IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate);
        [Obsolete("Class not used in program, needs more implementation")]
        UserTrip GetUserTrip(string name);
        [Obsolete("Class not used in program, needs more implementation")]
        void AddUserTrip(UserTrip tmpUserTrip);
        [Obsolete("Class not used in program, needs more implementation")]
        void DeleteUserTrip(string name);
        [Obsolete("Class not used in program, needs more implementation")]
        void UpdateUserTrip(UserTrip userTripToUpdate);
        #endregion

        #region LineStationToShow

        /// <summary>
        /// returns all Line stations of line to show 
        /// each station has all data needed to be showed
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="lineNumber">Number of Line</param>
        /// <returns></returns>
        IEnumerable<LineStationToShow> GetAllStationsOfLine(int lineNumber);

        /// <summary>
        /// Update Distance and time in line station to show
        /// Updates the pair station 
        /// Checks data
        /// 
        /// Throws exception gotten from DO encapsulated in Exception
        /// </summary>
        /// <param name="myStation"></param>
        /// <param name="line"></param>
        void UpdateDistanceAndTime(LineStationToShow myStation, int lineNumber);
        #endregion

        #region StationToAdd

        /// <summary>
        /// Returns all stations that can be added to a line 
        /// </summary>
        /// <param name="lineNumber">Number of line</param>
        /// <returns></returns>
        IEnumerable<StationToAdd> GetAllStationsToAdd(int lineNumber);
        #endregion

        #region StationToShow

        /// <summary>
        /// Returns a station to show that meets the station Number
        /// </summary>
        /// <param name="stationNumber">Number of station</param>
        /// <returns></returns>
        StationToShow getStationToShow(int stationNumber);
        #endregion

        #region LineToShow

        /// <summary>
        /// Returns all lines to be showed
        /// </summary>
        /// <returns></returns>
        LineToShow GetBusLineToShow(int id);

        /// <summary>
        /// Returns line to show for a specific line
        /// 
        /// Throws BOBadLineException
        /// </summary>
        /// <param name="id">Line number</param>
        /// <returns></returns>
        IEnumerable<LineToShow> GetAllLinesToShow();

        #endregion

        #region Simulation
        void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime);
        void StopSimulator();
        void SetStationPanel(int station, Action<LineTiming> updatePanel);
        bool IsSimulator();
        #endregion
    }
}