using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{

    public enum Status { READY, TRAVELLING, REFUELLING, TESTING }
    class Bus
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
    }
}
