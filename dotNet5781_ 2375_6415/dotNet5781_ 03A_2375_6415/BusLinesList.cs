using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03A_2375_6415
{

    /// <summary>
    /// Class that implement a list of lines of bus
    /// </summary>
    /// 
    class BusLinesList : IEnumerable
    {
        public List<Line> myList = new List<Line> { };
        /// <summary>
        /// Add line to lines list
        /// </summary>
        /// <param name="lineNumber">Number of the line</param>
        /// <param name="tmpArea">Area of the line</param>
        /// <returns></returns>
        public void AddLine(int lineNumber, Area tmpArea = Area.General)
        {
            int i = 0;
            foreach (var Line in myList) //goes over all lines
            {
                if (Line.LineNumber == lineNumber) //if same number
                {
                    i++; //sums the number of lines that have the same number than the one we want to add
                }
            }
            if (i < 2) //if there is less than 2 lines with this number
            {
                Line tmpLine = new Line(lineNumber, tmpArea); //create new line
                myList.Add(tmpLine); //add line to list
            }
            else //if there is already 2 lines
            {
                throw new ArgumentException("Line already exists, cannot build more than 2 routes for the same line !!");
            }
        }
        /// <summary>
        /// Add station to a line in the list
        /// </summary>
        /// <param name="lineNumber">Line number</param>
        /// <param name="predStation">key of previous station in line (if first station enter 0)</param>
        /// <param name="stationNumber">number of station</param>
        /// <param name="stationAddress">Adress of the station</param>
        public void AddStation(int lineNumber, int predStation, int stationNumber, string stationAddress = "")
        {
            BusLinesList tmpList = this[lineNumber]; //get all lines with lineNumber
            if (tmpList == null) ///if no line 
            {
                throw new NotFoundException("Bus line not found !!");
            }
            int lineChoice; //if there is two line user choose the one to add to
            if (tmpList.myList.Count == 2)
            {
                Console.WriteLine("First way - 1 / second way - 2");
                lineChoice = MainWindow.getIntInput();
                tmpList.myList[lineChoice - 1].AddStation(predStation, stationNumber); //adds station to line choosen
            }
            else
            {
                tmpList.myList[0].AddStation(predStation, stationNumber); //adds station to line chosen
            }
        }
        /// <summary>
        /// Deletes line in lines list
        /// </summary>
        /// <param name="tmpLine">Number of the line to delete</param>
        public void DeleteLine(int tmpLine)
        {
            int i = 0;
            for (; i < myList.Count; i++) //goes over all lines
            {
                if (myList[i].LineNumber == tmpLine) //if this line has the number to delete
                {
                    myList.RemoveAt(i); //remove this line
                    break; //end
                }
            }
            if (i == myList.Count) //if no line was deleted ( the line didn't exist)
            {
                throw new NotFoundException("Cannot delete line that doesn't exists !!");
            }
        }
        /// <summary>
        /// Checks if station already exists in one of the lines and returns it
        /// </summary>
        /// <param name="number">Number of the station we look for</param>
        /// <returns>Return the station if found else return a station with number -1</returns>
        public BusLineStop FindStop(int number)
        {
            BusLineStop tmpStop = new BusLineStop(); //creates new station
            foreach (Line item in myList) //goes over all lines in list
            {
                tmpStop = item.FindStation(number); //checks if station is in line using function, if yes get it
                if (tmpStop.BusStationKey != -1) //if station is in line
                {
                    return tmpStop;
                }
            }
            return tmpStop;//returns a station with number -1 (not found)
        }
        /// <summary>
        /// Returns list of lines in wich a specific station exists
        /// </summary>
        /// <param name="tmpStation">Number of the station we look for</param>
        /// <returns>returns the subList of lines </returns>
        public List<Line> FindStation(int tmpStation)
        {
            List<Line> subList = new List<Line> { }; //creates new list of lines
            foreach (Line line in myList) //goes over all lines in list
            {
                foreach (BusLineStop busStation in line) //goes over all stations in line
                {
                    if (tmpStation == busStation.BusStationKey) //if station is in line
                    {
                        subList.Add(line); //adds line to the subList
                        break;
                    }
                }
            }
            if (subList.Count == 0) //if subList is empty (no bus line was found)
            {
                throw new NotFoundException("cannot create list of lines, no line deserves that station !!");
            }
            return subList;
        }
        /// <summary>
        /// Creates sublist of Bus lines composed by all routes between 2 stops
        /// </summary>
        /// <param name="stn1">Source's station</param>
        /// <param name="stn2">Destination's station</param>
        /// <returns>returns subList of bus lines</returns>
        public BusLinesList CreateSubList(int stn1, int stn2)
        {
            BusLinesList subList = new BusLinesList();
            foreach (Line line in myList) //goes over all the lines
            {
                Line tmpLine = line.SubLine(stn1, stn2); //gets a subLine if this line has a route from stn1 to stn2
                if (tmpLine != null) //if the subLine exists
                {
                    tmpLine.LineNumber = line.LineNumber; //gets the number of the line from wich this subline is taken
                    subList.myList.Add(tmpLine); //adds subLine to subList
                }
            }
            return subList;
        }
        /// <summary>
        /// Sorts a list of lines according to the default comparer (Time of travel from begin to end)
        /// </summary>
        public void SortList()
        {
            myList.Sort();
        }
        /// <summary>
        /// Implementation of indexer interface
        /// </summary>
        /// <param name="tmpLine">Gets number of line</param>
        /// <returns>Returns List of lines of the number entered</returns>
        public BusLinesList this[int tmpLine]
        {
            get
            {
                BusLinesList tmpList = new BusLinesList(); //creates tmpList of lines to return
                for (int i = 0; i < myList.Count; i++) //searches for line in all array
                {
                    if (myList[i].LineNumber == tmpLine)
                    {
                        tmpList.myList.Add(myList[i]); //adds line in the List
                    }
                }
                if (tmpList.myList.Count == 0)
                {
                    tmpList = null;
                }
                return tmpList;
            }
        }
        /// <summary>
        /// Implementation of enumerator interface
        /// </summary>
        /// <returns>Enumerator of the list</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return myList.GetEnumerator();
        }
    }

}
