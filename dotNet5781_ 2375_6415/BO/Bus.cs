using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Bus
    {
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
