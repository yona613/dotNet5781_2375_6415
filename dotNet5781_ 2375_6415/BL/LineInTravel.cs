using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class LineInTravel
    {
        public int LineNumber { get; set; }
        public int Key { get; set; }
        public string LastStation { get; set; }
        public List<LineStationToShow> LineStations { get; set; }
    }
}
