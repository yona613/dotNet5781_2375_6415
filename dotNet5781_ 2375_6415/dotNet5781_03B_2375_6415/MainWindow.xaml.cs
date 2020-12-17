using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// List of Buses to be printed on screen
        /// </summary>
        public static List<Bus> myBusList = Program.CreateBusList();

        /// <summary>
        /// List of custom class to handle background workers
        /// </summary>
        public static List<MyBw> myBwList = new List<MyBw> { };

        public MainWindow()
        {
            InitializeComponent();
            //adds all buses to be displayed
            foreach (var item in myBusList)
            {
                BusList.Items.Add(item);
            }
        }

        /// <summary>
        /// Event when click on bus button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBusbt_Click(object sender, RoutedEventArgs e)
        {
            //creates new window to add bus
            Window addWin = new Window1();
            addWin.ShowDialog(); //opens the new window
        }


        /// <summary>
        /// event when click on travel button of a bus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Travel_Click(object sender, RoutedEventArgs e)
        {
            Bus current = ((Bus)(((Button)sender).DataContext)); //gets current bus
            if (current.BusStatus == Status.READY) //if bus can travel
            {
                Window tmpWin = new Travel(current); //opens new window to get the km 
                tmpWin.ShowDialog();
                foreach (var item in MainWindow.myBwList) //when returning from window searches the background worker that was opened
                {
                    if (item.MyBus.License == current.License)// update relevant objects on grid according to background worker
                    {
                        if (item.MyStatus == "Refuel")
                        {
                            Finditem<ProgressBar>(current, "refuelPB1").DataContext = item; //finds object using custom function
                        }
                        Finditem<Label>(current, "Counter").DataContext = item;//finds object using custom function
                    }
                }
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }  
        }


        /// <summary>
        /// event when Refuel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            Bus current = ((Bus)(((Button)sender).DataContext)); //gets current bus
            if (current.BusStatus == Status.READY) 
            {
                MyBw bw = new MyBw(current, "Refuel"); //creates new custom background worker 
                Finditem<ProgressBar>(current, "refuelPB1").DataContext = bw; ; //finds progress bar on the grid using custom function and updates it
                Finditem<Label>(current, "Counter").DataContext = bw;  //finds Label on the grid using custom function and updates it
                bw.bW.RunWorkerCompleted += OnProgressCompleted; //adds event of progress completed
                myBwList.Add(bw); //adds background worker to the list of bkgrnd wkers
                bw.Start(); //starts bkgrnd worker using custom functions of the class
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }


        /// <summary>
        /// event when double click on a bus in the list (shows data of the bus in a new window)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BusData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus current = BusList.SelectedItem as Bus; //gets current bus
            {
                BusData tmpWin = new BusData(current); //opens new window to show data of that bus
                tmpWin.ShowDialog();
                foreach (var item in MainWindow.myBwList)  //when returning from window searches the background worker that was opened
                {
                    if (item.MyBus.License == (current.License))
                    {
                        if (item.MyStatus=="Refuel")
                        {
                            Finditem<ProgressBar>(current, "refuelPB1").DataContext = item; //finds object using custom function and updates it
                        }
                        Finditem<Label>(current, "Counter").DataContext = item; //finds object using custom function and updates it
                    }
                }
            }
        }

        /// <summary>
        /// custom function from internet used to get a object out from data template
        /// </summary>
        /// <typeparam name="childItem"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            int k = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// Generic function used to get a object out from data template to be able to update it from code
        /// </summary>
        /// <typeparam name="A">type of the object we look for</typeparam>
        /// <param name="item">Name of the item presented by the data template</param>
        /// <param name="str">Name of the object in xaml code</param>
        /// <returns></returns>
        public A Finditem<A>(object item, string str)
        {

            ListBoxItem myListBoxItem = (ListBoxItem)(BusList.ItemContainerGenerator.ContainerFromItem(item));

            // Getting the ContentPresenter of myListBoxItem
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);

            // Finding textBlock from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            A myObject = (A)myDataTemplate.FindName(str, myContentPresenter);
            return myObject; //returns the object
        }

        /// <summary>
        /// fonction to be added to completed event of background worker to remove it from the list of background workers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var item in myBwList) //goes over the list of background workers
            {
                if (item.MyBus.bw == sender as BackgroundWorker)
                {
                    myBwList.Remove(item); //removes the specific background worker that finished
                }
            }
        }
    }
}
