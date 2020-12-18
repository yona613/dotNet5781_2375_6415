using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineDeparting
    {
        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private TimeSpan frequency;

        public TimeSpan Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        private DateTime stopTime;

        public DateTime StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }
    }
}
