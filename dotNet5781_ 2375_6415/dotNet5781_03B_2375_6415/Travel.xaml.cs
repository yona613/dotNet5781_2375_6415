using System.Windows;
using System.ComponentModel;
using System.Windows.Input;
using dotNet5781_01_2375_6415;
using System.Text.RegularExpressions;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for Travel.xaml
    /// </summary>
    public partial class Travel : Window
    {
        public Bus tmpBus1;
        public Travel(Bus tmpBus)
        {
            InitializeComponent();
            tmpBus1 = tmpBus;
        }

        /// <summary>
        /// Event raised for every key entered
        /// used to do when enter key is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) //if enter 
            {
                int kM = 0;
                if (int.TryParse(TravelTxb.Text, out kM))
                {
                    if (tmpBus1.BusStatus==Status.READY) 
                    {
                        MyBw bw = new MyBw(tmpBus1, "Travel",kM); //creates new bkgrnd worker to hanle the travel
                        bw.bW.RunWorkerCompleted += Travel_ProgressCompleted; //adds function to bkgrd worker events
                        bw.bW.RunWorkerCompleted += MainWindow.OnProgressCompleted; //adds function to bkgrd worker events
                        MainWindow.myBwList.Add(bw); //adds worker to list of bckgrnd workers
                        bw.Start(); //start worker
                    }
                    else
                    {
                        MessageBox.Show("Bus already Busy !!!");
                    }
                    Close(); //close window
                }
                else
                {
                    MessageBox.Show("Wrong Input !!!!");
                    TravelTxb.Clear();
                }
            }
        }

        /// <summary>
        /// Event raised when travel is completed
        /// used to show error of travel if occured (exceptions)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Travel_ProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
        }



        /// <summary>
        /// Raised on every key of keyboard before it is displayed on screen
        /// used to check that only digits are entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TravelTxb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex myReg = new Regex("[^0-9]+");//gets regular expression that allows only digits
            e.Handled = myReg.IsMatch(e.Text); //checks taht key entered is regular expression
            if (e.Handled) //if not regular expression
            { 
                MessageBox.Show($"Wrong Input !!!! \n {e.Text} is not a digit !!");
            }
        }
    }
}
