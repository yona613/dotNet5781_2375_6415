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
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int tmpInt;
                if (int.TryParse(GetlicenseTxb.Text, out tmpInt))
                {
                    if (tmpInt >= 10000000 && tmpInt < 1000000000)
                    {
                        Bus tmpBus = new Bus(DateTime.Now,tmpInt,1200,0,0,DateTime.Now);
                        MainWindow.myBusList.Add(tmpBus);
                        AddBusWin.Close();
                    }
                }
            }
        }
    }
}
