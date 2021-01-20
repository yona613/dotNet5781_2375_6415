using BO;
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

        TravelSimulator() {  }

        public static TravelSimulator Instance { get => instance; }
        #endregion
        public int StationNumber { get; set; }

        private event Action<LineTiming> setDigitalPanel;
        public event Action<LineTiming> SetDigitalPanel
        { 
            add
            {
                setDigitalPanel = value;
            }
            remove
            {
                setDigitalPanel -= value;
            }
        }

        private LineTiming lineTiming;
        public LineTiming LineTiming
        {
            get
            {
                return lineTiming;
            }
            set
            {
                lineTiming = value;
                setDigitalPanel?.Invoke(lineTiming);
            }
        }
    }
}
