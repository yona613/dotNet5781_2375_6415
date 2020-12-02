using System;
using System.ComponentModel;
using System.Threading;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    public class MyBw : INotifyPropertyChanged
    {
        public MyBw(Bus tmpBus, string str, int kM = 0)
        {
            myStatus = str;
            myBus = tmpBus;
            bW = new BackgroundWorker();
            switch (str)
            {
                case "Refuel":
                    bW.DoWork += Refuel;
                    bW.ProgressChanged += Refuel_ProgressChanged;
                    bW.RunWorkerCompleted += Refuel_ProgressCompleted;
                    break;
                case "Testing":
                    bW.DoWork += Testing;
                    bW.ProgressChanged += Test_ProgressChanged;
                    //bW.RunWorkerCompleted += Refuel_ProgressCompleted;
                    break;
                case "Travel":
                    myKm = kM;
                    bW.DoWork += Do_Travel;
                    bW.ProgressChanged += Travel_ProgressChanged;
                    break;
                default:
                    break;
            }
            bW.WorkerReportsProgress = true;
        }

        public void Start()
        {
            bW.RunWorkerAsync();
        }

        public BackgroundWorker bW;

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

        public void Refuel(object sender, DoWorkEventArgs e)
        {
            myBus.BusStatus = Status.OILING;
            myBus.Oil = 0;
            for (int i = 0; i < 12; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                myBus.Oil = i * 100;
                Thread.Sleep(1000);
            }
            myBus.Oil = 1200;
            myBus.BusStatus = Status.READY;
        }


        public void Refuel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Progress = e.ProgressPercentage;
            this.Counter = new TimeSpan(2 - (e.ProgressPercentage / 6), -((e.ProgressPercentage % 6) * 10), 0);
        }

        public void Refuel_ProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Progress = 0;
            this.Counter = new TimeSpan(0, 0, 0);
        }


        private void Testing(object sender, DoWorkEventArgs e)
        {
            myBus.BusStatus = Status.TESTING;
            for (int i = 0; i < 144; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(1000);
            }
            //myBus.DateOfTest = DateTime.Now;
            myBus.Oil = 1200;
            myBus.KmFromTest = 0;
            myBus.BusStatus = Status.READY;
        }

        private void Test_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Progress = e.ProgressPercentage;
            this.Counter = new TimeSpan(24 - (e.ProgressPercentage / 6), -((e.ProgressPercentage % 6) * 10), 0);
        }

        //public void Travel_DoWork(int tmpKm)
        //{
        //    bw.WorkerReportsProgress = true;
        //    bw.ProgressChanged += Travel_ProgressChanged;
        //    bw.RunWorkerAsync();
        //}

        private void Do_Travel(object sender, DoWorkEventArgs e)
        {
            if (myBus.Travel(myKm))
            {
                myBus.BusStatus = Status.TRAVELLING;
                int mySpeed = Program.r.Next(20, 51);
                int myTime = (myKm / mySpeed * 6000 + (myKm % mySpeed) * 100);
                this.Counter = new TimeSpan(myKm / mySpeed, (myKm % mySpeed), 0);
                for (int i = 1; i <= (myTime / 100); i++)
                {
                    bW.ReportProgress(i);
                    Thread.Sleep(100);
                }
                myBus.BusStatus = Status.READY;
            }
        }

        private void Travel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Progress = e.ProgressPercentage;
            this.Counter = this.counter.Subtract(new TimeSpan(0, 1, 0));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
