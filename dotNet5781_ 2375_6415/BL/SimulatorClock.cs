using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public delegate void Notify();

    /// <summary>
    /// Class used for simulation of clock 
    /// </summary>
    class SimulatorClock
    {
        #region Singleton
        static readonly SimulatorClock instance = new SimulatorClock();

        static SimulatorClock() {}

        SimulatorClock() { this.Cancel = true; }

        public static SimulatorClock Instance { get => instance; }
        #endregion

        private TimeSpan time;
        public TimeSpan Time 
        {
            get 
            { 
                return time; 
            }
            set
            {
                time = value;
                clockObserver?.Invoke(time);
            }
        }
        public int Rate { get; set; }
        internal volatile bool Cancel;

        public Stopwatch stopWatch = new Stopwatch();

        event Action<TimeSpan> clockObserver;

        public event Action<TimeSpan> ClockObserver
        {
            add //only add one function at a time
            {
                clockObserver = value; 
            }
            remove 
            {
                clockObserver -= value;            
            }
        }
    }
}
