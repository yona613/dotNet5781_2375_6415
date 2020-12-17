using System.Windows;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        //to be able to update the main window from that window
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        //public static Bus tmpBus1;
        public BusData(Bus tmpBus)
        {
            InitializeComponent();
            MainGrid.DataContext = tmpBus; //initialize data on the window
            //tmpBus1 = tmpBus;
            foreach (var item in MainWindow.myBwList) //searches for background worker working on that bus to update window
            {
                if (item.MyBus.License==tmpBus.License)
                {
                    if (item.MyBus.BusStatus == Status.OILING)
                    {
                        refuelPB.DataContext = item;
                    }
                    if (item.MyBus.BusStatus == Status.TESTING)
                    {
                        TestPB.DataContext = item;
                    }
                    Counter.DataContext = item;
                }
            }    
        }

        /// <summary>
        /// Event when click on refuel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refuel_Click(object sender, RoutedEventArgs e) 
        {
            if (((Bus)MainGrid.DataContext).BusStatus == Status.READY)
            {
                MyBw bw = new MyBw(((Bus)MainGrid.DataContext), "Refuel"); //creates new bkgrnd worker to handle this refuelling
                refuelPB.DataContext = bw; //binds element in window to the data of background worker
                Counter.DataContext = bw; //binds element in window to the data of background worker
                bw.bW.RunWorkerCompleted += MainWindow.OnProgressCompleted; //adds events to bkgrnd worker
                MainWindow.myBwList.Add(bw); //adds bkgrnd worker to the list
                bw.Start(); //runs bkgrnd worker
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }


        /// <summary>
        /// Event when click on test button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (((Bus)MainGrid.DataContext).BusStatus == Status.READY)
            {
                MyBw bw = new MyBw(((Bus)MainGrid.DataContext), "Testing");//creates new bkgrnd worker to handle this testing
                TestPB.DataContext = bw; //binds element in window to the data of background worker
                Counter.DataContext = bw;//binds element in window to the data of background worker
                bw.bW.RunWorkerCompleted += MainWindow.OnProgressCompleted; //adds events to bkgrnd worker
                MainWindow.myBwList.Add(bw);//adds bkgrnd worker to the list
                bw.Start(); //runs bkgrnd worker
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }
    }
}
