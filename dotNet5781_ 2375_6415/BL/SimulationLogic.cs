using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Implements Logic of simulation
    /// </summary>
    public static class SimulationLogic
    {
        public static void LineDepartingDoWork(object sender, DoWorkEventArgs e)
        {
            LineDepartingSimulation myLineDeparting = e.Argument as LineDepartingSimulation;
            while (SimulatorClock.Instance.Cancel == false)
            {
                if (myLineDeparting.StartTime <= SimulatorClock.Instance.Time && myLineDeparting.StopTime >= SimulatorClock.Instance.Time)
                {
                    BackgroundWorker lineTravelBw = new BackgroundWorker();
                    lineTravelBw.DoWork += LineInTravelSimulatorDoWork;
                    LineInTravel myLineIntravel = new LineInTravel()
                    {
                        LineNumber = myLineDeparting.LineNumber,
                        Key = Config.Number,
                        LastStation = myLineDeparting.LastStation,
                        LineStations = myLineDeparting.LineStations.ToList()
                    };
                    lineTravelBw.RunWorkerAsync(myLineIntravel);
                }
                Thread.Sleep((int)myLineDeparting.Frequency.TotalMilliseconds / SimulatorClock.Instance.Rate);
            }
        }

        public static void LineInTravelSimulatorDoWork(object sender, DoWorkEventArgs e)
        {
            LineInTravel myLine = e.Argument as LineInTravel;
            bool flag = false;
            int station = TravelSimulator.Instance.StationNumber;
            TimeSpan timeToStation;
            for (int i = 0; i < myLine.LineStations.Count && SimulatorClock.Instance.Cancel == false; i++)
            {
                timeToStation = new TimeSpan();
                for (int j = i; j < myLine.LineStations.Count; j++)
                {
                    if (myLine.LineStations[i].StationId == TravelSimulator.Instance.StationNumber)
                    {
                        flag = true;
                        station = TravelSimulator.Instance.StationNumber;
                        timeToStation = TimeSpan.Zero;
                        TravelSimulator.Instance.LineTiming = new LineTiming { Key = myLine.Key, ArrivalTime = timeToStation, LastStation = myLine.LastStation, LineNumber = myLine.LineNumber };
                        break;
                    }
                    if (myLine.LineStations[j].StationId == TravelSimulator.Instance.StationNumber)
                    {
                        flag = true;
                        station = TravelSimulator.Instance.StationNumber;
                        TravelSimulator.Instance.LineTiming = new LineTiming { Key = myLine.Key, ArrivalTime = timeToStation, LastStation = myLine.LastStation, LineNumber = myLine.LineNumber };
                        break;
                    }
                    timeToStation += myLine.LineStations[j].Time;
                }
                int timeToSleep = (int)(myLine.LineStations[i].Time.TotalMilliseconds / SimulatorClock.Instance.Rate);
                while (timeToSleep >= 1000)
                {
                    Thread.Sleep(1000);
                    timeToSleep -= 1000;
                    for (int k = i; k < myLine.LineStations.Count; k++)
                    {
                        if (myLine.LineStations[k].StationId == TravelSimulator.Instance.StationNumber && TravelSimulator.Instance.StationNumber == station)
                        {
                            if (timeToStation != TimeSpan.Zero)// && TravelSimulator.Instance.StationNumber == station && flag)
                            {
                                timeToStation = timeToStation.Subtract(new TimeSpan(10000000 * SimulatorClock.Instance.Rate));
                                TravelSimulator.Instance.LineTiming = new LineTiming { Key = myLine.Key, ArrivalTime = timeToStation, LastStation = myLine.LastStation, LineNumber = myLine.LineNumber };
                            }
                        }
                    }
                }
                Thread.Sleep(timeToSleep);
                flag = false;
            }
        }

    }
}
