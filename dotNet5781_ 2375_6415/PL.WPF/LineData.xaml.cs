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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new LineMap(myLine).ShowDialog();
        }
    }
}
