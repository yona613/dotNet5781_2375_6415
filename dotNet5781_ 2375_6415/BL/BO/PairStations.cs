﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class PairStations
    {
        public int FirstStationNumber { get; set; }
        public int LastStationNumber { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
    }
}
