﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace BO
{
    /// <summary>
    /// Implements station to send to upper layer
    /// gets all data needed to be shown
    /// </summary>
    public class LineStationToShow
    {
        public int StationId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int lineNumber { get; set; }

        public int Index { get; set; }

        public Location Coordinates { get; set; }

        public double Distance { get; set; }

        public TimeSpan Time { get; set; }
    }
}
