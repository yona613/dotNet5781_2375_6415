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

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        BO.Bus myBus;
        //public static Bus tmpBus1;
        public BusData(BO.Bus tmpBus)
        {
            InitializeComponent();
            myBus = tmpBus;
            MainGrid.DataContext = myBus; //initialize data on the window
        }

        /// <summary>
        /// Event when click on refuel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            if (myBus.BusStatus == BO.Status.Ready)
            {
                myBus.Fuel = 1200;
                try
                {
                    MainWindow.bl.UpdateBus(myBus);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                myBus = MainWindow.bl.GetBus(myBus.License);
                MainGrid.DataContext = myBus;
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }


        /// <summary>
        /// Event when click on test button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (myBus.BusStatus == BO.Status.Ready)
            {
                myBus.TestDate = DateTime.Now;
                myBus.Fuel = 1200;
                myBus.KmFromTest = 0;
                try
                {
                    MainWindow.bl.UpdateBus(myBus);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                myBus = MainWindow.bl.GetBus(myBus.License);
                MainGrid.DataContext = myBus;
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }
    }
}
