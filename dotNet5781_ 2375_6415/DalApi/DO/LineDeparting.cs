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

        private TimeSpan startTime;

        public TimeSpan StartTime
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

        private TimeSpan stopTime;

        public TimeSpan StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }

        private Activity myActivity;
        public Activity MyActivity
        {
            get { return myActivity; }
            set { myActivity = value; }
        }
    }
}
