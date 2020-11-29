using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using dotNet5781_01_2375_6415;

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
                    if (!tmpBus1.bw.IsBusy)
                    {
                        tmpBus1.bw = new BackgroundWorker();
                        tmpBus1.bw.DoWork += Do_Travel;
                        tmpBus1.bw.RunWorkerAsync(tmpBus1);
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

        private void Do_Travel(object sender, DoWorkEventArgs e)
        {
            try
            {
                tmpBus1.Travel(kM);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
