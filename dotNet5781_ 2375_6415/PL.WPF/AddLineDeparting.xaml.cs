using BLApi;
using System;
using System.Windows;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for AddLineDeparting.xaml
    /// </summary>
    public partial class AddLineDeparting : Window
    {
        //to get input from window
        BO.LineDeparting myLineDeparting = new BO.LineDeparting() { StartTime = new TimeSpan(0, 0, 0), StopTime = new TimeSpan(0, 0, 0), Frequency = new TimeSpan(0, 0, 0) };
        IBL bl = BLFactory.GetBL();
        public AddLineDeparting(int lineNumber)
        {
            InitializeComponent();
            myLineDeparting.LineNumber = lineNumber;
            formGrid.DataContext = myLineDeparting;
        }


        /// <summary>
        /// Event when submit button clicked
        /// adds line departing to data by query to bl
        /// </summary>
        /// <param name="sender">Submit button</param>
        /// <param name="e"></param>
        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddLineDeparting(myLineDeparting);
                Close();
            }
            catch (BO.BOStopTimeException dataException)
            { 
                MessageBox.Show(dataException.Message);
                stopTimeTxb.Text = "00:00";
            }
            catch (BO.BOFrequencyException dataException)
            {
                MessageBox.Show(dataException.Message);
                frequencyTxb.Text = "00:00";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
