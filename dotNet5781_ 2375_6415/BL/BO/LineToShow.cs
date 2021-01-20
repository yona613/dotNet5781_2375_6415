using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineToShow
    {
        public int LineNumber { get; set; }
        public Area LineArea { get; set; }
        public string LastStationName { get; set; }
        public IEnumerable<LineStationToShow> LineStations { get; set; }
        public IEnumerable<LineDeparting> LineDepartings { get; set; }
    }
}
