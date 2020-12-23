using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineDeparting
    {
        public int LineNumber  { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Frequency { get; set; }
        public DateTime StopTime { get; set; }
    }
}
