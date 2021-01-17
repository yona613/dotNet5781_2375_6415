using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class TravelSimulator
    {
        #region Singleton
        static readonly TravelSimulator instance = new TravelSimulator();
        static TravelSimulator() { }
        TravelSimulator() { }
        public static TravelSimulator Instance { get => instance; }
        #endregion


    }
}
