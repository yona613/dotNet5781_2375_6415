using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    class LineDepartingSimulation
    {
        public int LineNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Frequency { get; set; }
        public TimeSpan StopTime { get; set; }
        public IEnumerable<LineStationToShow> LineStations { get; set; }
        public string LastStation { get; set; }
    }
}
