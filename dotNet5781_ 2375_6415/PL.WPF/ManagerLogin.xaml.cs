using System.Windows;
using System.Windows.Input;
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
        /// <summary>
        /// Login Button event
        /// </summary>
        /// <param name="sender">Login Button</param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        /// <summary>
        /// Register button event
        /// opens registration's page
        /// </summary>
        /// <param name="sender">Register Button</param>
        /// <param name="e"></param>
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            new Registration().ShowDialog();
            Close();
        }

        /// <summary>
        /// When enter key pushed then submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        /// <summary>
        /// login implementation
        /// checks that fields are full
        /// then send login query to bl and checks if user exists and password is true
        /// if yes then opens program eklse error message
        /// </summary>
        private void Login()
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