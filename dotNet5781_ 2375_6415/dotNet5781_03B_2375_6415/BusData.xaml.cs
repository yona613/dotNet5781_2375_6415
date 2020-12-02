using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        
        public static Bus tmpBus1;
        public BusData(Bus tmpBus)
        {
            InitializeComponent();
            MainGrid.DataContext = tmpBus;
            tmpBus1 = tmpBus;
        }

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
           if (!((Bus)MainGrid.DataContext).bw.IsBusy)
            {
                ((Bus)MainGrid.DataContext).bw = new BackgroundWorker();
                ((Bus)MainGrid.DataContext).bw.DoWork += Refuel;
                ((Bus)MainGrid.DataContext).bw.ProgressChanged += RefuelingProgress;
                ((Bus)MainGrid.DataContext).bw.WorkerReportsProgress = true;
                ((Bus)MainGrid.DataContext).bw.RunWorkerAsync(((Bus)MainGrid.DataContext));
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (!((Bus)MainGrid.DataContext).bw.IsBusy)
            {
                ((Bus)MainGrid.DataContext).bw = new BackgroundWorker();
                ((Bus)MainGrid.DataContext).bw.DoWork += Test;
                ((Bus)MainGrid.DataContext).bw.RunWorkerAsync(((Bus)MainGrid.DataContext));
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }

        private void Test(object sender, DoWorkEventArgs e)
        {
            ((Bus)e.Argument).Test();
            Dispatcher.BeginInvoke(new Action(Update));
        }

        private void Refuel(object sender, DoWorkEventArgs e)
        {
            ((Bus)e.Argument).BusStatus = Status.OILING;
            Dispatcher.BeginInvoke(new Action(Update));
            for (int i = 1; i <= 12; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(1000);
            }
            ((Bus)e.Argument).Fuel();
            ((Bus)e.Argument).BusStatus = Status.READY;
            Dispatcher.BeginInvoke(new Action(Update));
            (sender as BackgroundWorker).ReportProgress(0);
        }

        private void Update()
        {
            MainGrid.DataContext = null;
            MainGrid.DataContext = tmpBus1;
        }

        void RefuelingProgress(object sender, ProgressChangedEventArgs e)
        {
            RefPB.Value = e.ProgressPercentage;
        }
    }
}
