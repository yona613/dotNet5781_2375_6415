using System.Windows;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        public static Bus tmpBus1;
        public BusData(Bus tmpBus)
        {
            InitializeComponent();
            MainGrid.DataContext = tmpBus;
            tmpBus1 = tmpBus;
            foreach (var item in MainWindow.myBwList)
            {
                if (item.MyBus.License==tmpBus1.License)
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

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            if (((Bus)MainGrid.DataContext).BusStatus == Status.READY)
            {
                MyBw bw = new MyBw(((Bus)MainGrid.DataContext), "Refuel");
                refuelPB.DataContext = bw;
                Counter.DataContext = bw;
                bw.bW.RunWorkerCompleted += MainWindow.OnProgressCompleted;
                MainWindow.myBwList.Add(bw);
                bw.Start();
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (((Bus)MainGrid.DataContext).BusStatus == Status.READY)
            {
                MyBw bw = new MyBw(((Bus)MainGrid.DataContext), "Testing");
                TestPB.DataContext = bw;
                Counter.DataContext = bw;
                bw.bW.RunWorkerCompleted += MainWindow.OnProgressCompleted;
                MainWindow.myBwList.Add(bw);
                bw.Start();
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }
    }
}
