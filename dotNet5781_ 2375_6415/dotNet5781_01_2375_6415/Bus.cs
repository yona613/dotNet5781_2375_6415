using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace dotNet5781_01_2375_6415
{
    public enum Status { READY , TRAVELLING , OILING , TESTING}

    public class Bus : INotifyPropertyChanged
    {

        public BackgroundWorker bw = new BackgroundWorker();


        /// <summary>
        /// Default constructor
        /// </summary>
        public Bus()
        {
            startDate = new DateTime(0, 0, 0);
            license = 0;
            oil = 1200;
            kilometrage = 0;
            kmFromTest = 0;
            //gets test of today's date
            dateOfTest = DateTime.Now; //to get today's date
        }

        public Bus(DateTime tmpStartDate, int tmpLicense, int tmpOil, int tmpKilometrage, int tmpKmTest, DateTime tmpDateOfTest)
        {
            startDate = tmpStartDate;
            license = tmpLicense;
            oil = tmpOil;
            kilometrage = tmpKilometrage;
            kmFromTest = tmpKmTest;
            dateOfTest = tmpDateOfTest;
        }


        /// <summary>
        /// Field that get the entry of service's date
        /// </summary>
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            //set { startDate = value; }
        }

        /// <summary>
        /// Getter for license's date
        /// </summary>
        /// <returns> Returns date of entry in service</returns>
        public DateTime getStartDate()
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
            int tmpInt = Program.getIntInput();
            while (tmpInt < 0)
            {
                tmpInt = Program.getIntInput();
            }
            startDate.AddYears(tmpInt);
            Console.WriteLine("month :");
            tmpInt = Program.getIntInput();
            while (tmpInt < 0 && tmpInt > 12)
            {
                tmpInt = Program.getIntInput();
            }
            startDate.AddMonths(tmpInt);
            Console.WriteLine("day :");          
            tmpInt = Program.getIntInput();
            while (tmpInt < 0 && tmpInt > 30)
            {
                tmpInt = Program.getIntInput();
            }
            startDate.AddDays(tmpInt);
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
            set { license = value; }
        }

        public string License1
        {
            get {
                if (startDate.Year < 2018)
                {
                    return (License / 100000).ToString().PadLeft(2, '0') + "-" + ((License % 100000) / 100).ToString().PadLeft(3, '0') + "-" + (License % 100).ToString().PadLeft(2, '0');
                }
                else
                {
                    return (License / 100000).ToString().PadLeft(3, '0') + "-" + ((License % 100000) / 1000).ToString().PadLeft(2, '0') + "-" + (License % 1000).ToString().PadLeft(3, '0');
                }
            }
        }

        /// <summary>
        /// Setter for License
        /// </summary>
        public void SetLicense()
        {
            Console.WriteLine("Enter license number :");
            if (startDate.Year < 2018) //checks format of license 
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
        /// gets status of autobus
        /// </summary>
        private Status busStatus = Status.READY;

        public Status BusStatus
        {
            get { return busStatus; }
            set {
                busStatus = value;
                OnPropertyChanged("BusStatus");
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
            set { 
                oil = value;
                OnPropertyChanged("Oil");
            }
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
        private DateTime dateOfTest;

        public DateTime DateOfTest
        {
            get { return dateOfTest; }
            //set { dateOfTest = value; }
        }

        /// <summary>
        /// getter for date field
        /// </summary>
        /// <returns> returns last test's date</returns>
        public DateTime getDateOfTest()
        {
            return dateOfTest;
        }

        /// <summary>
        /// setter for last test
        /// </summary>
        /// <param name="test"> gets date of last test to be updated</param>
        public void setDateOfTest(DateTime test)
        {
            dateOfTest = test;
        }

        /// <summary>
        /// Field that gets the km the bus has traveled since the test
        /// </summary>
        private int kmFromTest;

        /// <summary>
        /// getter and setter for kmFromTest
        /// </summary>
        public int KmFromTest
        {
            get { return kmFromTest; }
            set { 
                kmFromTest = value;
                OnPropertyChanged("KmFromTest");
            }
        }

        /// <summary>
        /// Function that gets number of Km to travel, checks if bus can travel
        /// and updates the relevant fields
        /// </summary>
        /// <param name="Km">number of Km to travel</param>
        public bool Travel(int Km)
        {
            //bw.ProgressChanged += Travel_ProgressChanged;
            DateTime currentDate = DateTime.Now; //gets current date from PC
            bool checkTest = true; //gets false if needs test
            if ((currentDate.Year - dateOfTest.Year) >= 2) //if difference > 2 then needs new test
            {
                checkTest = false;
            }
            else
            {
                if ((currentDate.Year - dateOfTest.Year) == 1) // if difference == 1 then check months
                {
                    if ((currentDate.Month > dateOfTest.Month)) // if current month > start month then test is needed
                    {
                        checkTest = false;
                    }
                    else if ((currentDate.Month == dateOfTest.Month) && (currentDate.Day >= dateOfTest.Day)) //else checks day, if current day >= start day then test is needed
                    {
                        checkTest = false;
                    }
                }
            }
            if (checkTest) //if doesn't need test by date then checks test by Km and checks fuel
            {
                if (((Km - Oil) < 0) && ((KmFromTest + Km) < 20000)) //first check is : is there enough oil / second check is : is there enough Km until next test 
                {
                    //if can travel
                    //busStatus = Status.TRAVELLING;
                    //int mySpeed = Program.r.Next(20, 51);
                    //int myTime = (Km / mySpeed * 6000 + (Km % mySpeed) * 100);
                    //counter = new TimeSpan(Km / mySpeed, (Km % mySpeed), 0);
                    //for (int i = 1; i <= (myTime / 100); i++)
                    //{
                    //    bw.ReportProgress(i);
                    //    Thread.Sleep(100);
                    //}
                    //Thread.Sleep(((int)e.Argument / Program.r.Next(20, 51)) * 6000 + ((int)e.Argument % mySpeed * 100);
                    this.Oil -= Km; //update oil
                    this.Kilometrage += Km; //update Kilometrage
                    this.KmFromTest += Km; //update Km from Test 
                    return true;
                    //this.BusStatus = Status.READY;
                    //throw new ArgumentException($"Bus traveled : {Km} Km");
                    //Console.WriteLine($"Bus traveled : {Km} Km"); //prints how many Kms the bus travelled
                }
                else //if cannot travel because Kms
                {
                    throw new ArgumentException("Overpass the allowed Kilometrage , cannot travel");
                    //Console.WriteLine("Overpass the allowed Kilometrage , cannot travel");
                }
            }
            else //if cannot travel because test
            {
                throw new ArgumentException("Cannot travel, test is needed");
                //Console.WriteLine("Cannot travel, test is needed");
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //private int progressFuel;

        //public int ProgressFuel
        //{
        //    get { return progressFuel; }
        //    set { 
        //        progressFuel = value;
        //        OnPropertyChanged("ProgressFuel");
        //    }
        //}

        //private int progressTest;

        //public int ProgressTest
        //{
        //    get { return progressTest; }
        //    set
        //    {
        //        progressTest = value;
        //        OnPropertyChanged("ProgressTest");
        //    }
        //}

        //private int progressTravel;

        //public int ProgressTravel
        //{
        //    get { return progressTravel; }
        //    set
        //    {
        //        progressTravel = value;
        //        OnPropertyChanged("ProgressTravel");
        //    }
        //}

        //private TimeSpan counter;

        //public  TimeSpan Counter
        //{
        //    get { return counter; }
        //    set { 
        //        counter = value;
        //        OnPropertyChanged("Counter");
        //    }
        //}

        //public void Refuel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    this.ProgressFuel = e.ProgressPercentage;
        //    Counter = new TimeSpan(2-(e.ProgressPercentage / 6),-((e.ProgressPercentage % 6)*10), 0);
        //}

        //public void Refuel_ProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ProgressFuel = 0;
        //    Counter = new TimeSpan(0,0,0);
        //    OnPropertyChanged("Oil");
        //    OnPropertyChanged("BusStatus");
        //}

        /// <summary>
        /// Updates fuel
        /// </summary>
        public void Fuel()
        {
            oil = 1200;
        }

        //public void Refuel(object sender, DoWorkEventArgs e)
        //{
        //    BusStatus = Status.OILING;
        //    this.Oil = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        (sender as BackgroundWorker).ReportProgress(i);
        //        this.Oil = i*100;
        //        Thread.Sleep(1000);
        //    }
        //    this.Oil = 1200;
        //    BusStatus = Status.READY;
        //}


        /// <summary>
        /// updates test
        /// </summary>
        public void Test()
        {
            dateOfTest = DateTime.Now;
            this.Oil = 1200;
            this.KmFromTest = 0;
        }

    //private void Testing(object sender, DoWorkEventArgs e)
    //{
    //    BusStatus = Status.TESTING;
    //    for (int i = 0; i < 144; i++)
    //    {
    //        (sender as BackgroundWorker).ReportProgress(i);
    //        Thread.Sleep(1000);
    //    }
    //    dateOfTest = DateTime.Now;
    //    this.Oil = 1200;
    //    this.KmFromTest = 0;
    //    this.BusStatus = Status.READY;
    //}

    //private void Test_ProgressChanged(object sender, ProgressChangedEventArgs e)
    //{
    //    this.ProgressTest = e.ProgressPercentage;
    //    Counter = new TimeSpan(24 - (e.ProgressPercentage / 6),- ((e.ProgressPercentage % 6) * 10), 0);
    //}

    /// <summary>
    /// Prints buse's License and Kilometrage from latest test
    /// </summary>
    public void Print()
        {
            if (startDate.Year < 2018)
            {
                //prints license in format XX-XXX-XX
                Console.WriteLine($"License : {License / 100000}-{(License % 100000) / 100}-{License % 100}");
            }
            else
            {
                //prints license in format XXX-XX-XXX
                Console.WriteLine($"License : {License / 100000}-{(License % 100000) / 1000}-{License % 1000}");
            }
            Console.WriteLine($"Kilometrage : {KmFromTest} "); //prints kilometrage from latest test
            Console.WriteLine();
        }

        public override string ToString()
        {
            if (startDate.Year < 2018)
            {
                return "License : " + (License / 100000).ToString().PadLeft(3, '0') + "-" + ((License % 100000) / 100).ToString().PadLeft(2, '0') + "-" + (License % 100).ToString().PadLeft(3, '0') + " " + " Status : " + busStatus.ToString() + " startDate :  " + startDate.ToString() + "  Kilometrage " + kilometrage.ToString() + " Test : " + dateOfTest.ToString() + " KmFromTst : " + kmFromTest.ToString() + " Oil : " + oil.ToString();
            }
            else
            {
                return "License : " + (License / 100000).ToString().PadLeft(3,'0')+"-"+((License % 100000) / 1000).ToString().PadLeft(2,'0') +"-" + (License % 1000).ToString().PadLeft(3,'0') + " " + " Status : " + busStatus.ToString() + " startDate :  " + startDate.ToString() + "  Kilometrage " + kilometrage.ToString() + " Test : " + dateOfTest.ToString() + " KmFromTst : " + kmFromTest.ToString() + " Oil : " + oil.ToString(); 
            }
        }
    }
}
