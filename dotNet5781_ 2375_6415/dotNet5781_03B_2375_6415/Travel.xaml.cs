using System.Windows;
using System.ComponentModel;
using System.Windows.Input;
using dotNet5781_01_2375_6415;
using System.Text.RegularExpressions;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for Travel.xaml
    /// </summary>
    public partial class Travel : Window
    {
        public Bus tmpBus1;
        public int kM = 0;
        public Travel(Bus tmpBus)
        {
            tmpBus1 = tmpBus;
            InitializeComponent();
        }

        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(TravelTxb.Text, out kM))
                {
                    if (tmpBus1.BusStatus==Status.READY)
                    {

                        MyBw bw = new MyBw(tmpBus1, "Travel",kM);
                        bw.bW.RunWorkerCompleted += Travel_ProgressCompleted;
                        MainWindow.myBwList.Add(bw);
                        bw.Start();
                        //tmpBus1.bw = new BackgroundWorker();
                        //tmpBus1.bw.WorkerReportsProgress = true;
                        //tmpBus1.bw.DoWork += Travel_ProgressCompleted;
                        //tmpBus1.bw.RunWorkerAsync(tmpBus1);
                    }
                    else
                    {
                        MessageBox.Show("Bus already Busy !!!");
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong Input !!!!");
                    TravelTxb.Clear();
                }
            }
        }

        public void Travel_ProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error!=null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                MessageBox.Show($"Bus traveled : {kM} Km");
            }
        }

        private void TravelTxb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex myReg = new Regex("[^0-9]+");
            e.Handled = myReg.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"Wrong Input !!!! \n {e.Text} Enter digits only");
            }
        }
    }
}
