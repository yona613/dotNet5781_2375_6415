using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Implements Bus 
    /// </summary>
    public class Bus 
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
        public int Fuel { get; set; }
        public Status BusStatus { get; set; }
        public DateTime TestDate { get; set; }
        public int KmFromTest { get; set; }
        public string Brand { get; set; }
        public bool AirConditionning { get; set; }
    }
}