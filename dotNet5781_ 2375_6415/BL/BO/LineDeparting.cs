using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Implements Frequency of departure of a line
    /// </summary>
    public class LineDeparting
    {
        public int LineNumber  { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Frequency { get; set; }
        public TimeSpan StopTime { get; set; }
    }
}
