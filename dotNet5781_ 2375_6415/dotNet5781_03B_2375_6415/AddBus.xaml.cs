using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        public Window1()
        {
            InitializeComponent();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int tmpInt;
                int tmpKm;
                if (int.TryParse(GetlicenseTxb.Text, out tmpInt))
                {
                    try
                    {
                        if (OldBusCB.IsChecked == true)
                        {
                            if ((DateTime)OldBusSD.SelectedDate <= DateTime.Now)
                            {
                                if ((DateTime)OldBusTD.SelectedDate <= DateTime.Now && (DateTime)OldBusTD.SelectedDate >= (DateTime)OldBusSD.SelectedDate)
                                {
                                    if (((DateTime)OldBusSD.SelectedDate).Year < 2018)
                                    {
                                        if (tmpInt >= 1000000 && tmpInt < 10000000)
                                        {
                                            if (int.TryParse(GetKmTxb.Text, out tmpKm))
                                            {
                                                Bus tmpBus1 = new Bus((DateTime)OldBusSD.SelectedDate, tmpInt, 1200, tmpKm, Program.r.Next(0, 10001), (DateTime)OldBusTD.SelectedDate);
                                                foreach (var item in MainWindow.myBusList)
                                                {
                                                    if (item.License == tmpInt)
                                                    {
                                                        GetlicenseTxb.Clear();
                                                        throw new ArgumentException("Bus already exists, enter valid number !!!");
                                                    }
                                                }
                                                MainWindow.myBusList.Add(tmpBus1);
                                                wnd.BusList.Items.Add(tmpBus1);
                                                AddBusWin.Close();
                                            }
                                        }
                                        else
                                        {
                                            GetlicenseTxb.Clear();
                                            throw new ArgumentException("License not valid, enter valid number (license : 7 digits) !!!");
                                        }
                                    }
                                    else
                                    {
                                        if (tmpInt >= 10000000 && tmpInt < 100000000)
                                        {
                                            if (int.TryParse(GetKmTxb.Text, out tmpKm))
                                            {
                                                Bus tmpBus1 = new Bus((DateTime)OldBusSD.SelectedDate, tmpInt, 1200, tmpKm, Program.r.Next(0, 10001), (DateTime)OldBusTD.SelectedDate);
                                                foreach (var item in MainWindow.myBusList)
                                                {
                                                    if (item.License == tmpInt)
                                                    {
                                                        GetlicenseTxb.Clear();
                                                        throw new ArgumentException("Bus already exists, enter valid number !!!");
                                                    }
                                                }
                                                MainWindow.myBusList.Add(tmpBus1);
                                                wnd.BusList.Items.Add(tmpBus1);
                                                AddBusWin.Close();
                                            }
                                        }
                                        else
                                        {
                                            GetlicenseTxb.Clear();
                                            throw new ArgumentException("License not valid, enter valid number (license : 8 digits) !!!");
                                        }
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Invalid Test date (must be between start date and today's date !!!)");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Invalid Start date (must be before today's date !!!)");
                            }
                            Bus tmpBus = new Bus((DateTime)OldBusSD.SelectedDate, tmpInt, 1200, 0, 0, DateTime.Now);
                        }
                        else if (tmpInt >= 10000000 && tmpInt < 100000000) //can only add bus with 8 digits (year 2020 +)
                        {
                            Bus tmpBus = new Bus(DateTime.Now, tmpInt, 1200, 0, 0, DateTime.Now);
                            foreach (var item in MainWindow.myBusList)
                            {
                                if (item.License == tmpInt)
                                {
                                    GetlicenseTxb.Clear();
                                    throw new ArgumentException("Bus already exists, enter valid number !!!");
                                }
                            }
                            MainWindow.myBusList.Add(tmpBus);
                            wnd.BusList.Items.Add(tmpBus);
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LicenseGrid.IsEnabled = true;
            TestGrid.IsEnabled = true;
            KmLB.IsEnabled = true;
            GetKmTxb.IsEnabled = true;
        }

        private void OldBusCB_Unchecked(object sender, RoutedEventArgs e)
        {
            LicenseGrid.IsEnabled = false;
            TestGrid.IsEnabled = false;
            KmLB.IsEnabled = false;
            GetKmTxb.IsEnabled = false;
        }

        private void _PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "\r")
            {
                Regex myReg = new Regex("[^0-9]+");
                e.Handled = myReg.IsMatch(e.Text);
            }
            if (e.Handled)
            {
                MessageBox.Show($"Wrong Input !!!! \n {e.Text} is not a digit !!");
            }
        }
    }
}
