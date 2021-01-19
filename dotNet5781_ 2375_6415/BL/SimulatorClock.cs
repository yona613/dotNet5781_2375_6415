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

        static SimulatorClock() { }

        SimulatorClock() { }

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
                //ClockObserver -= value; //delete all functions
                clockObserver = value; 
            }
            remove  //delete all functions
            {
                clockObserver -= value;
                //if (clockObserver != null)
                //{
                //    foreach (var d in clockObserver.GetInvocationList())
                //    {
                //        clockObserver -= (Action<TimeSpan>)d;
                //    }
                //}               
            }
        }
    }
}
