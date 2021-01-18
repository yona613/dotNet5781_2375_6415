using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BLApi;
using System.Threading;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for StationData.xaml
    /// </summary>
    public partial class StationData : Window
    {
        BO.StationToShow myStation;
        //observable collection implementing digital panel
        ObservableCollection<LineTiming> myLineTimings = new ObservableCollection<LineTiming>() { };
        IBL bl = BLFactory.GetBL();
        //background worker to handle digital panel
        BackgroundWorker myDigitalPanelBw = null;


        public StationData(BO.StationToShow tmpStation, BackgroundWorker tmpBackgroundWorker)
        {
            InitializeComponent();
            myStation = tmpStation;
            mainGrid.DataContext = tmpStation;
            digitalPanelDataGrid.ItemsSource = myLineTimings;
            if (myStation.DigitalPanel == true) //if line station has digital panel
            {
                //start process of digital panel
                myDigitalPanelBw = tmpBackgroundWorker;
                myDigitalPanelBw.DoWork += MyDigitalPanelDoWork;
                myDigitalPanelBw.RunWorkerAsync();
                //to enable cancellation when closing window
                myDigitalPanelBw.WorkerSupportsCancellation = true;
            }
        }
        /// <summary>
        /// Event to add to background worker's do work 
        /// start simulation of digital panel
        /// sends action of update of digital panel to bl every 6 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyDigitalPanelDoWork(object sender , DoWorkEventArgs e)
        {
            //check that simulator is opened and background worker is not cancelling
            while (bl.IsSimulator() && !(sender as BackgroundWorker).CancellationPending)
            {
                Dispatcher.Invoke(new Action(() => myLineTimings.Clear()));//clear observable collection
                bl.SetStationPanel(myStation, new Action<LineTiming>(x =>
                {
                    //action that adds all Line Timings to collection (in bl)
                    Dispatcher.Invoke(new Action(() => myLineTimings.Add(x)));
                }));
                Thread.Sleep(6000);
            }
        }
    }
}
