using BLApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public static IBL bl;
        public UserWindow()
        {
            InitializeComponent();
            bl = BLFactory.GetBL();
            lineDataGrid.ItemsSource = bl.GetAllLinesToShow();
            stationDataGrid.ItemsSource = bl.GetAllStations();
        }


        /// <summary>
        /// Event when object double clicked in list
        /// opens data window of that object
        /// </summary>
        /// <param name="sender">Mouse double Click</param>
        /// <param name="e"></param>
        private void ListDoubleMouseClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid).SelectedItem is BO.Station)
            {
                new StationData(bl.getStationToShow((stationDataGrid.SelectedItem as BO.Station).StationId)).ShowDialog();
            }
            else if ((sender as DataGrid).SelectedItem is BO.LineToShow)
            {
                new LineData(lineDataGrid.SelectedItem as BO.LineToShow).ShowDialog();
            }
        }
    }
}
