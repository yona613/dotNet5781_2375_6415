using System.Windows;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for LineData.xaml
    /// </summary>
    public partial class LineData : Window
    {
        BO.LineToShow myLine;
        public LineData(BO.LineToShow tmpLine)
        {
            InitializeComponent();
            myLine = tmpLine;
            MainGrid.DataContext = myLine;
        }

        /// <summary>
        /// Map Button clicked
        /// open map window to show the map of line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new LineMap(myLine).ShowDialog();
        }
    }
}
