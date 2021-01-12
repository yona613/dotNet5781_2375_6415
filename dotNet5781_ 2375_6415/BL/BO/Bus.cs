using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Bus //: INotifyPropertyChanged
    {
        public string LicenseToShow
        {
            get
            {
                if (LicenseDate.Year < 2018)
                {
                    return (License / 100000).ToString().PadLeft(2, '0') + "-" + ((License % 100000) / 100).ToString().PadLeft(3, '0') + "-" + (License % 100).ToString().PadLeft(2, '0');
                }
                else
                {
                    return (License / 100000).ToString().PadLeft(3, '0') + "-" + ((License % 100000) / 1000).ToString().PadLeft(2, '0') + "-" + (License % 1000).ToString().PadLeft(3, '0');
                }
            }
        }

        public int License { get; set; }
        public DateTime LicenseDate { get; set; }
        public int Kilometrage { get; set; }

        //private int fuel;
        public int Fuel { get; set; }
        //{
        //    get { return fuel; }
        //    set
        //    {
        //        fuel = value;
        //        OnPropertyChanged("Fuel");
        //    }
        //}

        //private Status busStatus;
        public Status BusStatus { get; set; }
        //{
        //    get { return busStatus; }
        //    set
        //    {
        //        busStatus = value;
        //        OnPropertyChanged("BusStatus");
        //    }
        //}

        //private DateTime testDate;
        public DateTime TestDate { get; set; }
        //{
        //    get { return testDate; }
        //    set
        //    {
        //        testDate = value;
        //        OnPropertyChanged("TestDate");
        //    }
        //}

        //private int kmFromTest;
        public int KmFromTest { get; set; }
        //{
        //    get { return kmFromTest; }
        //    set
        //    {
        //        kmFromTest = value;
        //        OnPropertyChanged("KmFromTest");
        //    }
        //}
        public string Brand { get; set; }

        //private bool airConditionning;
        public bool AirConditionning { get; set; }
        //{
        //    get { return airConditionning; }
        //    set
        //    {
        //        airConditionning = value;
        //        OnPropertyChanged("AirConditionning");
        //    }
        //}
        //private void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}