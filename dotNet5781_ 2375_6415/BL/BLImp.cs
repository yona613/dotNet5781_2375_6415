using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using Microsoft.Maps.MapControl.WPF;
using DalApi;
using BLApi;
using BO;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;

namespace BL
{
    class BLImp : IBL
    {

        #region singelton
        private static readonly BLImp instance = new BLImp();
        static BLImp() { }// static ctor to ensure instance init is done just before first usage
        private BLImp() { } // default => private
        public static BLImp Instance { get => instance; }// The public Instance property to use
        #endregion

        IDL dal = DalFactory.GetDal(); //Access to Dal
        Random r = new Random(DateTime.Now.Millisecond);

        #region Bus

        /// <summary>
        /// Adapter to copy Do.bus to BO.bus
        /// </summary>
        /// <param name="busDO">DO.Bus</param>
        /// <returns>BO.Bus</returns>
        Bus BusDoBOAdapter(DO.Bus busDO)
        {
            Bus busBO = new Bus();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }
        /// <summary>
        /// Returns Ienumerable of all Buses that satisfies the condiction
        /// 
        /// Throws BOReadDataException if no bus
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        public IEnumerable<Bus> GetAllBuseBy(Predicate<Bus> predicate)
        {
            IEnumerable<Bus> busList = from item in dal.GetAllBuses()
                                       let busBO = BusDoBOAdapter(item)
                                       where predicate(busBO)
                                       select busBO;
            if (busList != null)
                return busList;
            throw new BOReadDataException("No Bus meets the conditions");
        }

        /// <summary>
        /// Returns all buses
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Bus> GetAllBuses()
        {
            return from item in dal.GetAllBuses()
                   select BusDoBOAdapter(item);
        }

        /// <summary>
        /// Returns the Bus that has this id
        /// 
        /// Throws BOReadBusException if not found
        /// </summary>
        /// <param name="license">Bus License</param>
        /// <returns>Bus</returns>
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

        /// <summary>
        /// Adds bus to data
        /// Checks that the data of the bus is good else throws :
        /// 
        /// BOArgumentLicenseException
        /// BOArgumentLicenseDateException
        /// BOArgumentTestDateException
        /// </summary>
        /// <param name="myBus"></param>
        public void AddBus(Bus myBus)
        {
            if (myBus.LicenseDate <= DateTime.Now) //checks the start date entered
            {
                if (myBus.TestDate <= DateTime.Now && myBus.TestDate >= myBus.LicenseDate) // test date needs to be after start date and before now
                {
                    if (myBus.LicenseDate.Year < 2018) //checks format of license number
                    {
                        if (myBus.License >= 1000000 && myBus.License < 10000000) //checks if good license number entered
                        {
                            AddBusPrivate(myBus);
                        }
                        else
                        {
                            throw new BOArgumentLicenseException("License not valid, enter valid number (license : 7 digits) !!!");
                        }
                    }
                    else //other format of license
                    {
                        if (myBus.License >= 10000000 && myBus.License < 100000000) //checks that license is valid
                        {
                            AddBusPrivate(myBus);
                        }
                        else
                        {
                            throw new BOArgumentLicenseException("License not valid, enter valid number (license : 8 digits) !!!");
                        }
                    }
                }
                else
                {
                    throw new BOArgumentTestDateException("Invalid Test date (must be between start date and today's date !!!)");
                }
            }
            else
            {
                throw new BOArgumentLicenseDateException("Invalid Start date (must be before today's date !!!)");
            }
        }

        /// <summary>
        /// If bus needs to be added straight to data
        /// uses this private function
        /// doesn't check data of bus
        /// 
        /// Throws BOBadBusException
        /// </summary>
        /// <param name="busToAdd"></param>
        private void AddBusPrivate(Bus busToAdd)
        {
            DO.Bus busDo = new DO.Bus();
            busToAdd.CopyPropertiesTo(busDo);
            try
            {
                dal.AddBus(busDo);
            }
            catch (DO.BadBusException e)
            {
                throw new BOBadBusException(e.Message, busToAdd.License);
            }
        }

        /// <summary>
        /// Updates bus in data
        /// 
        /// Throws BOBadBusException
        /// </summary>
        /// <param name="busToUpdate">Bus to be Updated</param>
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

        /// <summary>
        /// Deletes bus from Data
        /// 
        /// Throws BOBadBusException if not found
        /// </summary>
        /// <param name="license">License of bus to be deleted</param>
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

        /// <summary>
        /// Adapter to copy Do.Line to BO.Line
        /// </summary>
        /// <param name="busDO">DO.Line</param>
        /// <returns>BO.Line</returns>
        BusLine BusLineDoBOAdapter(DO.BusLine lineDO)
        {
            BusLine lineBO = new BusLine();
            lineDO.CopyPropertiesTo(lineBO);
            return lineBO;
        }

        /// <summary>
        /// </summary>
        /// <returns>all lines in data</returns>
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in dal.GetAllBusLines()
                   select BusLineDoBOAdapter(busLine);
        }

        /// <summary>
        /// Returns all lines that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>All lines that meet condition</returns>
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

        /// <summary>
        /// Returns Bus Line with this number
        /// 
        /// Throws BOBadLineException if not found
        /// </summary>
        /// <param name="id">Line Number</param>
        /// <returns>Bus Line</returns>
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

        /// <summary>
        /// Adds line to data
        /// checks all data of line then adds it
        /// if data is not good throws :
        /// 
        /// BONewLineInsuffisantStationsException
        /// BOBadLineNumberException
        /// BOBadLineException if line already exists
        /// </summary>
        /// <param name="tmpBusLine">BusLine to add</param>
        /// <param name="stations">Physical Stations to add</param>
        /// <param name="stationsToShow">Stations to add</param>
        public void AddLine(LineToShow tmpBusLine, List<BO.Station> stations, List<BO.LineStationToShow> stationsToShow)
        {
            if (stationsToShow.Count < 2)
            {
                throw new BONewLineInsuffisantStationsException("Line needs to deserve at least 2 stations !");
            }
            if (tmpBusLine.LineNumber <= 0)
            {
                throw new BOBadLineNumberException("Line number needs to be bigger than 0 !");
            }
            try //tries if line doesn't already exist
            {
                dal.AddLine(new DO.BusLine() //creates the line based on data
                {
                    LineNumber = tmpBusLine.LineNumber,
                    LineArea = (DO.Area)tmpBusLine.LineArea,
                    FirstStation = stationsToShow[0].StationId,
                    LastStation = stationsToShow.Last().StationId,
                    MyActivity = DO.Activity.On
                });
            }
            catch (DO.BadLineException exception) //if line already exist
            {
                throw new BO.BOBadLineException(exception.Message, tmpBusLine.LineNumber);
            }
            try //add all line stations
            {
                for (int i = 0; i < stationsToShow.Count; i++)
                {
                    dal.AddLineStation(new DO.LineStation() //creates line station
                    {
                        Index = stationsToShow[i].Index,
                        LineNumber = tmpBusLine.LineNumber,
                        MyActivity = DO.Activity.On,
                        StationNumber = stationsToShow[i].StationId
                    });
                    if (i != stationsToShow.Count - 1) //calculates distance between stations
                    {
                        double distance = stationsToShow[i].Coordinates.GetDistanceTo(stationsToShow[i + 1].Coordinates);
                        dal.AddPairStations(new DO.PairStations() //adds pair stations
                        {
                            Distance = distance,
                            FirstStationNumber = stationsToShow[i].StationId,
                            LastStationNumber = stationsToShow[i + 1].StationId,
                            Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0)))
                        });
                    }
                }
                foreach (var item in stations) //adds all physical stations
                {
                    dal.AddStation((DO.Station)item.CopyPropertiesToNew(typeof(DO.Station)));
                }
            }
            catch (Exception exception) // can get a lot of kinds of exception
            {
                throw exception;
            }
        }

        /// <summary>
        /// Updates a line in data
        /// Throws BOBadLineException
        /// </summary>
        /// <param name="lineToUpdate">Line To Update</param>
        private void UpdateLine(BusLine lineToUpdate)
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

        /// <summary>
        /// Update Line 
        /// checks all data of line and appeal to update line private
        /// 
        /// throws BOBadLineException
        /// </summary>
        /// <param name="lineToUpdate">Line to update</param>
        /// <param name="lineNumber">line number</param>
        public void UpdateLine(LineToShow lineToUpdate, int lineNumber)
        {
            if (lineNumber != lineToUpdate.LineNumber) //if changed number
            {
                try //checks if line already exists
                {
                    dal.GetBusLine(lineToUpdate.LineNumber);
                    throw new BOBadLineException($"The line {lineToUpdate.LineNumber} already exists", lineNumber);
                }
                catch (DO.BadLineException) { }
            }
            DO.BusLine lineToUpdateDO = dal.GetBusLine(lineNumber); //get the line and changes his data
            lineToUpdateDO.LineNumber = lineToUpdate.LineNumber;
            lineToUpdateDO.LineArea = (DO.Area)((int)lineToUpdate.LineArea);
            try
            {
                dal.DeleteLine(lineNumber); //delete old data
                dal.AddLine(lineToUpdateDO); //add new data
                List<LineStation> stationList = GetAllLineStationsBy(x => x.LineNumber == lineNumber).ToList();
                for (int i = 0; i < stationList.Count; i++) //update all stations to new number of line
                {
                    DeleteLineStationPrivate(stationList[i].StationNumber, stationList[i].LineNumber);
                    stationList[i].LineNumber = lineToUpdateDO.LineNumber;
                    AddLineStation(stationList[i]);
                }
                List<LineDeparting> lineDepartingList = GetAllLineDepartingBy(x => x.LineNumber == lineNumber).ToList();
                for (int i = 0; i < lineDepartingList.Count; i++) //update all line departing to new number of line
                {
                    DeleteLineDeparting(lineDepartingList[i].LineNumber, lineDepartingList[i].StartTime);
                    lineDepartingList[i].LineNumber = lineToUpdateDO.LineNumber;
                    AddLineDeparting(lineDepartingList[i]);
                }
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, lineToUpdate.LineNumber);
            }
        }

        /// <summary>
        /// Adds station to line
        /// 
        /// Throws BOBadLineStationException if already exists
        /// </summary>
        /// <param name="station">Station to add</param>
        public void AddStationToLine(LineStation station)
        {
            //checks if already exist in line
            LineStation lineStation = GetAllLineStationsBy(x => x.LineNumber == station.LineNumber && x.StationNumber == station.StationNumber).FirstOrDefault();
            if (lineStation != null)
                throw new BOBadLineStationException($"Cannot add station {station.StationNumber}, it already exists in line {station.LineNumber}", station.LineNumber, station.StationNumber);
            else //adds station and updates index of all other stations after it in line
            {
                if (station.Index == 1) //if first station
                {
                    //update line
                    BusLine myLine = GetBusLine(station.LineNumber);
                    myLine.FirstStation = station.StationNumber;
                    UpdateLine(myLine);
                }
                int index = 1;
                AddLineStation(station);
                IEnumerable<LineStation> myLineStations = GetAllLineStationsBy(x => x.LineNumber == station.LineNumber).OrderBy(x => x.Index);
                foreach (var item in myLineStations) //updates all stations after this one in line
                {
                    if (item.Index == station.Index - 1) //creates pair station for station before
                    {
                        Station lastStation = GetStation(station.StationNumber);
                        Station firstStation = GetStation(item.StationNumber);
                        double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                        try
                        {
                            AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                        }
                        catch (BOBadPairStationException e)
                        { }; //if pair station already exist we get a exception but it doesn't bother us
                    }
                    if (item.Index == station.Index) //creates pair station to station after
                    {
                        Station firstStation = GetStation(station.StationNumber);
                        Station lastStation = GetStation(item.StationNumber);
                        double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                        try
                        {
                            AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                        }
                        catch (BOBadPairStationException e)
                        { };//if pair station already exist we get a exception but it doesn't bother us
                    }
                    if (item.Index >= station.Index) //updates indexes
                    {
                        if (item.StationNumber != station.StationNumber)
                        {
                            item.Index++;
                            UpdateLineStation(item);
                        }
                    }
                    index++;
                }
                if (index == station.Index + 1) //if last station update line
                {
                    BusLine myLine = GetBusLine(station.LineNumber);
                    myLine.LastStation = station.StationNumber;
                    UpdateLine(myLine);
                }
            }
        }


        /// <summary>
        /// Deletes line from data
        /// delets all stations of that line
        /// 
        /// Throws BOBadLineException
        /// </summary>
        /// <param name="lineNumber">Number of line to delete</param>
        public void DeleteLine(int lineNumber)
        {
            var myLineStationsList = GetAllLineStationsBy(x => x.LineNumber == lineNumber).ToList();
            for (int i = 0; i < myLineStationsList.Count; i++) //deletes all stations of line
            {
                DeleteLineStationPrivate(myLineStationsList[i].StationNumber, myLineStationsList[i].LineNumber);
            }
            var myLineDepartingList = GetAllLineDepartingBy(x => x.LineNumber == lineNumber).ToList();
            for (int i = 0; i < myLineDepartingList.Count; i++)//deletes all line departings of line
            {
                DeleteLineDeparting(lineNumber, myLineDepartingList[i].StartTime);
            }
            try
            {
                dal.DeleteLine(lineNumber); //delete line
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, lineNumber);
            }
        }

        /// <summary>
        /// Returns all indexes where we can add station to line
        /// </summary>
        /// <param name="lineNumber"Number of line></param>
        /// <returns></returns>
        public IEnumerable<int> GetAllIndexesToAdd(int lineNumber)
        {
            List<int> indexes = GetAllLineStationsBy(x => x.LineNumber == lineNumber).OrderBy(x => x.Index).Select(x => x.Index).ToList();
            indexes.Add(indexes.Last() + 1); //adds last index
            return from index in indexes
                   select index;

        }
        #endregion

        #region Station

        /// <summary>
        /// Adapter to copy Do.Station to BO.Station
        /// </summary>
        /// <param name="stationDO">DO.Station</param>
        /// <returns>BO.Station</returns>
        Station StationDoBOAdapter(DO.Station stationDO)
        {
            Station stationBO = new Station();
            stationDO.CopyPropertiesTo(stationBO);
            return stationBO;
        }

        /// <summary>
        /// </summary>
        /// <returns>all stations</returns>
        public IEnumerable<Station> GetAllStations()
        {
            return from station in dal.GetAllStations()
                   select StationDoBOAdapter(station);
        }

        /// <summary>
        /// Returns all stations that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns all lines that pass by a station
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns>ordered list of lines</returns>
        public IEnumerable<int> GetAllLinesOfStation(int id)
        {
            return from station in GetAllLineStations()
                   where station.StationNumber == id
                   orderby station.LineNumber
                   select station.LineNumber;
        }

        /// <summary>
        /// Returns station
        /// 
        /// Throws BOBadStationException
        /// </summary>
        /// <param name="id">Station number</param>
        /// <returns></returns>
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
        public void AddStation(Station tmpStation)
        {
            if (tmpStation.Coordinates.Longitude > 35.5 || tmpStation.Coordinates.Longitude < 34.3)
            {
                throw new BOBadStationCoordinatesLongitudeException(tmpStation.Coordinates.Longitude, $"Longitude : {tmpStation.Coordinates.Longitude} out of bounds \nNeeds to be between 34.3 & 35.5");
            }
            if (tmpStation.Coordinates.Latitude < 31 || tmpStation.Coordinates.Latitude > 33.3)
            {
                throw new BOBadStationCoordinatesLatitudeException(tmpStation.Coordinates.Latitude, $"Latitude : {tmpStation.Coordinates.Latitude} out of bounds \nNeeds to be between 31 & 33.3");
            }
            if (tmpStation.Name == null || tmpStation.Name == "")
            {
                throw new BOBadStationNameException("The station needs to have a name !!");
            }
            if (tmpStation.Address == null || tmpStation.Address == "")
            {
                throw new BOBadStationAddressException("The station needs to have an address !!");
            }
            if (tmpStation.StationId <= 0 || tmpStation.StationId > 999999)
            {
                throw new BOBadStationNumberException(tmpStation.StationId, $"The station number {tmpStation.StationId} is out of bounds \nNeeds to be at most 6 digits !!");
            }
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
        public bool CheckNewStation(Station tmpStation)
        {
            if (tmpStation.Coordinates.Longitude > 35.5 || tmpStation.Coordinates.Longitude < 34.3)
            {
                throw new BOBadStationCoordinatesLongitudeException(tmpStation.Coordinates.Longitude, $"Longitude : {tmpStation.Coordinates.Longitude} out of bounds \nNeeds to be between 34.3 & 35.5");
            }
            if (tmpStation.Coordinates.Latitude < 31 || tmpStation.Coordinates.Latitude > 33.3)
            {
                throw new BOBadStationCoordinatesLatitudeException(tmpStation.Coordinates.Latitude, $"Latitude : {tmpStation.Coordinates.Latitude} out of bounds \nNeeds to be between 31 & 33.3");
            }
            if (tmpStation.Name == null || tmpStation.Name == "")
            {
                throw new BOBadStationNameException("The station needs to have a name !!");
            }
            if (tmpStation.Address == null || tmpStation.Address == "")
            {
                throw new BOBadStationAddressException("The station needs to have an address !!");
            }
            if (tmpStation.StationId <= 0 || tmpStation.StationId > 999999)
            {
                throw new BOBadStationNumberException(tmpStation.StationId, $"The station number {tmpStation.StationId} is out of bounds \nNeeds to be at most 6 digits !!");
            }
            try
            {
                dal.GetStation(tmpStation.StationId);
            }
            catch (Exception) //if exception then station doesn't exist
            {
                return true;
            }
            return false;
        }


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
        public void UpdateStation(Station tmpStation)
        {
            if (tmpStation.Coordinates.Longitude > 35.5 || tmpStation.Coordinates.Longitude < 34.3)
            {
                throw new BOBadStationCoordinatesLongitudeException(tmpStation.Coordinates.Longitude, $"Longitude : {tmpStation.Coordinates.Longitude} out of bounds \nNeeds to be between 34.3 & 35.5");
            }
            if (tmpStation.Coordinates.Latitude < 31 || tmpStation.Coordinates.Latitude > 33.3)
            {
                throw new BOBadStationCoordinatesLatitudeException(tmpStation.Coordinates.Latitude, $"Latitude : {tmpStation.Coordinates.Latitude} out of bounds \nNeeds to be between 31 & 33.3");
            }
            if (tmpStation.Name == null || tmpStation.Name == "")
            {
                throw new BOBadStationNameException("The station needs to have a name !!");
            }
            if (tmpStation.Address == null || tmpStation.Address == "")
            {
                throw new BOBadStationAddressException("The station needs to have an address !!");
            }
            try
            {
                //update all pair stations where on of the stations is the one to update
                List<PairStations> pairStations = GetAllPairStationsBy(x => x.FirstStationNumber == tmpStation.StationId).ToList();
                for (int i = 0; i < pairStations.Count; i++)
                {
                    Station tmpStation1 = GetStation(pairStations[i].LastStationNumber);
                    pairStations[i].Distance = tmpStation.Coordinates.GetDistanceTo(tmpStation1.Coordinates);
                    dal.UpdatePairStations((DO.PairStations)pairStations[i].CopyPropertiesToNew(typeof(DO.PairStations)));
                }
                pairStations = GetAllPairStationsBy(x => x.LastStationNumber == tmpStation.StationId).ToList();
                for (int i = 0; i < pairStations.Count; i++)
                {
                    Station tmpStation1 = GetStation(pairStations[i].FirstStationNumber);
                    pairStations[i].Distance = tmpStation1.Coordinates.GetDistanceTo(tmpStation.Coordinates);
                    dal.UpdatePairStations((DO.PairStations)pairStations[i].CopyPropertiesToNew(typeof(DO.PairStations)));
                }
                dal.DeleteStation(tmpStation.StationId); //deletes old data
                dal.AddStation((DO.Station)tmpStation.CopyPropertiesToNew(typeof(DO.Station))); // add new data
            }
            catch (DO.BadStationException e)
            {
                throw new BOBadStationException(e.Message, tmpStation.StationId);
            }
        }

        /// <summary>
        /// Deletes station from data
        /// updates all lines
        /// 
        /// Throws BOLineDeleteException if line was deleted
        /// </summary>
        /// <param name="id">Number of station to delete</param>
        public void DeleteStation(int id)
        {
            var myStations = GetAllLineStations().GroupBy(x => x.LineNumber).ToList(); //gets all stations
            var busLines = GetAllBusLines().ToList(); //gets all lines
            List<int> linesDeleted = new List<int>() { };
            for (int i = 0; i < busLines.Count; i++) //for each line update all stations if this station was in line
            {
                bool first = false; //if the station deleted was first
                bool deleted = false; //if the station was in this line and was deleted
                int lastIndex = 0; //last index updated
                var lineStations = myStations.FirstOrDefault(x => x.Key == busLines[i].LineNumber).OrderBy(x => x.Index).ToList(); //gets all stations of this line
                IEnumerable<int> sizeCol = from station in lineStations
                                           where station.StationNumber == busLines[i].LastStation
                                           select station.Index;
                int size = sizeCol.First(); //number of stations
                if (size <= 2) //if only 2 stations and we delete one then no enough station so delete line
                {

                    LineStation station = null;
                    try
                    {
                        station = GetLineStation(id, busLines[i].LineNumber);
                    }
                    catch (Exception) { } //exception will be sent if station does'nt exist , it doesn't bother us
                    if (station != null) //the deleted station is in the line
                    {
                        DeleteLine(busLines[i].LineNumber);
                        linesDeleted.Add(busLines[i].LineNumber); //adds line deleted to list for exception throw
                        continue;
                        //<throw new BOLineDeleteException($"Line {busLines[i].LineNumber} too small, it has been deleted", busLines[i].LineNumber);
                    }

                }
                for (int j = 0; j < lineStations.Count; j++) //updates all indexes of stations of line where the deleted station was
                {
                    if (lineStations[j].StationNumber == id) //if the station needs to be deleted
                    {
                        if (lineStations[j].StationNumber == busLines[i].FirstStation)
                        {
                            //first station
                            DeleteLineStationPrivate(lineStations[j].StationNumber, busLines[i].LineNumber);
                            first = true;
                            deleted = true;
                            continue;
                        }
                        if (lineStations[j].StationNumber == busLines[i].LastStation)
                        {
                            //last station
                            lastIndex = lineStations[j].Index;
                            DeleteLineStationPrivate(lineStations[j].StationNumber, busLines[i].LineNumber);
                            break;
                        }
                        else
                        {
                            //delete station and adds pair station between station before and station after
                            DeleteLineStationPrivate(lineStations[j].StationNumber, busLines[i].LineNumber);
                            Station firstStation = StationDoBOAdapter(dal.GetStation(lineStations[j - 1].StationNumber));
                            Station lastStation = StationDoBOAdapter(dal.GetStation(lineStations[j + 1].StationNumber));
                            double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                            try
                            {
                                dal.AddPairStations(new DO.PairStations()
                                {
                                    FirstStationNumber = firstStation.StationId,
                                    LastStationNumber = lastStation.StationId,
                                    MyActivity = DO.Activity.On,
                                    Distance = distance,
                                    Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0)))
                                });
                            }
                            catch (Exception) { }
                            deleted = true; //station in line was deleted
                            continue;
                        }
                    }
                    if (deleted) //we need to update indexes in next stations
                    {
                        lineStations[j].Index--;
                        UpdateLineStation(lineStations[j]);
                    }
                    if (first)// need to update line
                    {
                        first = false; //not to update in next iteration
                        busLines[i].FirstStation = lineStations[j].StationNumber;
                        UpdateLine(busLines[i]);
                    }
                }
                if (lastIndex != 0) //last station was deleted
                {
                    busLines[i].LastStation = lineStations.FirstOrDefault(x => x.Index == lastIndex - 1).StationNumber;
                    UpdateLine(busLines[i]);
                }
            }
            var myPairStations = GetAllPairStationsBy(x => (x.FirstStationNumber == id) || (x.LastStationNumber == id)).ToList();
            for (int i = 0; i < myPairStations.Count; i++) //delete all pair stations where this station was
            {
                DeletePairStations(myPairStations[i].FirstStationNumber, myPairStations[i].LastStationNumber);
            }
            try
            {
                dal.DeleteStation(id); //delete the station
            }
            catch (DO.BadStationException e)
            {
                throw new BOBadStationException(e.Message, id);
            }
            finally
            {
                //throws exception if lines where deleted
                if (linesDeleted.Count != 0)
                {
                    string message = "No enough stations, Those lines have been deleted : \n";
                    foreach (var item in linesDeleted)
                    {
                        message += item.ToString() + "  ";
                    }
                    throw new BONewLineInsuffisantStationsException(message);
                }
            }
        }
        #endregion

        #region User

        /// <summary>
        /// Copy DO.User to BO.User
        /// </summary>
        /// <param name="userDO">DO.User</param>
        /// <returns>BO.User</returns>
        User UserDoBOAdapter(DO.User userDO)
        {
            User userBO = new User();
            userDO.CopyPropertiesTo(userBO);
            return userBO;
        }

        /// <summary>
        /// Returns all users 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return from user in dal.GetAllUsers()
                   select UserDoBOAdapter(user);
        }

        /// <summary>
        /// Returns all users that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            var myUsers = from user in dal.GetAllUsers()
                          let userBO = UserDoBOAdapter(user)
                          where predicate(userBO)
                          select userBO;

            if (myUsers == null)
                throw new BOReadDataException("No User meets the conditions");
            return myUsers;
        }

        /// <summary>
        /// Returns User that has that name
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <returns></returns>
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


        /// <summary>
        /// Updates user in data
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userToUpdate">User to update</param>
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


        /// <summary>
        /// Adds User to data
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="tmpUser"></param>
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

        /// <summary>
        /// Deletes user from data
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userName"></param>
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

        /// <summary>
        /// Copy DO.LineStation to BO.LineStation
        /// </summary>
        /// <param name="lineStationDO">DO.LineStation</param>
        /// <returns>BO.LineStation</returns>
        LineStation LineStationDoBOAdapter(DO.LineStation lineStationDO)
        {
            LineStation lineStationBO = new LineStation();
            lineStationDO.CopyPropertiesTo(lineStationBO);
            return lineStationBO;
        }

        /// <summary>
        /// Returns all line stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from lineStation in dal.GetAllLineStations()
                   select LineStationDoBOAdapter(lineStation);
        }

        /// <summary>
        /// Returns all line stations that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns line station in this line with this number
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="stationNumber">Station Number</param>
        /// <param name="lineNumber">Line Number</param>
        /// <returns></returns>
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


        /// <summary>
        /// Adds station to line in data
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="tmpLineStation">Station to Add</param>
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


        /// <summary>
        /// Delete station straight from data
        /// Private use of the logic class
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="stationNumber">Number of station to delete</param>
        /// <param name="lineNumber">Line Number</param>
        private void DeleteLineStationPrivate(int stationNumber, int lineNumber)
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


        /// <summary>
        /// Deletes station from line
        /// Updates all indexes of stations after it in line
        /// Creates pair stations if needed
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="stationNumber">Number of station to delete</param>
        /// <param name="lineNumber">Line Number</param>
        public void DeleteLineStation(int stationNumber, int lineNumber)
        {
            bool first = false; //if first station
            bool deleted = false; //to know when deleted
            int lastIndex = 0; //to know if last station was deleted
            var myLine = GetAllBusLinesBy(x => x.LineNumber == lineNumber).FirstOrDefault(); //gets line
            var lineStations = GetAllLineStationsBy(x => x.LineNumber == lineNumber).OrderBy(x => x.Index).ToList(); //gets all stations of the line ordered by index
            if (lineStations.Count <= 2) //if line has only 2 stations and we delete one then the aal line needs to be deleted
            {
                DeleteLine(lineNumber);
                throw new BOLineDeleteException($"Line {lineNumber} too small, it has been deleted", lineNumber);
            }
            for (int j = 0; j < lineStations.Count; j++) //searches for line station to delete
            {
                if (lineStations[j].StationNumber == stationNumber) //when needs to be deleted
                {
                    if (lineStations[j].StationNumber == myLine.FirstStation) //if first station
                    {
                        try
                        {
                            dal.DeleteLineStation(stationNumber, lineNumber);
                        }
                        catch (DO.BadLineStationException e)
                        {
                            throw new BOBadLineStationException(e.Message, lineNumber, stationNumber);
                        }
                        first = true; //first was deleted
                        deleted = true; //station was deleted
                        continue;
                    }
                    if (lineStations[j].StationNumber == myLine.LastStation) // if last station
                    {
                        lastIndex = lineStations[j].Index; //gets the index
                        try
                        {
                            dal.DeleteLineStation(stationNumber, lineNumber);
                        }
                        catch (DO.BadLineStationException e)
                        {
                            throw new BOBadLineStationException(e.Message, lineNumber, stationNumber);
                        }
                        break;
                    }
                    else
                    {
                        try
                        {
                            dal.DeleteLineStation(stationNumber, lineNumber);
                        }
                        catch (DO.BadLineStationException e)
                        {
                            throw new BOBadLineStationException(e.Message, lineNumber, stationNumber);
                        }
                        //creates pair station for station before and station after
                        Station lastStation = GetStation(lineStations[j + 1].StationNumber);
                        Station firstStation = GetStation(lineStations[j - 1].StationNumber);
                        double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                        try
                        {
                            //adds pair station
                            AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                        }
                        catch (BOBadPairStationException e)//exception will be throwed if pair station already exist but it doesn't bother us
                        { };
                        deleted = true;//station was deleted
                        continue;
                    }

                }
                if (deleted)//we need to update indexes
                {
                    lineStations[j].Index--;
                    UpdateLineStation(lineStations[j]);
                }
                if (first)//need to update line's first station
                {
                    first = false;
                    myLine.FirstStation = lineStations[j].StationNumber;
                    UpdateLine(myLine);
                }
            }
            if (lastIndex != 0)//need to update line's last station 
            {
                myLine.LastStation = lineStations.FirstOrDefault(x => x.Index == lastIndex - 1).StationNumber;
                UpdateLine(myLine);
            }
        }


        /// <summary>
        /// Updates line statuion in data
        /// 
        /// Throws BOBadLineStationException
        /// </summary>
        /// <param name="lineStationToUpdate">Line Station to update</param>
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
        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        BusInTravel BusInTravelDOBOAdapter(DO.BusInTravel busInTravelDO)
        {
            BusInTravel busInTravelBO = new BusInTravel();
            busInTravelDO.CopyPropertiesTo(busInTravelBO);
            return busInTravelBO;
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public IEnumerable<BusInTravel> GetAllBusInTravel()
        {
            return from busInTravel in dal.GetAllBusInTravel()
                   select BusInTravelDOBOAdapter(busInTravel);
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
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

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
        public BusInTravel GetBusInTravel(int license, int line, DateTime departureTime)
        {
            DO.BusInTravel busInTravelDO;
            try
            {
                busInTravelDO = dal.GetBusInTravel(license, line, departureTime);
                return BusInTravelDOBOAdapter(busInTravelDO);
            }
            catch (DO.BadBusInTravelException e)
            {
                throw new BOBadBusInTravelException(e.Message, line, license, departureTime);
            }
        }

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
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

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
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

        [Obsolete("This Class wasn't used in the project, needs more implementation")]
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

        /// <summary>
        /// Copy DO.LineDeparting to BO.LineDeparting
        /// </summary>
        /// <param name="lineDepartingDO">DO.LineDEparting</param>
        /// <returns>BO.LineDeparting</returns>
        LineDeparting LineDepartingDOBOAdapter(DO.LineDeparting lineDepartingDO)
        {
            LineDeparting lineDepartingBO = new LineDeparting();
            lineDepartingDO.CopyPropertiesTo(lineDepartingBO);
            return lineDepartingBO;
        }

        /// <summary>
        /// Returns all Line departings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LineDeparting> GetAllLineDeparting()
        {
            return from lineDeparting in dal.GetAllLineDeparting()
                   select LineDepartingDOBOAdapter(lineDeparting);
        }


        /// <summary>
        /// Returns all line departings thet meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns line departing that meets that data
        /// 
        /// Throws BOBadLineDepartingException
        /// </summary>
        /// <param name="lineNumber">Number of Line</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public LineDeparting GetLineDeparting(int lineNumber, TimeSpan startTime)
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
        public void AddLineDeparting(LineDeparting tmpLineDeparting)
        {
            if (tmpLineDeparting.StartTime >= tmpLineDeparting.StopTime)//needs to be on same day
            {
                throw new BOStopTimeException("Error : Start Time and Stop Time need to be on the same day !");
            }
            if (tmpLineDeparting.Frequency > tmpLineDeparting.StopTime - tmpLineDeparting.StartTime)
            {
                //frequency needs to be less than entire time 
                throw new BOFrequencyException("Error : Frequency must be less or equal to interval !");
            }
            IEnumerable<LineDeparting> myLineDeparting = GetAllLineDepartingBy(x => x.LineNumber == tmpLineDeparting.LineNumber).OrderBy(x => x.StartTime);
            foreach (var item in myLineDeparting) //checks if there is no line departing in that hours
            {
                //if there is one we need to update it
                if (tmpLineDeparting.StartTime >= item.StartTime && tmpLineDeparting.StopTime <= item.StopTime)
                {
                    try
                    {
                        dal.DeleteLineDeparting(item.LineNumber, item.StartTime);
                        dal.AddLineDeparting(new DO.LineDeparting() { StartTime = item.StartTime, StopTime = tmpLineDeparting.StartTime, Frequency = item.Frequency, LineNumber = item.LineNumber, MyActivity = DO.Activity.On });
                        item.StartTime = tmpLineDeparting.StopTime;
                        dal.AddLineDeparting(new DO.LineDeparting() { StartTime = item.StartTime, StopTime = item.StartTime, Frequency = item.Frequency, LineNumber = item.LineNumber, MyActivity = DO.Activity.On });
                    }
                    catch (DO.BadLineDepartingException e)
                    {
                        throw new BOBadLineDepartingException(e.Message, tmpLineDeparting.LineNumber, tmpLineDeparting.StartTime);
                    }
                }
                if (tmpLineDeparting.StartTime >= item.StartTime && tmpLineDeparting.StartTime <= item.StopTime)
                {
                    item.StopTime = tmpLineDeparting.StartTime;
                    UpdateLineDeparting(item);
                }
                if (tmpLineDeparting.StopTime >= item.StartTime && tmpLineDeparting.StopTime <= item.StopTime)
                {
                    UpdateLineDeparting(item);
                }
            }
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


        /// <summary>
        /// Deletes line departing from data
        /// 
        /// throws BOBadLineDepartingException
        /// </summary>
        /// <param name="lineNumber">Number of line</param>
        /// <param name="startTime">Starting time</param>
        public void DeleteLineDeparting(int lineNumber, TimeSpan startTime)
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

        /// <summary>
        /// Update line departing
        /// checks data , if not good throws :
        /// 
        /// BOStopTimeException
        /// BOFrequencyException
        /// BOBadLineDepartingException
        /// </summary>
        /// <param name="lineDepartingToUpdate"></param>
        public void UpdateLineDeparting(LineDeparting lineDepartingToUpdate)
        {
            if (lineDepartingToUpdate.StartTime >= lineDepartingToUpdate.StopTime)
            {
                throw new BOStopTimeException("Error : Start Time and Stop Time need to be on the same day !");
            }
            if (lineDepartingToUpdate.Frequency > lineDepartingToUpdate.StopTime - lineDepartingToUpdate.StartTime)
            {
                throw new BOFrequencyException("Error : Frequency must be less or equal to interval !");
            }
            DO.LineDeparting lineDepartingDO = new DO.LineDeparting();
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

        #region PairStations

        /// <summary>
        /// Copy DO.PairStations to BO.PairStations
        /// </summary>
        /// <param name="pairStationsDO">DO.PairStations</param>
        /// <returns>BO.PairStations</returns>
        PairStations PairStationsDoBOAdapter(DO.PairStations pairStationsDO)
        {
            PairStations pairStationsBO = new PairStations();
            pairStationsDO.CopyPropertiesTo(pairStationsBO);
            return pairStationsBO;
        }

        /// <summary>
        /// Returns all pair stations 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PairStations> GetAllPairStations()
        {
            return from pairStations in dal.GetAllPairStations()
                   select PairStationsDoBOAdapter(pairStations);
        }

        /// <summary>
        /// Returns all pair stations that meet the condition
        /// 
        /// Throws BOReadDataException
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        public IEnumerable<PairStations> GetAllPairStationsBy(Predicate<PairStations> predicate)
        {
            IEnumerable<PairStations> myPairStationsList = from pairStations in dal.GetAllPairStations()
                                                           let item = PairStationsDoBOAdapter(pairStations)
                                                           where predicate(item)
                                                           select item;
            if (myPairStationsList != null)
                return myPairStationsList;
            throw new BOReadDataException("No Pair Stations meets the conditions");
        }

        /// <summary>
        /// Returns Pair station that meets the data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="firstStations">First Station Number</param>
        /// <param name="secondStation">Last Station Number</param>
        /// <returns></returns>
        public PairStations GetPairStations(int firstStations, int secondStation)
        {
            DO.PairStations pairStationsDo;
            try
            {
                pairStationsDo = dal.GetPairStations(firstStations, secondStation);
                return PairStationsDoBOAdapter(pairStationsDo);
            }
            catch (DO.BadPairStationException e)
            {
                throw new BOBadPairStationException(e.Message, firstStations, secondStation);
            }
        }


        /// <summary>
        /// Adds pair station ton data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="tmpPairStations">Pair station to add</param>
        public void AddPairStations(PairStations tmpPairStations)
        {
            try
            {
                dal.AddPairStations((DO.PairStations)tmpPairStations.CopyPropertiesToNew(typeof(DO.PairStations)));
            }
            catch (DO.BadPairStationException e)
            {
                throw new BOBadPairStationException(e.Message, tmpPairStations.FirstStationNumber, tmpPairStations.LastStationNumber);
            }
        }


        /// <summary>
        /// Deletes pair station from data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="firstStation">First station number</param>
        /// <param name="secondStation">Last station number</param>
        public void DeletePairStations(int firstStation, int secondStation)
        {
            try
            {
                dal.DeletePairStations(firstStation, firstStation);
            }
            catch (DO.BadPairStationException e)
            {
                //throw new BOBadPairStationException(e.Message, firstStation, firstStation);
            }
        }


        /// <summary>
        /// Updates pair station in data
        /// 
        /// Throws BOBadPairStationException
        /// </summary>
        /// <param name="pairStationsToUpdate">Pair Station to update</param>
        public void UpdatePairStations(PairStations pairStationsToUpdate)
        {
            try
            {
                dal.UpdatePairStations((DO.PairStations)pairStationsToUpdate.CopyPropertiesToNew(typeof(DO.PairStations)));
            }
            catch (DO.BadPairStationException e)
            {
                throw new BOBadPairStationException(e.Message, pairStationsToUpdate.FirstStationNumber, pairStationsToUpdate.LastStationNumber);
            }
        }

        #endregion

        #region UserTrip

        [Obsolete("Class not used in program, needs more implementation")]
        /// <summary>
        /// Copy DO.UserTrip to BO.UserTrip
        /// </summary>
        /// <param name="userTripDO">DO.UserTrip</param>
        /// <returns>BO.UserTrip</returns>
        UserTrip UserTripDoBOAdapter(DO.UserTrip userTripDO)
        {
            UserTrip userTripBO = new UserTrip();
            userTripDO.CopyPropertiesTo(userTripBO);
            return userTripBO;
        }

        [Obsolete("Class not used in program, needs more implementation")]
        public IEnumerable<UserTrip> GetAllUserTrip()
        {
            return from userTrip in dal.GetAllUserTrip()
                   select UserTripDoBOAdapter(userTrip);
        }

        [Obsolete("Class not used in program, needs more implementation")]
        public IEnumerable<UserTrip> GetAllUserTripBy(Predicate<UserTrip> predicate)
        {
            IEnumerable<UserTrip> myUserTripList = from userTrip in dal.GetAllUserTrip()
                                                   let item = UserTripDoBOAdapter(userTrip)
                                                   where predicate(item)
                                                   select item;
            if (myUserTripList != null)
                return myUserTripList;
            throw new BOBadUserTripException("No Line meets the conditions");
        }

        [Obsolete("Class not used in program, needs more implementation")]
        public UserTrip GetUserTrip(string name)
        {
            DO.UserTrip userTripDo;
            try
            {
                userTripDo = dal.GetUserTrip(name);
                return UserTripDoBOAdapter(userTripDo);
            }
            catch (DO.BadUserTripException e)
            {
                throw new BOBadUserTripException(e.Message);
            }
        }

        [Obsolete("Class not used in program, needs more implementation")]
        public void AddUserTrip(UserTrip tmpUserTrip)
        {
            try
            {
                dal.AddUserTrip((DO.UserTrip)tmpUserTrip.CopyPropertiesToNew(typeof(DO.UserTrip)));
            }
            catch (DO.BadUserTripException e)
            {
                throw new BOBadUserTripException(e.Message);
            }
        }

        [Obsolete("Class not used in program, needs more implementation")]
        public void DeleteUserTrip(string name)
        {
            try
            {
                dal.DeleteUserTrip(name);
            }
            catch (DO.BadLineException e)
            {
                throw new NotImplementedException(e.Message);
            }
        }

        [Obsolete("Class not used in program, needs more implementation")]
        public void UpdateUserTrip(UserTrip userTripToUpdate)
        {
            try
            {
                dal.UpdateUserTrip((DO.UserTrip)userTripToUpdate.CopyPropertiesToNew(typeof(DO.UserTrip)));
            }
            catch (DO.BadUserTripException e)
            {
                throw new BOBadUserTripException(e.Message);
            }
        }
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
        public IEnumerable<LineStationToShow> GetAllStationsOfLine(int lineNumber)
        {
            //gets all data by querying dal
            var myLineStationsTmp = from station in GetAllLineStationsBy(x => x.LineNumber == lineNumber)
                                    orderby station.Index
                                    let station1 = GetStation(station.StationNumber)
                                    select new { StationId = station1.StationId, Name = station1.Name, Address = station1.Address, Index = station.Index, Coordinates = station1.Coordinates };
            IEnumerable<LineStationToShow> myLineStations = from station in myLineStationsTmp
                                                            select (LineStationToShow)station.CopyPropertiesToNew(typeof(LineStationToShow));
            var myLineStationsList = myLineStations.ToList();
            for (int i = 0; i < myLineStationsList.Count - 1; i++)
            {
                var pairStation = dal.GetPairStations(myLineStationsList[i].StationId, myLineStationsList[i + 1].StationId);
                myLineStationsList[i].Distance = pairStation.Distance;
                myLineStationsList[i].Time = pairStation.Time;
                myLineStationsList[i].lineNumber = lineNumber;
            }
            if (myLineStations == null)
                throw new BOReadDataException("no stations");
            return from lineStation in myLineStationsList
                   select lineStation;

        }


        /// <summary>
        /// Update Distance and time in line station to show
        /// Updates the pair station 
        /// Checks data
        /// 
        /// Throws exception gotten from DO encapsulated in Exception
        /// </summary>
        /// <param name="myStation"></param>
        /// <param name="line"></param>
        public void UpdateDistanceAndTime(LineStationToShow myStation, int line)
        {
            try
            {
                foreach (var item in GetAllStationsOfLine(myStation.lineNumber))
                {
                    if (item.Index == myStation.Index + 1)
                    {
                        //Updates the adequat pair station
                        PairStations pairStation = PairStationsDoBOAdapter(dal.GetPairStations(myStation.StationId, item.StationId));
                        pairStation.Distance = myStation.Distance;
                        pairStation.Time = myStation.Time;
                        dal.UpdatePairStations((DO.PairStations)pairStation.CopyPropertiesToNew(typeof(DO.PairStations)));
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

        #region StationToAdd

        /// <summary>
        /// Returns all stations that can be added to a line 
        /// </summary>
        /// <param name="lineNumber">Number of line</param>
        /// <returns></returns>
        public IEnumerable<StationToAdd> GetAllStationsToAdd(int lineNumber)
        {
            //Get all stations 
            IEnumerable<StationToAdd> stationsToAdd = from station in GetAllStations()
                                                      orderby station.StationId
                                                      let stationToAdd = station.CopyPropertiesToNew(typeof(StationToAdd)) as StationToAdd
                                                      select stationToAdd;
            List<StationToAdd> stations = stationsToAdd.ToList();
            //remove the stations already in the line
            foreach (var item in GetAllLineStationsBy(x => x.LineNumber == lineNumber))
            {
                for (int i = 0; i < stations.Count(); i++)
                {
                    if (stations[i].StationId == item.StationNumber)
                    {
                        stations.RemoveAt(i);
                    }
                }
            }
            return from station in stations
                   select station;
        }
        #endregion

        #region StationToShow

        /// <summary>
        /// Returns a station to show that meets the station Number
        /// </summary>
        /// <param name="stationNumber">Number of station</param>
        /// <returns></returns>
        public StationToShow getStationToShow(int stationNumber)
        {
            //creates the station to show with all the data
            var stationToShow = (StationToShow)dal.GetStation(stationNumber).CopyPropertiesToNew(typeof(StationToShow));
            stationToShow.Lines = from station in GetAllLineStationsBy(x => x.StationNumber == stationNumber)
                                  orderby station.LineNumber
                                  let line = station.LineNumber + "  --->  "
                                  let lastStation = GetBusLine(station.LineNumber).LastStation
                                  let name = GetStation(lastStation).Name
                                  select line + name;
            return stationToShow;

        }
        #endregion

        #region LineToShow

        /// <summary>
        /// Returns all lines to be showed
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LineToShow> GetAllLinesToShow()
        {
            //Creates line to show with all the dtata for all lines in data
            var lines = GetAllBusLines().OrderBy(x => x.LineNumber);
            List<LineToShow> lineToShowList = new List<LineToShow>() { };
            foreach (var item in lines)
            {
                var lineToShow = (LineToShow)item.CopyPropertiesToNew(typeof(LineToShow));
                lineToShow.LineDepartings = from lineDeparting in GetAllLineDepartingBy(x => x.LineNumber == item.LineNumber)
                                            orderby lineDeparting.StartTime
                                            let BOLineDeparting = (BO.LineDeparting)lineDeparting.CopyPropertiesToNew(typeof(BO.LineDeparting))
                                            select BOLineDeparting;
                lineToShow.LineStations = GetAllStationsOfLine(item.LineNumber);
                lineToShow.LastStationName = lineToShow.LineStations.Last().Name;
                lineToShowList.Add(lineToShow);
            }
            return from line in lineToShowList
                   select line;
        }

        /// <summary>
        /// Returns line to show for a specific line
        /// 
        /// Throws BOBadLineException
        /// </summary>
        /// <param name="id">Line number</param>
        /// <returns></returns>
        public LineToShow GetBusLineToShow(int id)
        {
            DO.BusLine busLineDO;
            try
            {
                //gets all data from dal and creates line to show
                busLineDO = dal.GetBusLine(id);
                var lineToShow = (LineToShow)busLineDO.CopyPropertiesToNew(typeof(LineToShow));
                lineToShow.LineDepartings = from lineDeparting in GetAllLineDepartingBy(x => x.LineNumber == id)
                                            orderby lineDeparting.StartTime
                                            let BOLineDeparting = (BO.LineDeparting)lineDeparting.CopyPropertiesToNew(typeof(BO.LineDeparting))
                                            select BOLineDeparting;
                lineToShow.LineStations = GetAllStationsOfLine(id);
                return lineToShow;
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, id);
            }
        }

        #endregion

        #region Simulation

        /// <summary>
        /// Starts the simulation
        /// Starts the clock of the simulator 
        /// Starts the action of sending buses to travel
        /// </summary>
        /// <param name="startTime">Time of start of simulation</param>
        /// <param name="rate">Rate of the clock of the simulation</param>
        /// <param name="updateTime">Action sent to update front end</param>
        public void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            SimulatorClock simulatorClock = SimulatorClock.Instance; //gets simulator instance
            simulatorClock.Cancel = false; //not cancelling
            simulatorClock.Rate = rate; //set the rate
            simulatorClock.stopWatch.Restart(); //starts the timer
            simulatorClock.ClockObserver += updateTime; //Observer (event) that fires when time is updated
            simulatorClock.Time = startTime; //set start time
            SendLinesToTravel(); //starts action of sending lines to travel
            while (simulatorClock.Cancel != true) //works untill we stop the simulator
            {
                //sets new time
                simulatorClock.Time = startTime + new TimeSpan(simulatorClock.stopWatch.ElapsedTicks * simulatorClock.Rate);
                Thread.Sleep(100);//in order to get updates by seconds(simulation seconds)
            }
        }

        /// <summary>
        /// Function that stops the simulator
        /// </summary>
        public void StopSimulator()
        {
            SimulatorClock.Instance.Cancel = true;
        }

        /// <summary>
        /// Set a station to be the observed of the travel simulator
        /// </summary>
        /// <param name="station">Observed station</param>
        /// <param name="updatePanel">Function to register to event of the observer</param>
        public void SetStationPanel(int station, Action<LineTiming> updatePanel)
        {
            TravelSimulator.Instance.StationNumber = station; //sets station
            TravelSimulator.Instance.SetDigitalPanel += updatePanel; //registers function
        }


        /// <summary>
        /// Function that starts Line Departings to send lines to travel
        /// Each line departing is a thread 
        /// </summary>
        public void SendLinesToTravel()
        {
            foreach (var line in GetAllBusLines().OrderBy(x => x.LineNumber)) //for all bus lines
            {
                //for all Line Departinf of this line
                foreach (var lineDeparting in GetAllLineDepartingBy(x => x.LineNumber == line.LineNumber).OrderBy(x => x.StartTime))
                {
                    //Send Line Departing to set lines to travel in a new thread
                    BackgroundWorker LineDepartingBw = new BackgroundWorker();
                    LineDepartingBw.DoWork += SimulationLogic.LineDepartingDoWork;

                    //gets all the data for the specific line in order to send it to the thread
                    LineToShow myLine = GetBusLineToShow(lineDeparting.LineNumber);
                    LineDepartingSimulation myLineDeparting = new LineDepartingSimulation()
                    {
                        LineNumber = myLine.LineNumber,
                        StartTime = lineDeparting.StartTime,
                        StopTime = lineDeparting.StopTime,
                        Frequency = lineDeparting.Frequency,
                        LineStations = myLine.LineStations,
                        LastStation = myLine.LineStations.Last().Name
                    };
                    LineDepartingBw.RunWorkerAsync(myLineDeparting);//Start thread
                }
            }
        }

        /// <summary>
        /// Function to check if simulator is working
        /// </summary>
        /// <returns>Is simulator working ?</returns>
        public bool IsSimulator()
        {
            return SimulatorClock.Instance.Cancel != true;
        }

        #endregion

    }
}