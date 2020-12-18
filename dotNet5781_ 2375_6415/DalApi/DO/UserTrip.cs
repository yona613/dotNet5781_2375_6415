using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class UserTrip
    {
        private int key;

        public int Key
        {
            get { return key; }
            set { key = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        private int firstStation;

        public int FirstStation
        {
            get { return firstStation; }
            set { firstStation = value; }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private int lastStation;

        public int LastStation
        {
            get { return lastStation; }
            set { lastStation = value; }
        }

        private DateTime stopTime;

        public DateTime StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }
    }
}
