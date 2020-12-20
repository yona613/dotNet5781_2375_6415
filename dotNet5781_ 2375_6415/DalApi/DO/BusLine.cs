using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusLine
    {
        private int key;

        public int Key
        {
            get { return key; }
            set { key = value; }
        }

        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        private Area lineArea;

        public Area LineArea
        {
            get { return lineArea; }
            set { lineArea = value; }
        }

        private int firstStation;

        public int FirstStation
        {
            get { return firstStation; }
            set { firstStation = value; }
        }

        private int lastStation;

        public int LastStation
        {
            get { return lastStation; }
            set { lastStation = value; }
        }

        private Activity myActivity;
        public Activity MyActivity
        {
            get { return myActivity; }
            set { myActivity = value; }
        }
    }
}
