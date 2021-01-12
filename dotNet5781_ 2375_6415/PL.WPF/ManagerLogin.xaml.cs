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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BLApi;

namespace PL.WPF
{
    /// <summary>  
    /// Interaction logic for MainWindow.xaml  
    /// </summary>   
    public partial class ManagerLogin : Window
    {
        public static IBL bl;
        public ManagerLogin()
        {
            InitializeComponent();
            bl = BLFactory.GetBL();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxName.Text.Length == 0)
            {
                errormessage.Text = "Enter a Name.";
                textBoxName.Focus();
            }
            else
            {
                string name = textBoxName.Text;
                string password = passwordBox1.Password;
                try
                {
                    var user = bl.GetUser(name);
                    if (password == user.Password)
                    {
                        new MainWindow().Show();
                        Close();
                    }
                    else
                    {
                        errormessage.Text = "Sorry! Please enter correct Password";
                        passwordBox1.Clear();
                    }
                }
                catch (BO.BOBadUserException)
                {
                    errormessage.Text = "Sorry! Please enter existing UserName/Password";
                    textBoxName.Clear();
                    passwordBox1.Clear();
                    errormessage.TextWrapping = TextWrapping.WrapWithOverflow;
                }
            }
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            new Registration().ShowDialog();
            Close();
        }

        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (textBoxName.Text.Length == 0)
                {
                    errormessage.Text = "Enter a Name.";
                    textBoxName.Focus();
                }
                else
                {
                    string name = textBoxName.Text;
                    string password = passwordBox1.Password;
                    try
                    {
                        var user = bl.GetUser(name);
                        if (password == user.Password)
                        {
                            new MainWindow().Show();
                            Close();
                        }
                        else
                        {
                            errormessage.Text = "Sorry! Please enter correct Password";
                            passwordBox1.Clear();
                        }
                    }
                    catch (BO.BOBadUserException)
                    {
                        errormessage.Text = "Sorry! Please enter existing UserName/Password";
                        textBoxName.Clear();
                        passwordBox1.Clear();
                        errormessage.TextWrapping = TextWrapping.WrapWithOverflow;
                    }
                }
            }
        }
    }
}