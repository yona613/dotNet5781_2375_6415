using BLApi;
using System;
using System.Windows;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for DistanceInput.xaml
    /// </summary>
    public partial class DistanceTimeInput : Window
    {
        IBL bl = BLFactory.GetBL();
        BO.LineStationToShow myStation;
        int lineNumber;
        public DistanceTimeInput(BO.LineStationToShow tmpStation, int tmpLineNumber)
        {
            InitializeComponent();
            myStation = tmpStation;
            lineNumber = tmpLineNumber;
            mainGrid.DataContext = myStation;
        }

        /// <summary>
        /// Event when submit button clicked
        /// update pair station's distance and time
        /// </summary>
        /// <param name="sender">submit button</param>
        /// <param name="e"></param>
        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDistanceAndTime(myStation, lineNumber);
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
