using System;
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
using System.Windows.Shapes;
using BLApi;
using BL;
using System.ComponentModel;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Start : Window
    {
        public static IBL bl = BLFactory.GetBL();
        public Start()
        {
            InitializeComponent();
        }

        private void ManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            new ManagerLogin().ShowDialog();
        }

        private void UserBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet ... in buiding !!! !!!");
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (startTimePicker.SelectedTime != null)
            {
                if (rateTb.Text != "00")
                {
                    stopBtn.IsEnabled = true;
                    startBtn.IsEnabled = false;
                    startBtn.Background = Brushes.Red;
                    stopBtn.Background = Brushes.Green;
                    rateTb.IsEnabled = false;
                    startTimePicker.IsEnabled = false;
                    BackgroundWorker simulator = new BackgroundWorker();
                    simulator.DoWork += StartSimulator;
                    simulator.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Choose a Rate !");
                }
            }
            else
            {
                MessageBox.Show("Choose a Starting Time !");
            }
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            startTimePicker.IsEnabled = true;
            startTimePicker.SelectedTime = null;
            rateTb.IsEnabled = true;
            rateTb.Text = "00";
            stopBtn.IsEnabled = false;
            stopBtn.Background = Brushes.Red;
            startBtn.Background = Brushes.Green;
            startBtn.IsEnabled = true;
            bl.StopSimulator();
        }

        public void StartSimulator(object sender, DoWorkEventArgs e)
        {
            TimeSpan myTimeSpan = new TimeSpan();
            int rate = 0;
            Dispatcher.Invoke(new Action(() => GetValues(out myTimeSpan, out rate)));
            bl.StartSimulator(myTimeSpan, rate, x => {
            myTimeSpan = x;
            Dispatcher.BeginInvoke(new Action(() => timeLbl.Content = myTimeSpan.ToString("hh\\:mm\\:ss")));          
            });
        }

        public void GetValues(out TimeSpan myTimeSpan , out int rate)
        {
            myTimeSpan = startTimePicker.SelectedTime.Value.TimeOfDay;
            int.TryParse(rateTb.Text, out rate);
        }
    }
}
