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
                    try
                    {
                        if (tmpInt >= 10000000 && tmpInt < 100000000) //can only add bus with 8 digits (year 2020 +)
                        {
                            Bus tmpBus = new Bus(DateTime.Now, tmpInt, 1200, 0, 0, DateTime.Now);
                            foreach (var item in MainWindow.myBusList)
                            {
                                if (item.License == tmpInt)
                                {
                                    throw new ArgumentException("Bus already exists, enter valid number !!!");
                                }
                            }
                            MainWindow.myBusList.Add(tmpBus);
                            AddBusWin.Close();
                        }
                        else
                        {
                            throw new ArgumentException("License not valid, enter valid number (license : 8 digits) !!!");
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                        GetlicenseTxb.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong Input !!!!");
                    GetlicenseTxb.Clear();
                }
            }
        }
    }
}
