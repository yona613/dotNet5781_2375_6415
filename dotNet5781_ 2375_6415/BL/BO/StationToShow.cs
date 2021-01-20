using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace BO
{
    /// <summary>
    /// Implements station to be sent to upper layer
    /// Gets all data that is needed to be shawn
    /// </summary>
    public class StationToShow
    {
        public int StationId { get; set; }

        public Location Coordinates { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool Invalid { get; set; }

        public bool Roof { get; set; }

        public bool DigitalPanel { get; set; }

        public IEnumerable<string> Lines { get; set; }
    }
}
