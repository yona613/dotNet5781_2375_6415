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
    /// Interaction logic for AddLineDeparting.xaml
    /// </summary>
    public partial class AddLineDeparting : Window
    {
        BO.LineDeparting myLineDeparting = new BO.LineDeparting() { StartTime = new TimeSpan(0, 0, 0), StopTime = new TimeSpan(0, 0, 0), Frequency = new TimeSpan(0, 0, 0) };
        public AddLineDeparting(int lineNumber)
        {
            InitializeComponent();
            myLineDeparting.LineNumber = lineNumber;
            formGrid.DataContext = myLineDeparting;
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.AddLineDeparting(myLineDeparting);
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
