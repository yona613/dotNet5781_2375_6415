using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
using System.Device.Location;

namespace DO
{
    public class Station
    {
        private int stationId;

        public int StationId
        {
            get { return stationId; }
            set { stationId = value; }
        }

        private Location coordinates;

        public Location Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private bool invalid;

        public bool  Invalid
        {
            get { return invalid; }
            set { invalid = value; }
        }

        private bool roof;

        public bool Roof
        {
            get { return roof; }
            set { roof = value; }
        }

        private bool digitalPanel;

        public bool DigitalPanel
        {
            get { return digitalPanel; }
            set { digitalPanel = value; }
        }

        private Activity myActivity;
        public Activity MyActivity
        {
            get { return myActivity; }
            set { myActivity = value; }
        }
    }
}
