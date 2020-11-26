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
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        public static Bus tmpBus1;
        public BusData(Bus tmpBus)
        {
            InitializeComponent();
            MainGrid.DataContext = tmpBus;
            tmpBus1 = tmpBus;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((Bus)MainGrid.DataContext).Fuel();
            MainGrid.DataContext = null;
            MainGrid.DataContext = tmpBus1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Bus)MainGrid.DataContext).Test();
            MainGrid.DataContext = null;
            MainGrid.DataContext = tmpBus1;
        }

    }
}
