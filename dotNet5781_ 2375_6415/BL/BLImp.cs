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

namespace BL
{
    class BLImp : IBL
    {

        #region singelton
        public static readonly BLImp instance = new BLImp();
        static BLImp() { }// static ctor to ensure instance init is done just before first usage
        BLImp() { } // default => private
        public static BLImp Instance { get => instance; }// The public Instance property to use
        #endregion

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
                                       where predicate(busBO)
                                       select busBO;
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
                //return (LineToShow)busLineDO.CopyPropertiesToNew(typeof(LineToShow));
                return BusLineDoBOAdapter(busLineDO);
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, id);
            }
        }

        public LineToShow GetBusLineToShow(int id)
        {
            DO.BusLine busLineDO;
            try
            {
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

        //public void AddLine(BusLine tmpBusLine, Station firstStation, Station lastStation)
        //{
        //    AddStation(firstStation);
        //    AddStation(lastStation);
        //    AddLineStation(new LineStation() { Index = 1, LineNumber = tmpBusLine.LineNumber, StationNumber = tmpBusLine.FirstStation });
        //    AddLineStation(new LineStation() { Index = 2, LineNumber = tmpBusLine.LineNumber, StationNumber = tmpBusLine.LastStation });
        //    double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
        //    AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
        //    try
        //    {
        //        dal.AddLine((DO.BusLine)tmpBusLine.CopyPropertiesToNew(typeof(DO.BusLine)));
        //    }
        //    catch (DO.BadLineException e)
        //    {
        //        throw new BOBadLineException(e.Message, tmpBusLine.LineNumber);
        //    }
        //}
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
            try
            {
                dal.AddLine(new DO.BusLine() 
                {
                    LineNumber = tmpBusLine.LineNumber,
                    LineArea = (DO.Area)tmpBusLine.LineArea,
                    FirstStation = stationsToShow[0].StationId,
                    LastStation = stationsToShow.Last().StationId,
                    MyActivity = DO.Activity.On
            });
            }
            catch (DO.BadLineException exception )
            {
                throw new BO.BOBadLineException(exception.Message, tmpBusLine.LineNumber);
            }
            try
            {
                for (int i = 0; i < stationsToShow.Count; i++)
                {
                    dal.AddLineStation(new DO.LineStation()
                    {
                        Index = stationsToShow[i].Index,
                        LineNumber = tmpBusLine.LineNumber,
                        MyActivity = DO.Activity.On,
                        StationNumber = stationsToShow[i].StationId
                    });
                    if (i != stationsToShow.Count - 1)
                    {
                        double distance = stationsToShow[i].Coordinates.GetDistanceTo(stationsToShow[i + 1].Coordinates);
                        dal.AddPairStations(new DO.PairStations()
                        {
                            Distance = distance,
                            FirstStationNumber = stationsToShow[i].StationId,
                            LastStationNumber = stationsToShow[i + 1].StationId,
                            Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0)))
                        });
                    }
                }
                foreach (var item in stations)
                {
                    dal.AddStation((DO.Station)item.CopyPropertiesToNew(typeof(DO.Station)));
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }          
            //Station firstStation = GetStation(tmpBusLine.FirstStation);
            //Station lastStation = GetStation(tmpBusLine.LastStation);
            //AddLineStation(new LineStation() { Index = 1, LineNumber = tmpBusLine.LineNumber, StationNumber = tmpBusLine.FirstStation });
            //AddLineStation(new LineStation() { Index = 2, LineNumber = tmpBusLine.LineNumber, StationNumber = tmpBusLine.LastStation });
            //double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
            //AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
            //try
            //{
            //    dal.AddLine((DO.BusLine)tmpBusLine.CopyPropertiesToNew(typeof(DO.BusLine)));
            //}
            //catch (DO.BadLineException e)
            //{
            //    throw new BOBadLineException(e.Message, tmpBusLine.LineNumber);
            //}
        }

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

        public void UpdateLine(LineToShow lineToUpdate, int lineNumber)
        {
            if (lineNumber != lineToUpdate.LineNumber)
            {
                try
                {
                    dal.GetBusLine(lineToUpdate.LineNumber);
                    throw new BOBadLineException($"The line {lineToUpdate.LineNumber} already exists", lineNumber);
                }
                catch (DO.BadLineException) { }
            }
            DO.BusLine lineToUpdateDO = dal.GetBusLine(lineNumber);
            lineToUpdateDO.LineNumber = lineToUpdate.LineNumber;
            lineToUpdateDO.LineArea = (DO.Area)((int)lineToUpdate.LineArea);
            try
            {
                dal.DeleteLine(lineNumber);
                dal.AddLine(lineToUpdateDO);
                List<LineStation> stationList = GetAllLineStationsBy(x => x.LineNumber == lineNumber).ToList();
                for (int i = 0; i < stationList.Count; i++)
                {
                    DeleteLineStationPrivate(stationList[i].StationNumber, stationList[i].LineNumber);
                    stationList[i].LineNumber = lineToUpdateDO.LineNumber;
                    AddLineStation(stationList[i]);
                }
                List<LineDeparting> lineDepartingList = GetAllLineDepartingBy(x => x.LineNumber == lineNumber).ToList();
                for (int i = 0; i < lineDepartingList.Count; i++)
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

        public void AddStationToLine(LineStation station)
        {
            LineStation lineStation = GetAllLineStationsBy(x => x.LineNumber == station.LineNumber && x.StationNumber == station.StationNumber).FirstOrDefault();
            if (lineStation != null)
                throw new BOBadLineStationException($"Cannot add station {station.StationNumber}, it already exists in line {station.LineNumber}", station.LineNumber, station.StationNumber);
            else
            {
                if (station.Index == 1)
                {
                    BusLine myLine = GetBusLine(station.LineNumber);
                    myLine.FirstStation = station.StationNumber;
                    UpdateLine(myLine);
                }
                int index = 1;
                AddLineStation(station);
                IEnumerable<LineStation> myLineStations = GetAllLineStationsBy(x => x.LineNumber == station.LineNumber).OrderBy(x => x.Index);
                foreach (var item in myLineStations)
                {
                    if (item.Index == station.Index - 1)
                    {
                        Station lastStation = GetStation(station.StationNumber);
                        Station firstStation = GetStation(item.StationNumber);
                        double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                        try
                        {
                            AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                        }
                        catch (BOBadPairStationException e)
                        { };
                    }
                    if (item.Index == station.Index)
                    {
                        Station firstStation = GetStation(station.StationNumber);
                        Station lastStation = GetStation(item.StationNumber);
                        double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                        try
                        {
                            AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                        }
                        catch (BOBadPairStationException e)
                        { };
                    }
                    if (item.Index >= station.Index)
                    {
                        if (item.StationNumber != station.StationNumber)
                        {
                            item.Index++;
                            UpdateLineStation(item);
                        }
                    }
                    index++;
                }
                if (index == station.Index + 1)
                {
                    BusLine myLine = GetBusLine(station.LineNumber);
                    myLine.LastStation = station.StationNumber;
                    UpdateLine(myLine);
                }
            }
        }

        public void DeleteStationFromLine(int stationNumber, int lineNumber)
        {
            /*bool first = false;
            bool deleted = false;
            int lastIndex = 0;
            BusLine myLine = GetBusLine(lineNumber);
            IEnumerable<LineStation> myStations = GetAllLineStationsBy(x => x.LineNumber == lineNumber).OrderBy(x => x.Index);
            IEnumerable<int> sizeCol = from station in myStations
                                       where station.StationNumber == myLine.LastStation
                                       select station.Index;
            int size = sizeCol.First();
            if (size <= 2)
            {
                DeleteLine(lineNumber);
                throw new BOReadDataException($"Line {lineNumber} too small, it has been deleted");
            }
            foreach (var station in myStations)
            {
                if (station.StationNumber == stationNumber)
                {
                    if (station.StationNumber == myLine.FirstStation)
                    {
                        DeleteLineStationPrivate(station.StationNumber, myLine.LineNumber);
                        first = true;
                        deleted = true;
                        continue;
                    }
                    if (station.StationNumber == myLine.LastStation)
                    {
                        lastIndex = station.Index;
                        DeleteLineStationPrivate(station.StationNumber, myLine.LineNumber);
                        break;
                    }
                    else
                    {
                        DeleteLineStationPrivate(station.StationNumber, myLine.LineNumber);
                        deleted = true;
                        continue;
                    }

                }
                if (deleted)
                {
                    station.Index--;
                    UpdateLineStation(station);
                }
                if (first)
                {
                    first = false;
                    myLine.FirstStation = station.StationNumber;
                    UpdateLine(myLine);
                }
            }
            myLine.LastStation = myStations.FirstOrDefault(x => x.Index == lastIndex - 1).StationNumber;
            UpdateLine(myLine);*/
        }

        public void DeleteLine(int lineNumber)
        {
            var myLineStationsList = GetAllLineStationsBy(x => x.LineNumber == lineNumber).ToList();
            for (int i = 0; i < myLineStationsList.Count; i++)
            {
                DeleteLineStationPrivate(myLineStationsList[i].StationNumber, myLineStationsList[i].LineNumber);
            }
            var myLineDepartingList = GetAllLineDepartingBy(x => x.LineNumber == lineNumber).ToList();
            for (int i = 0; i < myLineDepartingList.Count; i++)
            {
                DeleteLineDeparting(lineNumber, myLineDepartingList[i].StartTime);
            }
            try
            {
                dal.DeleteLine(lineNumber);
            }
            catch (DO.BadLineException e)
            {
                throw new BOBadLineException(e.Message, lineNumber);
            }
        }

        public IEnumerable<int> GetAllIndexesToAdd(int lineNumber)
        {
            List<int> indexes = GetAllLineStationsBy(x => x.LineNumber == lineNumber).OrderBy(x => x.Index).Select(x => x.Index).ToList();
            indexes.Add(indexes.Last() + 1);
            return from index in indexes
                   select index;

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


        public IEnumerable<int> GetAllLinesOfStation(int id)
        {
            return from station in GetAllLineStations()
                   where station.StationNumber == id
                   orderby station.LineNumber
                   select station.LineNumber;
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
            catch (Exception)
            {
                return true;
            }
            return false;
        }

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
                dal.DeleteStation(tmpStation.StationId);
                dal.AddStation((DO.Station)tmpStation.CopyPropertiesToNew(typeof(DO.Station)));
            }
            catch (DO.BadStationException e)
            {
                throw new BOBadStationException(e.Message, tmpStation.StationId);
            }
        }

        public void DeleteStation(int id)
        {
            var myStations = GetAllLineStations().GroupBy(x => x.LineNumber).ToList();
            var busLines = GetAllBusLines().ToList();
            for (int i = 0; i < busLines.Count; i++)
            {
                bool first = false;
                bool deleted = false;
                int lastIndex = 0;
                var lineStations = myStations.FirstOrDefault(x => x.Key == busLines[i].LineNumber).OrderBy(x => x.Index).ToList();
                IEnumerable<int> sizeCol = from station in lineStations
                                           where station.StationNumber == busLines[i].LastStation
                                           select station.Index;
                int size = sizeCol.First();
                if (size <= 2)
                {
                    DeleteLine(busLines[i].LineNumber);
                    throw new BOLineDeleteException($"Line {busLines[i].LineNumber} too small, it has been deleted", busLines[i].LineNumber);
                }
                for (int j = 0; j < lineStations.Count; j++)
                {
                    if (lineStations[j].StationNumber == id)
                    {
                        if (lineStations[j].StationNumber == busLines[i].FirstStation)
                        {
                            DeleteLineStationPrivate(lineStations[j].StationNumber, busLines[i].LineNumber);
                            first = true;
                            deleted = true;
                            continue;
                        }
                        if (lineStations[j].StationNumber == busLines[i].LastStation)
                        {
                            lastIndex = lineStations[j].Index;
                            DeleteLineStationPrivate(lineStations[j].StationNumber, busLines[i].LineNumber);
                            break;
                        }
                        else
                        {
                            DeleteLineStationPrivate(lineStations[j].StationNumber, busLines[i].LineNumber);
                            Station firstStation = StationDoBOAdapter(dal.GetStation(lineStations[j - 1].StationNumber));
                            Station lastStation = StationDoBOAdapter(dal.GetStation(lineStations[j + 1].StationNumber));
                            double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                            dal.AddPairStations(new DO.PairStations()
                            {
                                FirstStationNumber = firstStation.StationId,
                                LastStationNumber = lastStation.StationId,
                                MyActivity = DO.Activity.On,
                                Distance = distance,
                                Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0)))
                            });
                            deleted = true;
                            continue;
                        }

                    }
                    if (deleted)
                    {
                        lineStations[j].Index--;
                        UpdateLineStation(lineStations[j]);
                    }
                    if (first)
                    {
                        first = false;
                        busLines[i].FirstStation = lineStations[j].StationNumber;
                        UpdateLine(busLines[i]);
                    }
                }
                if (lastIndex != 0)
                {
                    busLines[i].LastStation = lineStations.FirstOrDefault(x => x.Index == lastIndex - 1).StationNumber;
                    UpdateLine(busLines[i]);
                }
            }
            var myPairStations = GetAllPairStationsBy(x => (x.FirstStationNumber == id) || (x.LastStationNumber == id)).ToList();
            for (int i = 0; i < myPairStations.Count; i++)
            {
                DeletePairStations(myPairStations[i].FirstStationNumber, myPairStations[i].LastStationNumber);
            }
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
            var myUsers = from user in dal.GetAllUsers()
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

        public void DeleteLineStation(int stationNumber, int lineNumber)
        {
            bool first = false;
            bool deleted = false;
            int lastIndex = 0;
            var myLine = GetAllBusLinesBy(x => x.LineNumber == lineNumber).FirstOrDefault();
            var lineStations = GetAllLineStationsBy(x => x.LineNumber == lineNumber).OrderBy(x => x.Index).ToList();
            if (lineStations.Count <= 2)
            {
                DeleteLine(lineNumber);
                throw new BOLineDeleteException($"Line {lineNumber} too small, it has been deleted", lineNumber);
            }
            for (int j = 0; j < lineStations.Count; j++)
            {
                if (lineStations[j].StationNumber == stationNumber)
                {
                    if (lineStations[j].StationNumber == myLine.FirstStation)
                    {
                        try
                        {
                            dal.DeleteLineStation(stationNumber, lineNumber);
                        }
                        catch (DO.BadLineStationException e)
                        {
                            throw new BOBadLineStationException(e.Message, lineNumber, stationNumber);
                        }
                        first = true;
                        deleted = true;
                        continue;
                    }
                    if (lineStations[j].StationNumber == myLine.LastStation)
                    {
                        lastIndex = lineStations[j].Index;
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
                        Station lastStation = GetStation(lineStations[j + 1].StationNumber);
                        Station firstStation = GetStation(lineStations[j - 1].StationNumber);
                        double distance = firstStation.Coordinates.GetDistanceTo(lastStation.Coordinates);
                        try
                        {
                            AddPairStations(new PairStations() { FirstStationNumber = firstStation.StationId, LastStationNumber = lastStation.StationId, Distance = distance, Time = new TimeSpan((int)(distance / 40.0), (int)((distance % 40.0) / (40.0 / 60.0)), (int)(((distance % 40.0) % (40.0 / 60.0)) / (40.0 / 3600.0))) });
                        }
                        catch (BOBadPairStationException e)
                        { };
                        deleted = true;
                        continue;
                    }

                }
                if (deleted)
                {
                    lineStations[j].Index--;
                    UpdateLineStation(lineStations[j]);
                }
                if (first)
                {
                    first = false;
                    myLine.FirstStation = lineStations[j].StationNumber;
                    UpdateLine(myLine);
                }
            }
            if (lastIndex != 0)
            {
                myLine.LastStation = lineStations.FirstOrDefault(x => x.Index == lastIndex - 1).StationNumber;
                UpdateLine(myLine);
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
                busInTravelDO = dal.GetBusInTravel(license, line, departureTime);
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

        public void AddLineDeparting(LineDeparting tmpLineDeparting)
        {
            if (tmpLineDeparting.StartTime >= tmpLineDeparting.StopTime)
            {
                throw new BOStopTimeException("Error : Start Time and Stop Time need to be on the same day !");
            }
            if (tmpLineDeparting.Frequency > tmpLineDeparting.StopTime - tmpLineDeparting.StartTime)
            {
                throw new BOFrequencyException("Error : Frequency must be less or equal to interval !");
            }
            IEnumerable<LineDeparting> myLineDeparting = GetAllLineDepartingBy(x => x.LineNumber == tmpLineDeparting.LineNumber).OrderBy(x => x.StartTime);
            foreach (var item in myLineDeparting)
            {
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
                    //AddLineDeparting(new LineDeparting() { StartTime = item.StartTime, StopTime = tmpLineDeparting.StartTime, Frequency = item.Frequency, LineNumber = item.LineNumber });      
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
        PairStations PairStationsDoBOAdapter(DO.PairStations pairStationsDO)
        {
            PairStations pairStationsBO = new PairStations();
            pairStationsDO.CopyPropertiesTo(pairStationsBO);
            return pairStationsBO;
        }

        public IEnumerable<PairStations> GetAllPairStations()
        {
            return from pairStations in dal.GetAllPairStations()
                   select PairStationsDoBOAdapter(pairStations);
        }

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
        UserTrip UserTripDoBOAdapter(DO.UserTrip userTripDO)
        {
            UserTrip userTripBO = new UserTrip();
            userTripDO.CopyPropertiesTo(userTripBO);
            return userTripBO;
        }

        public IEnumerable<UserTrip> GetAllUserTrip()
        {
            return from userTrip in dal.GetAllUserTrip()
                   select UserTripDoBOAdapter(userTrip);
        }

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
        public IEnumerable<LineStationToShow> GetAllStationsOfLine(int lineNumber)
        {
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

        public void UpdateDistanceAndTime(LineStationToShow myStation, int line)
        {
            try
            {
                ;
                foreach (var item in GetAllStationsOfLine(myStation.lineNumber))
                {
                    if (item.Index == myStation.Index + 1)
                    {
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

        public IEnumerable<StationToAdd> GetAllStationsToAdd(int lineNumber)
        {
            IEnumerable<StationToAdd> stationsToAdd = from station in GetAllStations()
                                                      orderby station.StationId
                                                      let stationToAdd = station.CopyPropertiesToNew(typeof(StationToAdd)) as StationToAdd
                                                      select stationToAdd;
            List<StationToAdd> stations = stationsToAdd.ToList();
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

        public StationToShow getStationToShow(int stationNumber)
        {
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
    }
}