using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusInTravel
    {
        public int License { get; set; }
        public int Line { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime RealDeparture { get; set; }
        public int LastPassedStation { get; set; }
        public DateTime TimeLastPassedStation { get; set; }
        public DateTime TimeAtNextStation { get; set; }
        public int DriversId { get; set; }
    }
}
