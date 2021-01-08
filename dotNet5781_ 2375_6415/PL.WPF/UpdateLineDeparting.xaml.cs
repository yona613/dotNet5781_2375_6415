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
    /// Interaction logic for UpdateLineDeparting.xaml
    /// </summary>
    public partial class UpdateLineDeparting : Window
    {
        BO.LineDeparting myLineDeparting;
        public UpdateLineDeparting(BO.LineDeparting tmpLineDeparting)
        {
            InitializeComponent();
            myLineDeparting = tmpLineDeparting;
            formGrid.DataContext = tmpLineDeparting;
        }

        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.UpdateLineDeparting(myLineDeparting);
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
