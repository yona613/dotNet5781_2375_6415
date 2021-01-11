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
    public class PairStations
    {
        private int firstStationNumber;

        public int FirstStationNumber
        {
            get { return firstStationNumber; }
            set { firstStationNumber = value; }
        }

        private int lastStationNumber;

        public int LastStationNumber
        {
            get { return lastStationNumber; }
            set { lastStationNumber = value; }
        }

        private double distance;

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        private TimeSpan time;

        [XmlIgnore]
        public TimeSpan Time
        {
            get { return time; }
            set { time = value; }
        }


        [XmlElement("Time", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlTime
        {
            get { return XmlConvert.ToString(time); }
            set { time = XmlConvert.ToTimeSpan(value); }
        }

        private Activity myActivity;
        public Activity MyActivity
        {
            get { return myActivity; }
            set { myActivity = value; }
        }
    }
}
