using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace BO
{
    public class Station
    {
        public int StationId { get; set; }

        public  GeoCoordinate Coordinates { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool  Invalid { get; set; }

        public bool Roof { get; set; }

        public bool DigitalPanel { get; set; }
    }
}
