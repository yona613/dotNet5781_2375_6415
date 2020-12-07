using System;
using System.ComponentModel;
using System.Threading;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Class used to implement background workers and their data
    /// </summary>
    public class MyBw : INotifyPropertyChanged
    {
        /// <summary>
        /// constructor depending on parameter sent
        /// </summary>
        /// <param name="tmpBus">Bus on wich we do work</param>
        /// <param name="str">Name of the work we want to achieve</param>
        /// <param name="kM">Km to travel(in case of travel)</param>
        public MyBw(Bus tmpBus, string str, int kM = 0)
        {
            myStatus = str; //status of the backgrnd worker
            myBus = tmpBus;
            bW = new BackgroundWorker();
            switch (str) //switching what to add to events of backgrnd worker depending on parameter sent
            {
                case "Refuel":
                    bW.DoWork += Refuel;
                    bW.ProgressChanged += Refuel_ProgressChanged;
                    break;
                case "Testing":
                    bW.DoWork += Testing;
                    bW.ProgressChanged += Test_ProgressChanged;
                    break;
                case "Travel":
                    myKm = kM;
                    bW.DoWork += Do_Travel;
                    bW.ProgressChanged += Travel_ProgressChanged;
                    break;
                default:
                    break;
            }
            bW.RunWorkerCompleted += OnProgressCompleted;
            bW.WorkerReportsProgress = true;
        }

        /// <summary>
        /// Fonction to start work of background worker
        /// </summary>
        public void Start()
        {
            bW.RunWorkerAsync();
        }

        public BackgroundWorker bW;


        /// <summary>
        ///status of background worker
        /// </summary>
        private string myStatus;

        public string MyStatus
        {
            get { return myStatus; }
            set { myStatus = value; }
        }

        public int myKm;

        private Bus myBus;

        public Bus MyBus
        {
            get { return myBus; }
            set { myBus = value; }
        }

        /// <summary>
        /// Progress used to update progress bars
        /// </summary>
        private int progress;

        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        /// <summary>
        /// Counter used to implement counter until end of work
        /// </summary>
        private TimeSpan counter;

        public TimeSpan Counter
        {
            get { return counter; }
            set
            {
                counter = value;
                OnPropertyChanged("Counter");
            }
        }

        /// <summary>
        /// function to effectuate refueling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Refuel(object sender, DoWorkEventArgs e)
        {
            myBus.BusStatus = Status.OILING;
            myBus.Oil = 0;
            for (int i = 0; i < 12; i++) //to spend 12 seconds
            {
                (sender as BackgroundWorker).ReportProgress(i); //report progress 
                myBus.Oil = i * 100; //add oil
                Thread.Sleep(1000); //stay for one second
            }
            myBus.Oil = 1200;
            myBus.BusStatus = Status.READY;
        }

        /// <summary>
        /// Reports progress of Refuelling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Refuel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Progress = e.ProgressPercentage;
            this.Counter = new TimeSpan(2 - (e.ProgressPercentage / 6), -((e.ProgressPercentage % 6) * 10), 0);
        }

        /// <summary>
        /// Fonction to effectueate testing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Testing(object sender, DoWorkEventArgs e)
        {
            myBus.BusStatus = Status.TESTING;
            for (int i = 0; i < 144; i++) //to last 144 seconds
            {
                (sender as BackgroundWorker).ReportProgress(i); //reports progress
                Thread.Sleep(1000);//sleeps one second
            }
            myBus.Test();
            myBus.BusStatus = Status.READY;
        }

        /// <summary>
        /// To update progress bar and counter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Progress = e.ProgressPercentage;
            this.Counter = new TimeSpan(24 - (e.ProgressPercentage / 6), -((e.ProgressPercentage % 6) * 10), 0);
        }

        /// <summary>
        /// Function to handle travelling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Do_Travel(object sender, DoWorkEventArgs e)
        {
            myBus.Travel(myKm); //effctuate travel using buse's function 
            myBus.BusStatus = Status.TRAVELLING;
            int mySpeed = Program.r.Next(20, 51); //gets average speed
            int myTime = (myKm / mySpeed * 6000 + (myKm % mySpeed) * 100); //gets time to last
            this.Counter = new TimeSpan(myKm / mySpeed, (myKm % mySpeed), 0);
            for (int i = 1; i <= (myTime / 100); i++) //to last tim needed
            {
                bW.ReportProgress(i);
                Thread.Sleep(100);
            }
            myBus.BusStatus = Status.READY;
            throw new MyTravelException($"Bus {myBus.License1} has traveled : {myKm} Km");
        }


        /// <summary>
        /// Upddates travel progress counter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Travel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Counter = this.counter.Subtract(new TimeSpan(0, 1, 0));
        }

        /// <summary>
        /// When Work is completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Progress = 0;
            this.Counter = new TimeSpan();
        }

        public event PropertyChangedEventHandler PropertyChanged; //to implement INotifyPropertyChanged

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
