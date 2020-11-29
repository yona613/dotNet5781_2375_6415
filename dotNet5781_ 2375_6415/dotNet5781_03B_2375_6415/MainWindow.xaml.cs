using System;
using System.ComponentModel;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static List<Bus> myBusList = Program.CreateBusList();


        public MainWindow()
        {
            InitializeComponent();
            BusList.ItemsSource = myBusList;
        }

        private void AddBusbt_Click(object sender, RoutedEventArgs e)
        {
            Window addWin = new dotNet5781_03B_2375_6415.Window1();
            addWin.ShowDialog();
            BusList.Items.Refresh();
        }

        private void Travel_Click(object sender, RoutedEventArgs e)
        {
            if (!((Bus)(((Button)sender).DataContext)).bw.IsBusy)
            {
                Window tmpWin = new Travel((Bus)(((Button)sender).DataContext));
                tmpWin.ShowDialog();
                Dispatcher.BeginInvoke(new Action(BusList.Items.Refresh));
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
            
        }

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            if (!((Bus)(((Button)sender).DataContext)).bw.IsBusy)
            {
                ((Bus)(((Button)sender).DataContext)).bw = new BackgroundWorker();
                ((Bus)(((Button)sender).DataContext)).bw.DoWork += Refuel;
                ((Bus)(((Button)sender).DataContext)).bw.WorkerReportsProgress = true;
                ((Bus)(((Button)sender).DataContext)).bw.ProgressChanged += RefuelingProgress;
                ((Bus)(((Button)sender).DataContext)).bw.RunWorkerAsync(((Bus)(((Button)sender).DataContext)));
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }

        private void Refuel(object sender, DoWorkEventArgs e)
        {
            ((Bus)e.Argument).BusStatus = Status.OILING;
            Dispatcher.BeginInvoke(new Action(BusList.Items.Refresh));
            for (int i = 0; i < 12; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i * 8);
                Thread.Sleep(1000);
            }
            ((Bus)e.Argument).Fuel();
            ((Bus)e.Argument).BusStatus = Status.READY;
            Dispatcher.BeginInvoke(new Action(BusList.Items.Refresh));
        }

        private void BusData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                BusData tmpWin = new BusData(((Bus)(((TextBlock)sender).DataContext)));
                tmpWin.ShowDialog();
                BusList.Items.Refresh();
            }
        }

        void RefuelingProgress(object sender , ProgressChangedEventArgs e)
        {
            ProgressBar myPB = (ProgressBar)BusList.FindName("RefuelPB");
           // myPB.Value = e.ProgressPercentage;
        }
    }
}
