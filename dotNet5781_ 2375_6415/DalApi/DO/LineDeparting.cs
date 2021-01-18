using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

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

        public TimeSpan StartTime { get; set;}

        public TimeSpan Frequency { get; set; }

        public TimeSpan StopTime { get; set; }

        private Activity myActivity;

        public Activity MyActivity
        {
            get { return myActivity; }
            set { myActivity = value; }
        }
    }
}
