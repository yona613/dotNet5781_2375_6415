using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public Travel(Bus tmpBus)
        {
            tmpBus1 = tmpBus;
            InitializeComponent();
        }

        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            int tmpInt;
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(TravelTxb.Text, out tmpInt))
                {
                    try
                    {
                        tmpBus1.Travel(tmpInt);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong Input !!!!");
                    TravelTxb.Clear();
                }
            }

        }
    }
}
