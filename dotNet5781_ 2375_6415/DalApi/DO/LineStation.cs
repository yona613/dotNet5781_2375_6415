﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// implement station in line
    /// </summary>
    public class LineStation
    {
        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        private int stationNumber;

        public int StationNumber
        {
            get { return stationNumber; }
            set { stationNumber = value; }
        }

        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private Activity myActivity;
        public Activity MyActivity
        {
            get { return myActivity; }
            set { myActivity = value; }
        }
    }

}
