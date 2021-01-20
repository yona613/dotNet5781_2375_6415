using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// implements bus
    /// </summary>
    public class Bus
    {
        private int license;

        public int License
        {
            get { return license; }
            set { license = value; }
        }

        private DateTime licenseDate;

        public DateTime LicenseDate
        {
            get { return licenseDate; }
            set { licenseDate = value; }
        }

        private int kilometrage;

        public int Kilometrage
        {
            get { return kilometrage; }
            set { kilometrage = value; }
        }

        private int fuel;

        public int Fuel
        {
            get { return fuel; }
            set { fuel = value; }
        }

        private Status busStatus;

        public Status BusStatus
        {
            get { return busStatus; }
            set { busStatus = value; }
        }

        private DateTime testDate;

        public DateTime TestDate
        {
            get { return testDate; }
            set { testDate = value; }
        }

        private int kmFromTest;

        public int KmFromTest
        {
            get { return kmFromTest; }
            set { kmFromTest = value; }
        }

        private string brand;

        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        private bool airConditionning;

        public bool AirConditionning
        {
            get { return airConditionning; }
            set { airConditionning = value; }
        }

        private Activity myActivity;
        public Activity MyActivity 
        {
            get { return myActivity; }
            set { myActivity = value; } 
        }
    }
}