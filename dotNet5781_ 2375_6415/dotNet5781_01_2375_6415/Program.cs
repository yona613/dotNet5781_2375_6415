using System;
using System.Collections.Generic;

namespace dotNet5781_01_2375_6415
{
    public class Program
    {
        static void Main(string[] args)
        {

            string choice; //gets user's choice
            List<Bus> busList = CreateBusList();
            do
            {
                Console.WriteLine("Choose an action :");
                Console.WriteLine("add  /  travel  /  fuel or test  /  print  /  exit" );
                choice = Console.ReadLine(); //enter a choice
                switch (choice)
                {
                    case "add": //add a new bus in list
                        Bus tmpBus = new Bus(); //create new bus
                        tmpBus.SetStartDate(); //updates start date of new bus
                        tmpBus.SetLicense(); //updates license of new bus
                        busList.Add(tmpBus);  //adds bus in list
                        break;
                    case "travel": //one buse's travel
                        int tmpLicense; //gets license from user
                        string tmp; //gets user's entry
                        Console.WriteLine("Enter the buse's license :");
                        do
                        {
                            tmp = Console.ReadLine();
                        } while (!(int.TryParse(tmp, out tmpLicense)));
                        bool flag = false; //checks if buse is in list
                        for (int i = 0; i < busList.Count; i++) //goes over the whole list
                        {
                            if (busList[i].License == tmpLicense) //if bus found
                            {
                                flag = true; //found
                                try
                                {
                                    busList[i].Travel(r.Next(1200)); //operate travel function on the bus using a random number of Kms to travel (bus cannot travel more than 1200 Km)
                                    
                                }
                                catch (MyTravelException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    break;
                                }
                                
                            }
                        }
                        if (!flag) //if bus wasn't found
                        {
                            Console.WriteLine("Cannot find this License in fleet");
                        }
                        break;
                    case "fuel or test": //fuels or testes a bus according to user's choice
                        Console.WriteLine("Enter the buse's license :");
                        do
                        {
                            tmp = Console.ReadLine();
                        } while (!(int.TryParse(tmp, out tmpLicense)));
                        flag = false; //checks the bus is in the list
                        for (int i = 0; i < busList.Count; i++) //goes over the whole list
                        {
                            if (busList[i].License == tmpLicense) // if bus is found
                            {
                                flag = true; //found
                                Console.WriteLine("To fuel the bus enter : fuel  /  To go to test enter : test");
                                string innerChoice; // gets user's choice
                                do
                                {
                                    innerChoice = Console.ReadLine();
                                } while ((choice != "fuel") && (choice != "test")); //checks that user's entry is acceptable
                                if (innerChoice == "fuel")
                                {
                                    busList[i].Fuel();
                                }
                                else
                                {
                                    busList[i].Test();
                                }                                    
                                break;
                            }
                        }
                        if (!flag) //if bus wasn't found
                        {
                            Console.WriteLine("Cannot find this License in fleet");
                        }
                        break;
                    case "print": //prints all buses in list ( license and kilometrage from latest test)
                        for (int i = 0; i < busList.Count; i++) //goes over the whole list
                        {
                            busList[i].Print(); //prints that bus using fonction of the buse's class
                        }
                        break;
                    case "exit": //end of switch
                        Console.WriteLine("GoodBye");
                        break;
                    default:
                        break;
                }
            } while (choice != "exit");
            Console.ReadLine();
        }

        /// <summary>
        /// Random number to be used in the whole namespace
        /// </summary>
        public static Random r = new Random(DateTime.Now.Millisecond);

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

        static public List<Bus> CreateBusList()
        {
            List<Bus> busList = new List<Bus> { }; //list of buses 
            for (int i = 0; i < 7; i++)
            {
                DateTime tmpDate = new DateTime(2000 + r.Next(0, 19), r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                DateTime tmpTest = new DateTime(2020, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
                if (tmpDate.Year < 2018)
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus(tmpDate, r.Next(1000000, 10000000), r.Next(0, 1201),tmpKm , r.Next(0,18000), tmpTest);
                    busList.Add(tmpBus);
                }
                else
                {
                    int tmpKm = r.Next(20500, 200000);
                    Bus tmpBus = new Bus(tmpDate, r.Next(10000000, 100000000), r.Next(0, 1201), tmpKm, r.Next(0, 18000), tmpTest);
                    busList.Add(tmpBus);
                }
            }
            //Bus that next test time is passed
            DateTime tmpDate1 = new DateTime(2015, r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            DateTime tmpTest1 = new DateTime(2018, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            int tmpKm1 = r.Next(20500, 200000);
            Bus tmpBus1 = new Bus(tmpDate1, r.Next(1000000, 10000000), r.Next(0, 1201), tmpKm1, r.Next(0, 18000), tmpTest1);
            busList.Add(tmpBus1);

            //bus close to test because of km
            tmpDate1 = new DateTime(2018, r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus(tmpDate1, r.Next(10000000, 100000000), r.Next(0, 1201), tmpKm1, r.Next(19900, 19998), tmpTest1);
            busList.Add(tmpBus1);

            //bus close to refuel
            tmpDate1 = new DateTime(2019, r.Next(1, 13), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpTest1 = new DateTime(2020, r.Next(1, 10), r.Next(1, 30), r.Next(1, 13), r.Next(0, 60), r.Next(0, 60));
            tmpKm1 = r.Next(20500, 200000);
            tmpBus1 = new Bus(tmpDate1, r.Next(10000000, 100000000), r.Next(0, 50), tmpKm1, r.Next(0, 18000), tmpTest1);
            busList.Add(tmpBus1);

            return busList;
        }
    }
}