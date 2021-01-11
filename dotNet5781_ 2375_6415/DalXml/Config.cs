using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public static class Config
    {
        static int busInTravelCounter = 0;
        public static int BusInTravelCounter => ++busInTravelCounter;

        static int busLineCounter = 0;
        public static int BusLineCounter => ++busLineCounter;

        static int userTripCounter = 0;
        public static int UserTripCounter => ++userTripCounter;
    }
}
