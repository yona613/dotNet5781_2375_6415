using BLApi;
using System;
using System.Windows;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        IBL bl = BLFactory.GetBL();
        BO.Bus myBus;
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
                    bl.UpdateBus(myBus);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                myBus = bl.GetBus(myBus.License);
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
                    bl.UpdateBus(myBus);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                myBus = bl.GetBus(myBus.License);
                MainGrid.DataContext = myBus;
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }
    }
}
