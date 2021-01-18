using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL;
using BLApi;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for LineInTravel.xaml
    /// </summary>
    public partial class LineInTravel : Window
    {
        public static IBL bl = BLFactory.GetBL();
        public LineInTravel()
        {
            InitializeComponent();
            myDataGrid.ItemsSource = bl.GetLineInTravel();
            int threads1;
            int threads2;
            ThreadPool.GetMaxThreads(out threads1, out threads2);
            thread.Content = threads1;
        }
    }
}
