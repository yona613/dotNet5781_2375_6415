using BLApi;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddBus : Window
    {

        public IBL bl = BLFactory.GetBL();
        public AddBus()
        {
            InitializeComponent();
            //gets brands list for adding new buses
            List<string> brands = new List<string>() { "Volvo", "Mercedes", "Merkavim", "Man", "Chevrolet", "Renault", "Ford" };
            brands.Sort();
            brandCbb.ItemsSource = brands;
        }

        /// <summary>
        /// Event that is raised every key down
        /// used to raise when enter is pressed
        /// gets input and adds bus by querying bl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            int tmpInt; //tmp license
            int tmpKm;
            if (int.TryParse(GetlicenseTxb.Text, out tmpInt))
            {
                if (OldBusCB.IsChecked == true) //input depends on old/new bus
                {
                    if (int.TryParse(GetKmTxb.Text, out tmpKm))
                    {
                        try
                        {
                            bl.AddBus(new BO.Bus() { License = tmpInt, Kilometrage = tmpKm, TestDate = (DateTime)OldBusTD.SelectedDate, LicenseDate = (DateTime)OldBusSD.SelectedDate, Fuel = 1200, KmFromTest = 0, AirConditionning = (bool)AcCbb.IsChecked, Brand = (string)brandCbb.SelectedItem });
                            Close();
                        }
                        catch (BO.BOArgumentLicenseException exception)
                        {
                            MessageBox.Show(exception.Message);
                            GetlicenseTxb.Clear();
                        }
                        catch (BO.BOArgumentLicenseDateException exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                        catch (BO.BOArgumentTestDateException exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Wrong Input !!!!");
                        GetlicenseTxb.Clear();
                    }
                }
                else
                {
                    try
                    {
                        bl.AddBus(new BO.Bus() { License = tmpInt, Kilometrage = 0, LicenseDate = DateTime.Now, TestDate = DateTime.Now, KmFromTest = 0, Fuel = 1200, AirConditionning = (bool)AcCbb.IsChecked, Brand = (string)brandCbb.SelectedItem });
                        Close();
                    }
                    catch (BO.BOArgumentLicenseException exception)
                    {
                        MessageBox.Show(exception.Message);
                        GetlicenseTxb.Clear();
                    }
                    catch (BO.BOArgumentLicenseDateException exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    catch (BO.BOArgumentTestDateException exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Wrong Input !!!!");
                GetlicenseTxb.Clear();
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
