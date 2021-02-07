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
        /// <summary>
        /// Function do work of Line Departing's Background worker
        /// sends line to travel in a separate thread each frequency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void LineDepartingDoWork(object sender, DoWorkEventArgs e)
        {
            //gets all data sent
            LineDepartingSimulation myLineDeparting = e.Argument as LineDepartingSimulation;
            while (SimulatorClock.Instance.Cancel == false) //works only when simulator is on
            {
                //if it's time to send lines to travel
                if (((myLineDeparting.StartTime.Hours < SimulatorClock.Instance.Time.Hours ) ||
                    (myLineDeparting.StartTime.Hours == SimulatorClock.Instance.Time.Hours &&
                    myLineDeparting.StartTime.Minutes <= SimulatorClock.Instance.Time.Minutes )) &&
                    ((myLineDeparting.StopTime.Hours > SimulatorClock.Instance.Time.Hours) ||  
                    (myLineDeparting.StopTime.Hours == SimulatorClock.Instance.Time.Hours  && 
                    myLineDeparting.StopTime.Minutes >= SimulatorClock.Instance.Time.Minutes)))
                {
                    //send new line to travel in a different thread
                    BackgroundWorker lineTravelBw = new BackgroundWorker();
                    lineTravelBw.DoWork += LineInTravelSimulatorDoWork;
                    //gets all data for the new thread
                    LineInTravel myLineIntravel = new LineInTravel()
                    {
                        LineNumber = myLineDeparting.LineNumber,
                        Key = Config.Number, //unique key to be differenciated
                        LastStation = myLineDeparting.LastStation,
                        LineStations = myLineDeparting.LineStations.ToList()
                    };
                    lineTravelBw.RunWorkerAsync(myLineIntravel); //start thread
                }
                //sleep until next frequency
                Thread.Sleep((int)myLineDeparting.Frequency.TotalMilliseconds / SimulatorClock.Instance.Rate);
            }
        }

        /// <summary>
        /// Function for line's background worker do work
        /// this function implements the travel of a specific line all the way
        /// sends updates for the observed station to Travel Simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void LineInTravelSimulatorDoWork(object sender, DoWorkEventArgs e)
        {
            //gets data sent
            LineInTravel myLine = e.Argument as LineInTravel;
            bool flag = false; //to know if the observed station is in this line
            int station = TravelSimulator.Instance.StationNumber; //gets number of station observed
            TimeSpan timeToStation; //sets time to observed station
            //goes over the all way of the line
            for (int i = 0; i < myLine.LineStations.Count && SimulatorClock.Instance.Cancel == false; i++)
            {
                timeToStation = new TimeSpan();
                for (int j = i; j < myLine.LineStations.Count; j++) //goes over all next stations till the last one
                {
                    //if we are in the station observed
                    if (myLine.LineStations[i].StationId == TravelSimulator.Instance.StationNumber)
                    {
                        flag = true; //yes
                        station = TravelSimulator.Instance.StationNumber;
                        timeToStation = TimeSpan.Zero; //send time span Zero because we arrived to station
                        //update Travel Simulator
                        TravelSimulator.Instance.LineTiming = new LineTiming { Key = myLine.Key, ArrivalTime = timeToStation, LastStation = myLine.LastStation, LineNumber = myLine.LineNumber };
                        break;
                    }
                    //If the observed station is on the way
                    if (myLine.LineStations[j].StationId == TravelSimulator.Instance.StationNumber)
                    {
                        flag = true; //yes
                        station = TravelSimulator.Instance.StationNumber;
                        //sends update to travel simulator with the time to observed station
                        TravelSimulator.Instance.LineTiming = new LineTiming { Key = myLine.Key, ArrivalTime = timeToStation, LastStation = myLine.LastStation, LineNumber = myLine.LineNumber };
                        break;
                    }
                    timeToStation += myLine.LineStations[j].Time; //sets time to observed station
                }
                //gets time we need to sleep untill next station in line
                int timeToSleep = (int)(myLine.LineStations[i].Time.TotalMilliseconds / SimulatorClock.Instance.Rate);
                while (timeToSleep >= 1000) //to send updates each second
                {
                    Thread.Sleep(1000);
                    timeToSleep -= 1000;
                    for (int k = i; k < myLine.LineStations.Count; k++) //checks if observed station is in the line (Because it may have changed)
                    {
                        if (myLine.LineStations[k].StationId == TravelSimulator.Instance.StationNumber && TravelSimulator.Instance.StationNumber == station)
                        {
                            if (timeToStation != TimeSpan.Zero) //if we need update
                            {
                                //update time to station and send update to travel simulator
                                timeToStation = timeToStation.Subtract(new TimeSpan(10000000 * SimulatorClock.Instance.Rate));
                                TravelSimulator.Instance.LineTiming = new LineTiming { Key = myLine.Key, ArrivalTime = timeToStation, LastStation = myLine.LastStation, LineNumber = myLine.LineNumber };
                            }
                        }
                    }
                }
                //sleep for remaining time
                Thread.Sleep(timeToSleep);
                flag = false;
            }
        }
    }
}
