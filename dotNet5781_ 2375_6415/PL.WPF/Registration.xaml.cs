using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BLApi;

namespace PL.WPF
{
    /// <summary>  
    /// Interaction logic for Registration.xaml  
    /// </summary>  
    public partial class Registration : Window
    {
        public static IBL bl;
        public Registration()
        {
            InitializeComponent();
            bl = BLFactory.GetBL();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ManagerLogin login = new ManagerLogin();
            login.Show();
            Close();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public void Reset()
        {
            textBoxName.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox1.Password.Length == 0)
            {
                errormessage.Text = "Enter password.";
                passwordBox1.Focus();
            }
            else if (passwordBoxConfirm.Password.Length == 0)
            {
                errormessage.Text = "Enter Confirm password.";
                passwordBoxConfirm.Focus();
            }
            else if (passwordBox1.Password != passwordBoxConfirm.Password)
            {
                errormessage.Text = "Confirm password must be same as password.";
                passwordBoxConfirm.Focus();
            }
            else
            {
                BO.User user = new BO.User()
                {
                    UserName = textBoxName.Text,
                    Password = passwordBox1.Password,
                    Permission = BO.Permit.Admin
                };
                errormessage.Text = "";
                try
                {
                    bl.AddUser(user);
                    errormessage.Text = "You have Registered successfully.";
                    Reset();
                }
                catch (BO.BOBadUserException exception)
                {
                    errormessage.Text = exception.Message;
                    Reset();
                }
            }
        }
    }
}