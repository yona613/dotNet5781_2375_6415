using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace BO
{
    /// <summary>
    /// Implements physical station
    /// </summary>
    public class Station
    {
        public int StationId { get; set; }

        public Location Coordinates { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool Invalid { get; set; }

        public bool Roof { get; set; }

        public bool DigitalPanel { get; set; }
    }
}
