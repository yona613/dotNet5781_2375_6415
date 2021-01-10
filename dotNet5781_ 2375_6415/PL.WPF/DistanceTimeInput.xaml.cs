using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for DistanceInput.xaml
    /// </summary>
    public partial class DistanceTimeInput : Window
    {
        BO.LineStationToShow myStation;
        int lineNumber;
        public DistanceTimeInput(BO.LineStationToShow tmpStation, int tmpLineNumber)
        {
            InitializeComponent();
            myStation = tmpStation;
            lineNumber = tmpLineNumber;
            mainGrid.DataContext = myStation;
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.UpdateDistanceAndTime(myStation, lineNumber);
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
