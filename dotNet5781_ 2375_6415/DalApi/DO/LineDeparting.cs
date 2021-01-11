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

        private TimeSpan startTime;

        [XmlIgnore]
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        [XmlElement("StartTime", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlStartTime
        {
            get { return XmlConvert.ToString(startTime); }
            set { startTime = XmlConvert.ToTimeSpan(value); }
        }

        private TimeSpan frequency;

        [XmlIgnore]
        public TimeSpan Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        [XmlElement("Frequency", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlFrequency
        {
            get { return XmlConvert.ToString(frequency); }
            set { frequency = XmlConvert.ToTimeSpan(value); }
        }

        private TimeSpan stopTime;

        [XmlIgnore]
        public TimeSpan StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }


        [XmlElement("StopTime", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlStopTime
        {
            get { return XmlConvert.ToString(stopTime); }
            set { stopTime = XmlConvert.ToTimeSpan(value); }
        }

        private Activity myActivity;
        public Activity MyActivity
        {
            get { return myActivity; }
            set { myActivity = value; }
        }
    }
}
