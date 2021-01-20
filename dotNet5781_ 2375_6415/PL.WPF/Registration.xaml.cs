using System.Windows;
using System.Windows.Input;
using BLApi;

namespace PL.WPF
{
    /// <summary>  
    /// Interaction logic for Registration.xaml  
    /// </summary>  
    public partial class Registration : Window
    {
        bool IsManager;
        public static IBL bl;
        public Registration(bool tmpIsMAnager)
        {
            InitializeComponent();
            IsManager = tmpIsMAnager;
            bl = BLFactory.GetBL();
        }

        /// <summary>
        /// When click on login opens login window and closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (IsManager)
            {
                new ManagerLogin().Show();
            }
            else
            {
                new UserLogin().Show();
            }
            Close();
        }

        /// <summary>
        /// Button reset event
        /// </summary>
        /// <param name="sender">Reset Button</param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// to reset registration
        /// </summary>
        public void Reset()
        {
            textBoxName.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }

        /// <summary>
        /// Close Button event
        /// </summary>
        /// <param name="sender">Close Button</param>
        /// <param name="e"></param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Submit Button event
        /// </summary>
        /// <param name="sender">Submit Button</param>
        /// <param name="e"></param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Submition();
        }

        /// <summary>
        /// Submition of registration
        /// checks that fields are full and sends registration to bl
        /// </summary>
        private void Submition()
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
                BO.User user = new BO.User() //creates new user to register
                {
                    UserName = textBoxName.Text,
                    Password = passwordBox1.Password
                };
                if (IsManager)
                {
                    user.Permission = BO.Permit.Admin;
                }
                else
                {
                    user.Permission = BO.Permit.User;
                }
                errormessage.Text = "";
                try
                {
                    bl.AddUser(user); //registers user
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

        /// <summary>
        /// When enter key then submit form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordBoxConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Submition();
            }
        }
    }
}