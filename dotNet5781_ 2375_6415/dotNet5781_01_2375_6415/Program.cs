using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dotNet5781_01_2375_6415
{ 
    /// <summary>
    /// Struct used to enter dates 
    /// </summary>
    struct Date
    {
        public int year;
        public int month;
        public int day;
    }
    class Bus
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Bus()
        {
            startDate.year = 0;
            startDate.month = 0;
            startDate.day = 0;
            license = 0;
            oil = 1200;
            kilometrage = 0;
            kmOfTest = 0;
            DateTime currentDate = DateTime.Now; //to get today's date
            //gets test of today's date
            dateOfTest.year = currentDate.Year;
            dateOfTest.month = currentDate.Month;
            dateOfTest.day = currentDate.Day;
        }

        /// <summary>
        /// Field that get the entry of service's date
        /// </summary>
        private Date startDate;

        /// <summary>
        /// Getter for license's date
        /// </summary>
        /// <returns> Returns date of entry in service</returns>
        public Date getStartDate()
        {
            return startDate;
        }

        /// <summary>
        /// Setter for license's date
        /// </summary>
        public void SetStartDate()
        {
            Console.WriteLine("Enter the date of entering service :");
            Console.WriteLine("year :");
            bool flag = false; //checks is it an acceptable date
            do
            {
                string myDate = Console.ReadLine(); //myDate gets user's entry
                flag = int.TryParse(myDate, out startDate.year);
                if (startDate.year < 0) //year<0 not acceptable date
                {
                    flag = false;
                }
            } while (!flag);
            Console.WriteLine("month :");
            do
            {
                string myDate = Console.ReadLine(); //myDate gets user's entry
                flag = int.TryParse(myDate, out startDate.month);
                if ((startDate.month < 1) || (startDate.month > 12)) // 0 < month < 13
                {
                    flag = false;
                }
            } while (!flag);
            Console.WriteLine("day :");
            do
            {
                string myDate = Console.ReadLine(); //myDate gets user's entry
                flag = int.TryParse(myDate, out startDate.day);
                if ((startDate.day < 1) || (startDate.day > 31)) // 0 < day < 32
                {
                    flag = false;
                }
            } while (!flag);
        }

        /// <summary>
        /// Field that gets license number
        /// </summary>
        private int license;

        /// <summary>
        /// getter for license number using property
        /// </summary>
        public int License
        {
            get { return license; }
        }

        /// <summary>
        /// Setter for License
        /// </summary>
        public void SetLicense()
        {
            Console.WriteLine("Enter license number :");
            if (startDate.year < 2018) //checks format of license 
            {
                bool flag = false;
                while (flag == false)
                {
                    string tmp = Console.ReadLine(); //tmp gets user's entry
                    flag = int.TryParse(tmp, out license); //flag checks the conversion
                    if (flag)
                    {
                        if ((license < 1000000) || (license > 9999999)) //checks that license is in the range
                        {
                            flag = false; //if out of range, get another license
                        }
                    }                  
                }
            }
            else
            {
                bool flag = false;
                while (flag == false)
                {
                    string tmp = Console.ReadLine(); //tmp gets user's entry
                    flag = int.TryParse(tmp, out license); //flag checks the conversion
                    if (flag)
                    {
                        if ((license < 10000000) || (license > 99999999))//checks that license is in the range
                        {
                            flag = false; //if out of range, get another license
                        }
                    }
                   
                }
            }
        }


        /// <summary>
        /// Field thats get Km that rest until next fueling
        /// </summary>
        private int oil;

        /// <summary>
        /// getter and setter for oil
        /// </summary>
        public int Oil
        {
            get { return oil; }
            set { oil = value; }
        }

        /// <summary>
        /// Field that gets Kilometrage of the bus
        /// </summary>
        private int kilometrage;

        /// <summary>
        /// getter and setter for Kilometrage
        /// </summary>
        public int Kilometrage
        {
            get { return kilometrage; }
            set { kilometrage = value; }
        }

        /// <summary>
        /// Field that gets date of last test
        /// </summary>
        private Date dateOfTest;

        /// <summary>
        /// getter for date field
        /// </summary>
        /// <returns> returns last test's date</returns>
        public Date getDateOfTest()
        {
            return dateOfTest;
        }
        
        /// <summary>
        /// setter for last test
        /// </summary>
        /// <param name="test"> gets date of last test to be updated</param>
        public void setDateOfTest(Date test)
        {
            dateOfTest = test;
        }

        /// <summary>
        /// Field that gets Kilometrage when last test was effectued
        /// </summary>
        private int kmOfTest;

        /// <summary>
        /// getter and setter for kmOfTest
        /// </summary>
        public int KmOfTest
        {
            get { return kmOfTest; }
            set { kmOfTest = value; }
        }

        /// <summary>
        /// Function that gets number of Km to travel, checks if bus can travel
        /// and updates the relevant fields
        /// </summary>
        /// <param name="Km">number of Km to travel</param>
        public void Travel(int Km)
        { 
            DateTime currentDate = DateTime.Now; //gets current date from PC
            bool checkTest = true; //gets false if needs test
            if ((currentDate.Year - startDate.year) >= 2) //if difference > 2 then needs new test
            {
                checkTest = false;
            }
            else
            {
                if ((currentDate.Year - startDate.year) == 1) // if difference == 1 then check months
                {
                    if ((currentDate.Month > startDate.month)) // if current month > start month then test is needed
                    {
                        checkTest = false;
                    }
                    else if ((currentDate.Month == startDate.month) && (currentDate.Day >= startDate.day)) //else checks day, if current day >= start day then test is needed
                    {
                        checkTest = false;
                    }
                }
            }
            if (checkTest) //if doesn't need test by date then checks test by Km and checks fuel
            {
                if (((Km - Oil) < 0) && ((Kilometrage + Km - KmOfTest) < 20000)) //first check is : is there enough oil / second check is : is there enough Km until next test 
                {
                    //if can travel
                    Oil -= Km; //update oil
                    Kilometrage += Km; //update Kilometrage
                    Console.WriteLine($"Bus traveled : {Km} Km"); //prints how many Kms the bus travelled
                }
                else //if cannot travel because Kms
                {
                    Console.WriteLine("Overpass the allowed Kilometrage , cannot travel");
                }
            }
            else //if cannot travel because test
            {
                Console.WriteLine("Cannot travel, test is needed");
            }
        }

        /// <summary>
        /// Updates fuel or Test, by choice of the user
        /// </summary>
        public void FuelOrTest()
        {
            Console.WriteLine("To fuel the bus enter : fuel  /  To go to test enter : test");
            string choice; // gets user's choice
            do
            {
                choice = Console.ReadLine();
            } while ((choice != "fuel") && (choice != "test")); //checks that user's entry is acceptable
            if (choice == "fuel")
            {
                Oil = 1200; //updates oil
            }
            else //updates test
            {
                DateTime currentDate = DateTime.Now; //gets current date from PC
                //updates all relevant fields
                dateOfTest.year = currentDate.Year;
                dateOfTest.month = currentDate.Month;
                dateOfTest.day = currentDate.Day;
                KmOfTest = Kilometrage;
            }
        }

        /// <summary>
        /// Prints buse's License and Kilometrage from latest test
        /// </summary>
        public void Print()
        {
            if (startDate.year < 2018)
            {
                //prints license in format XX-XXX-XX
                Console.WriteLine($"License : {License / 100000}-{(License % 100000) / 100}-{License % 100}");
            }
            else
            {
                //prints license in format XXX-XX-XXX
                Console.WriteLine($"License : {License / 100000}-{(License % 100000) / 1000}-{License % 1000}");
            }
            Console.WriteLine($"Kilometrage : {Kilometrage - KmOfTest} "); //prints kilometrage from latest test
            Console.WriteLine();
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            string choice; //gets user's choice
            List<Bus> busList = new List<Bus> { }; //list of buses 
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
                                busList[i].Travel(r.Next(1200)); //operate travel function on the bus using a random number of Kms to travel (bus cannot travel more than 1200 Km)
                                break;
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
                        flag = false; //checks tha bus is in the list
                        for (int i = 0; i < busList.Count; i++) //goes over the whole list
                        {
                            if (busList[i].License == tmpLicense) // if bus is found
                            {
                                flag = true; //found
                                busList[i].FuelOrTest(); //fuel or test that bus using method of buse's class       
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
        static Random r = new Random(DateTime.Now.Millisecond);  
    }
}
