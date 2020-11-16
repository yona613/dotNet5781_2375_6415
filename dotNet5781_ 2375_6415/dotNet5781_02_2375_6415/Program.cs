using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2375_6415
{
    using static Math;
    /// <summary>
    /// Assigning the bus line to a specific area from a defined area list
    /// or be cross-areas (general)/// </summary>
    enum Area { General, North, South, Center, Jerusalem };

    class Program
    {
        enum Choice { ADD = 1, DELETE, FIND, PRINT, EXIT }

        static void Main(string[] args)
        {
            BusLinesList myList = new BusLinesList(); //creates a list of lines
            for (int i = 0; i < 10; i++) //adds lines
            {
                myList.AddLine(i + 1);
            }

            for (int i = 0; i < 10; i++) //adds stations
            {
                int k = 0;
                int tmp = 0;
                for (int j = 0; j < 20; j++)
                {
                    tmp = myRandom.r.Next(1, 41);
                    try
                    {
                        myList.AddStation(i + 1,k,tmp);
                    }
                    // catches arr empties for the boot continuity of stations and lines and to avoid duplication
                    catch (ArgumentOutOfRangeException ex)
                    {
                    }
                    catch (NotFoundException ex1)
                    {
                    }
                    catch (ArgumentException ex2)
                    {
                    }
                    k = tmp;
                }
            }
            int myChoice; //gets choice of user
            int innerChoice; //gets innerchoice of user
            do
            {
                Console.WriteLine("Enter your choice :");
                Console.WriteLine("Add - 1 / Delete - 2 / Find - 3 / Print - 4 / Exit - 5 ");
                myChoice = getIntInput();
                switch ((Choice)myChoice)
                {
                    case Choice.ADD:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("New line - 1 / New BusStop - 2");
                        innerChoice = getIntInput(); // read and check the input
                        if (innerChoice == 1) // adds New line
                        {
                            Console.WriteLine("Enter Number of Line :");
                            innerChoice = getIntInput(); // read and check the input
                            try
                            {
                                myList.AddLine(innerChoice);
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                            innerChoice = 1;
                        }
                        if (innerChoice == 2) // adds New bus stop
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum; // num of the line
                            tmpNum = getIntInput();
                            Console.WriteLine("Enter number of pred Stop in Line :");
                            int predStation = getIntInput(); // Location of the station on the list

                            Console.WriteLine("Enter number of station :");
                            int stnNum = getIntInput(); // num of station
                            try
                            {
                                myList.AddStation(tmpNum, predStation, stnNum);
                            }
                            catch (ArgumentOutOfRangeException ex) // wrong index
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                            catch (NotFoundException ex1) // no line
                            {
                                Console.WriteLine($" ERROR : {ex1.ToString()}");
                            }
                            catch (ArgumentException ex2) // bus stop already exist
                            {
                                Console.WriteLine($" ERROR : {ex2.ToString()}");
                            }
                        }
                        break;
                    case Choice.DELETE:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Delete line - 1 / Delete BusStop - 2");
                        innerChoice = getIntInput();
                        if (innerChoice == 1)
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum = getIntInput();
                            try
                            {
                                myList.DeleteLine(tmpNum);
                            }
                            catch (NotFoundException ex) // wrong line
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                        }
                        if (innerChoice == 2) // Delete BusStop
                        {
                            Console.WriteLine("Enter Bus Line :");
                            int tmpNum = getIntInput();
                            bool flag = false; // true = we find the line
                            foreach (Line line in myList)
                            {
                                if (line.LineNumber == tmpNum)
                                {
                                    flag = true;
                                    Console.WriteLine("Enter number of bus stop in line :");
                                    tmpNum = getIntInput();
                                    try
                                    {
                                        line.DeleteStation(tmpNum);
                                    }
                                    catch (NotFoundException ex) // no exist bus stop in this line
                                    {
                                        Console.WriteLine($" ERROR : {ex.ToString()}");
                                    }
                                    break;
                                }
                            }
                            try
                            {
                                if (!flag) // we didn't find the line
                                    throw new NotFoundException("Bus line not found !!");
                            }
                            catch (NotFoundException ex)
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                        }
                        break;
                    case Choice.FIND:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Find Lines for station - 1 / Find bus Line - 2");
                        innerChoice = getIntInput();
                        if (innerChoice == 1)
                        {
                            Console.WriteLine("Enter Bus station :");
                            int tmpNum = getIntInput();
                            try
                            {
                                List<Line> subList = myList.FindStation(tmpNum); // A new list of lines that pass through the station

                                foreach (Line item in subList)
                                {
                                    Console.WriteLine($"Line #{item.LineNumber}");
                                }
                            }
                            catch (NotFoundException ex) // wrong bus stop
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                        }
                        if (innerChoice == 2) // Finding the fast route between two stations
                        {
                            Console.WriteLine("Enter first station :");
                            int stn1 = getIntInput();
                            Console.WriteLine("Enter second station :");
                            int stn2 = getIntInput();
                            // A new list of lines that pass between these 2 stations
                            BusLinesList subList = myList.CreateSubList(stn1, stn2);
                            try
                            {
                                // Sort the list according to a criterion of total travel time
                                subList.SortList();
                                foreach (Line item in subList)
                                {
                                    Console.WriteLine($"Line #{item.LineNumber}");
                                }
                            }
                            catch (NotFoundException ex) // wrong station/stations
                            {
                                Console.WriteLine($" ERROR : {ex.ToString()}");
                            }
                        }
                        break;
                    case Choice.PRINT:
                        Console.WriteLine("Enter your choice :");
                        Console.WriteLine("Print all lines - 1 / Print all stops - 2");
                        innerChoice = getIntInput();
                        if (innerChoice == 1) // Print all lines
                        {
                            foreach (Line item in myList)
                            {
                                Console.WriteLine($"Line #{item.LineNumber}");
                            }
                        }
                        if (innerChoice == 2) // Print all stops
                        {
                            List<int> stationList = new List<int> { }; // List of station numbers
                            foreach (Line line in myList) // For each line from the list of existing lines
                            {
                                foreach (BusLineStop stop in line)
                                {
                                    bool flag = true; // True if the bus stop is not yet in the list of printable bus stops
                                    foreach (int num in stationList) // For each station where the line stops
                                    {
                                        if (num == stop.BusStationKey) // We have already printed this station, it is already on the list
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (flag == true) // the bus stop is not yet in the list of printable bus stops
                                    {
                                        List<Line> subList = myList.FindStation(stop.BusStationKey); // List of lines stopping at this station
                                        Console.WriteLine(stop.ToString()); // Prints the station
                                        foreach (Line item in subList) // Prints the lines that stop at the station
                                        {
                                            Console.Write($"Line #{item.LineNumber} / ");
                                        }
                                        Console.WriteLine(); // new line

                                        // Adds the station to the list of stations we have printed
                                        stationList.Add(stop.BusStationKey);
                                    }
                                }
                            }
                        }
                        break;
                    case Choice.EXIT:
                        Console.WriteLine("GoodBye");
                        break;
                    default:
                        break;
                }
            } while (myChoice != 5);
        }
        /// <summary>
        /// Gets input from user untill it is integer and returns it in integer form
        /// Throws exception if input is not an integer
        /// </summary>
        /// <returns>input casted into integer</returns>
        static public int getIntInput()
        {
            string tmpString;
            int tmpNum;
            do
            {
                tmpString = Console.ReadLine(); // reads the input
                try
                {
                    if (!int.TryParse(tmpString, out tmpNum)) // didn't succeed to switch the input to integer
                    {
                        throw new InvalidCastException("Invalid input !!");
                    }
                    break;
                }
                catch (InvalidCastException ex)
                {
                    Console.WriteLine($" ERROR : {ex.ToString()}");
                }
            } while (true);
            return tmpNum;
        }
    }
}