using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BO;
using BLApi;
using System.Threading;

namespace PL.WPF
{
    /// <summary>
    /// Interaction logic for StationData.xaml
    /// </summary>
    public partial class StationData : Window
    {
        BO.StationToShow myStation;
        //observable collection implementing digital panel
        ObservableCollection<LineTiming> myLineTimings = new ObservableCollection<LineTiming>() { };
        IBL bl = BLFactory.GetBL();

        public StationData(StationToShow tmpStation)
        {
            InitializeComponent();
            myStation = tmpStation;
            mainGrid.DataContext = tmpStation;
            digitalPanelDataGrid.ItemsSource = myLineTimings;
            digitalPanelDataGrid.Items.SortDescriptions.Add(new SortDescription("ArrivalTime", ListSortDirection.Ascending));
            if (myStation.DigitalPanel == true) //if line station has digital panel
            {
                //initialize the observation for digital panel
                bl.SetStationPanel(myStation.StationId, new Action<LineTiming>(x =>
                {
                    //action that adds all Line Timings to collection (in bl)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        bool flag = true;//to check if line timing was found
                        for (int i = 0; i < myLineTimings.Count; i++) //checks if new line timing already exists
                        {
                            LineTiming tmpLineTiming = myLineTimings[i];
                            if (tmpLineTiming.Key == x.Key)
                            {
                                myLineTimings.Remove(tmpLineTiming);//remove old from list and add new if not 0
                                if (x.ArrivalTime != TimeSpan.Zero)
                                {
                                    myLineTimings.Add(x);
                                }
                                flag = false; //line Timing was found
                            }
                        }
                        if (flag && x.ArrivalTime != TimeSpan.Zero) //if timing wasn't find and time != 0
                        {
                            myLineTimings.Add(x); //adds timing to list
                        }                       
                    }));
                }));
            }
        }

        /// <summary>
        /// Event when closing the window
        /// sets observed station to -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (bl.IsSimulator())
            {
                bl.SetStationPanel(-1, null); //set observed station to -1
            }
        }
    }
}
