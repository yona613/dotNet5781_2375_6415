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
        ObservableCollection<LineTiming> myLineTimings = new ObservableCollection<LineTiming>() { };
        IBL bl = BLFactory.GetBL();
        BackgroundWorker myDigitalPanelBw = null;


        public StationData(BO.StationToShow tmpStation, BackgroundWorker tmpBackgroundWorker)
        {
            InitializeComponent();
            myStation = tmpStation;
            mainGrid.DataContext = tmpStation;
            digitalPanelDataGrid.ItemsSource = myLineTimings;
            if (myStation.DigitalPanel == true)
            {
                myDigitalPanelBw = tmpBackgroundWorker;
                myDigitalPanelBw.DoWork += MyDigitalPanelDoWork;
                myDigitalPanelBw.RunWorkerAsync();
                myDigitalPanelBw.WorkerSupportsCancellation = true;
            }
        }

        void MyDigitalPanelDoWork(object sender , DoWorkEventArgs e)
        {
            while (bl.IsSimulator() && !(sender as BackgroundWorker).CancellationPending)
            {
                Dispatcher.Invoke(new Action(() => myLineTimings.Clear()));
                bl.SetStationPanel(myStation, new Action<LineTiming>(x =>
                {
                    Dispatcher.Invoke(new Action(() => myLineTimings.Add(x)));
                }));
                Thread.Sleep(6000);
            }
        }
    }
}
