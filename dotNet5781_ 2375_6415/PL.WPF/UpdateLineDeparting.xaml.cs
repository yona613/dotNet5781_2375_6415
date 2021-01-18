using BLApi;
using System;
using System.Windows;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for UpdateLineDeparting.xaml
    /// </summary>
    public partial class UpdateLineDeparting : Window
    {
        IBL bl = BLFactory.GetBL();
        BO.LineDeparting myLineDeparting;
        public UpdateLineDeparting(BO.LineDeparting tmpLineDeparting)
        {
            InitializeComponent();
            myLineDeparting = tmpLineDeparting;
            formGrid.DataContext = tmpLineDeparting;
        }

        /// <summary>
        /// Event when Apply button clicked
        /// Updates Line departing by sending update to bl
        /// </summary>
        /// <param name="sender">Update Button</param>
        /// <param name="e"></param>
        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateLineDeparting(myLineDeparting);
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
