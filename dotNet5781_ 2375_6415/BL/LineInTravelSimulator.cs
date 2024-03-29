﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Class for simulation of line in travel
    /// gets all data needed in the process
    /// </summary>
    class LineInTravelSimulator
    {
        public int LineNumber { get; set; }
        public string LastStation { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan TravelTime { get; set; }
    }
}
