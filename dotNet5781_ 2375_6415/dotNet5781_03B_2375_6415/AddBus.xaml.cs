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
        //to be abale to update current window from this window
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        public Window1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event that is raised every key down
        /// used to raise when enter is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) //checks for enter key
            {
                int tmpInt; //tmp license
                int tmpKm;
                if (int.TryParse(GetlicenseTxb.Text, out tmpInt))
                {
                    try
                    {
                        if (OldBusCB.IsChecked == true) //if it is an old bus
                        {
                            if ((DateTime)OldBusSD.SelectedDate <= DateTime.Now) //checks the start date entered
                            {
                                if ((DateTime)OldBusTD.SelectedDate <= DateTime.Now && (DateTime)OldBusTD.SelectedDate >= (DateTime)OldBusSD.SelectedDate) // test date needs to be after start date and before now
                                {
                                    if (((DateTime)OldBusSD.SelectedDate).Year < 2018) //checks format of license number
                                    {
                                        if (tmpInt >= 1000000 && tmpInt < 10000000) //checks if good license number entered
                                        {
                                            if (int.TryParse(GetKmTxb.Text, out tmpKm))
                                            {
                                                Bus tmpBus1 = new Bus((DateTime)OldBusSD.SelectedDate, tmpInt, 1200, tmpKm, Program.r.Next(0, tmpKm), (DateTime)OldBusTD.SelectedDate); //creates bus to add to the list
                                                foreach (var item in MainWindow.myBusList) //checks that new bus does'nt already exists
                                                {
                                                    if (item.License == tmpInt)
                                                    {
                                                        GetlicenseTxb.Clear();
                                                        throw new ArgumentException("Bus already exists, enter valid number !!!");
                                                    }
                                                }
                                                MainWindow.myBusList.Add(tmpBus1); //adds bus to the list
                                                wnd.BusList.Items.Add(tmpBus1); //adds bus to list box of main window
                                                AddBusWin.Close(); //close window
                                            }
                                        }
                                        else
                                        {
                                            GetlicenseTxb.Clear();
                                            throw new ArgumentException("License not valid, enter valid number (license : 7 digits) !!!");
                                        }
                                    }
                                    else //other format of license
                                    {
                                        if (tmpInt >= 10000000 && tmpInt < 100000000) //checks that license is valid
                                        {
                                            if (int.TryParse(GetKmTxb.Text, out tmpKm))
                                            {
                                                Bus tmpBus1 = new Bus((DateTime)OldBusSD.SelectedDate, tmpInt, 1200, tmpKm, Program.r.Next(0, tmpKm), (DateTime)OldBusTD.SelectedDate); //creates bus to add to the list
                                                foreach (var item in MainWindow.myBusList)//checks that new bus does'nt already exists
                                                {
                                                    if (item.License == tmpInt) 
                                                    {
                                                        GetlicenseTxb.Clear();
                                                        throw new ArgumentException("Bus already exists, enter valid number !!!");
                                                    }
                                                }
                                                MainWindow.myBusList.Add(tmpBus1); //adds bus to the list
                                                wnd.BusList.Items.Add(tmpBus1); //adds bus to list box of main window
                                                AddBusWin.Close(); //close window
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
                            //Bus tmpBus = new Bus((DateTime)OldBusSD.SelectedDate, tmpInt, 1200, 0, 0, DateTime.Now);
                        }
                        else if (tmpInt >= 10000000 && tmpInt < 100000000) //can only add bus with 8 digits (year 2020 +)
                        {
                            Bus tmpBus = new Bus(DateTime.Now, tmpInt, 1200, 0, 0, DateTime.Now); //creates new bus to add to list
                            foreach (var item in MainWindow.myBusList) //checks that bus doesn't already exist
                            {
                                if (item.License == tmpInt)
                                {
                                    GetlicenseTxb.Clear();
                                    throw new ArgumentException("Bus already exists, enter valid number !!!");
                                }
                            }
                            MainWindow.myBusList.Add(tmpBus); //adds bus to list
                            wnd.BusList.Items.Add(tmpBus); //adds bus to list box of main window
                            AddBusWin.Close(); //close window
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


        /// <summary>
        /// handles the check box when checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //enable all fields for old bus data 
            LicenseGrid.Visibility = Visibility.Visible;
            LicenseGrid.IsEnabled = true;
            TestGrid.Visibility = Visibility.Visible;
            TestGrid.IsEnabled = true;
            KmLB.Visibility = Visibility.Visible;
            KmLB.IsEnabled = true;
            GetKmTxb.IsEnabled = true;
            GetKmTxb.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Handles when checkbox is unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OldBusCB_Unchecked(object sender, RoutedEventArgs e)
        {
            //disable all fields for old bus data 
            LicenseGrid.Visibility = Visibility.Collapsed;
            TestGrid.Visibility = Visibility.Collapsed;
            KmLB.Visibility = Visibility.Collapsed;
            GetKmTxb.Visibility = Visibility.Collapsed;
            LicenseGrid.IsEnabled = false;
            TestGrid.IsEnabled = false;
            KmLB.IsEnabled = false;
            GetKmTxb.IsEnabled = false;
        }


        /// <summary>
        /// Raised on every key of keyboard before it is displayed on screen
        /// used to check that only digits are entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "\r") //checks that key wasn't enter
            {
                Regex myReg = new Regex("[^0-9]+"); //gets regular expression that allows only digits
                e.Handled = myReg.IsMatch(e.Text); //checks taht key entered is regular expression
            }
            if (e.Handled) //if not regular expression
            {
                MessageBox.Show($"Wrong Input !!!! \n {e.Text} is not a digit !!");
            }
        }
    }
}
