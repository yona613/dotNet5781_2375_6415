using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Implements line in travel
    /// Gets all data for the line travelling
    /// </summary>
    public class LineInTravel
    {
        public int LineNumber { get; set; }
        public int Key { get; set; } //unique key from config class
        public string LastStation { get; set; }
        public List<LineStationToShow> LineStations { get; set; }
    }
}
