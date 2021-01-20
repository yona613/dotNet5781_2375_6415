using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Implements timing of line to station observed
    /// </summary>
    public class LineTiming
    {
        public int Key { get; set; }      
        public int LineNumber { get; set; }
        public string LastStation { get; set; }
        public TimeSpan ArrivalTime { get; set; }

    }
}
