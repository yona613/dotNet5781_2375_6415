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
            oil = 0;
            kilometrage = 0;
            kmOfTest = 0;
            dateOfTest.year = 0;
            dateOfTest.month = 0;
            dateOfTest.day = 0;
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="tmpDate">Date of entry in service</param>
        /// <param name="tmpLicense">License number</param>
        public Bus(Date tmpDate, int tmpLicense)
        {
            startDate = tmpDate;
            license = tmpLicense;
            oil = 1200; // enters with full tank
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
        /// Getter for entry of service's date
        /// </summary>
        /// <returns> Returns date of entry in service</returns>
        public Date getStartDate()
        {
            return startDate;
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

    }
    class Program
    {
        static void Main(string[] args)
        {
            string choice;
            List<Bus> busList = new List<Bus> { };
            do
            {
                Console.WriteLine("Choose an action :");
                Console.WriteLine("add  /  travel  /  fuel or test  /  print  /  exit" );
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "add":
                        Date tmpDate = GetDateMain();
                        int license = GetLicense(tmpDate.year);
                        Bus tmpBus = new Bus(tmpDate,license);
                        busList.Add(tmpBus);
                        break;
                    case "travel":
                        int tmpLicense;
                        string tmp;
                        Console.WriteLine("Enter the buse's license :");
                        do
                        {
                            tmp = Console.ReadLine();
                        } while (!(int.TryParse(tmp, out tmpLicense)));
                        bool flag = false;
                        for (int i = 0; i < busList.Count; i++)
                        {
                            if (busList[i].License == tmpLicense)
                            {
                                flag = true;
                                int travel = r.Next(1200);
                                tmpDate = busList[i].getDateOfTest();
                                DateTime currentDate = DateTime.Now;
                                bool checkTest = true;
                                if ((currentDate.Year-tmpDate.year) >= 2)
                                {
                                    checkTest = false;
                                }
                                else
                                {
                                    if ((currentDate.Year - tmpDate.year) == 1) 
                                    {
                                        if ((currentDate.Month > tmpDate.month))
                                        {
                                            checkTest = false;
                                        }
                                        else if ((currentDate.Month == tmpDate.month) && (currentDate.Day >= tmpDate.day))
                                        {
                                            checkTest = false;
                                        }
                                    }                                                                
                                }
                                if (checkTest)
                                {
                                    if (((travel - busList[i].Oil) < 0) && ((busList[i].Kilometrage + travel - busList[i].KmOfTest) < 20000))
                                    {
                                        busList[i].Oil -= travel;
                                        busList[i].Kilometrage += travel;
                                        Console.WriteLine($"Bus traveled : {travel} Km");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Overpass the allowed Kilometrage , cannot travel");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Cannot travel, test is needed");
                                }
                                break;
                            }
                        }
                        if (!flag)
                        {
                            Console.WriteLine("Cannot find this License in fleet");
                        }
                        break;
                    case "fuel or test":
                        Console.WriteLine("Enter the buse's license :");
                        do
                        {
                            tmp = Console.ReadLine();
                        } while (!(int.TryParse(tmp, out tmpLicense)));
                        flag = false;
                        for (int i = 0; i < busList.Count; i++)
                        {
                            if (busList[i].License == tmpLicense)
                            {
                                flag = true;
                                Console.WriteLine("To fuel the bus enter : fuel  /  To go to test enter : test");
                                string choice1;
                                do
                                {
                                    choice1 = Console.ReadLine();
                                } while ((choice1 != "fuel") && (choice1 != "test"));                          
                                if (choice1 =="fuel")
                                {
                                    busList[i].Oil = 1200;
                                }
                                else
                                {
                                    DateTime currentDate = DateTime.Now;
                                    Date test;
                                    test.year = currentDate.Year;
                                    test.month = currentDate.Month;
                                    test.day = currentDate.Day;
                                    busList[i].setDateOfTest(test);
                                    busList[i].KmOfTest = busList[i].Kilometrage;
                                }                             
                                break;
                            }
                        }
                        if (!flag)
                        {
                            Console.WriteLine("Cannot find this License in fleet");
                        }
                        break;
                    case "print":
                        for (int i = 0; i < busList.Count; i++)
                        {
                            tmpDate = busList[i].getStartDate();
                            if (tmpDate.year < 2018)
                            {
                                Console.WriteLine($"License : {busList[i].License / 100000}-{(busList[i].License % 100000)/100}-{busList[i].License % 100}");
                            }
                            else
                            {
                                Console.WriteLine($"License : {busList[i].License / 100000}-{(busList[i].License % 100000) / 1000}-{busList[i].License % 1000}");
                            }
                            Console.WriteLine($"Kilometrage : {busList[i].Kilometrage - busList[i].KmOfTest}");
                            Console.WriteLine();
                        }
                        break;
                    case "exit":
                        Console.WriteLine("GoodBye");
                        break;
                    default:
                        break;
                }
            } while (choice != "exit");
        }

        /// <summary>
        /// Random number to be used in the whole namespace
        /// </summary>
        static Random r = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Fonction that gets Date of entering in service from the user and checks that the date is good
        /// </summary>
        /// <returns> Returns Date of entering in service</returns>
        private static Date GetDateMain()
        {
            Date tmpDate = new Date();
            Console.WriteLine("Enter the date of entering service :");
            Console.WriteLine("year :");
            bool flag = false;
            do
            {
                string myDate = Console.ReadLine();
                flag = int.TryParse(myDate, out tmpDate.year);
                if (tmpDate.year<0)
                {
                    flag = false;
                }
            } while (!flag);
            Console.WriteLine("month :");
            do
            {
                string myDate = Console.ReadLine();
                flag = int.TryParse(myDate, out tmpDate.month);
                if ((tmpDate.month < 1)||(tmpDate.month > 12) )
                {
                    flag = false;
                }
            } while (!flag);
            Console.WriteLine("day :");
            do
            {
                string myDate = Console.ReadLine();
                flag = int.TryParse(myDate, out tmpDate.day);
                if ((tmpDate.day < 1) || (tmpDate.day > 31))
                {
                    flag = false;
                }
            } while (!flag);
            return tmpDate;  
        }

        /// <summary>
        /// Fonction that gets License number until a good number is entered
        /// </summary>
        /// <param name="year"> Year in whitch bus was licensed </param>
        /// <param name="license"> License number </param>
        private static int GetLicense(int year)
        {
            int license = 0;
            Console.WriteLine("Enter license number :");
            if (year < 2018)
            {
                bool flag1 = false;
                bool flag2 = false;
                while ((flag1 == false) || (flag2 == false))
                {
                    string tmp = Console.ReadLine();
                    flag1 = int.TryParse(tmp, out license);
                    if ((license >= 1000000) && (license <= 9999999))
                    {
                        flag2 = true;
                    }
                }
            }
            else
            {
                bool flag1 = false;
                bool flag2 = false;
                while ((flag1 == false) || (flag2 == false))
                {
                    string tmp = Console.ReadLine();
                    flag1 = int.TryParse(tmp, out license);
                    if ((license >= 10000000) && (license <= 99999999))
                    {
                        flag2 = true;
                    }
                }
            }
            return license;
        }
    }
}
