using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using dotNet5781_01_2375_6415;

namespace dotNet5781_03B_2375_6415
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Bus> myBusList = Program.CreateBusList();
        public static List<MyBw> myBwList = new List<MyBw> { };

        public MainWindow()
        {
            InitializeComponent();
            foreach (var item in myBusList)
            {
                BusList.Items.Add(item);
            }
        }

        private void AddBusbt_Click(object sender, RoutedEventArgs e)
        {
            Window addWin = new dotNet5781_03B_2375_6415.Window1();
            addWin.ShowDialog();
            //BusList.Items.Refresh();
        }

        private void Travel_Click(object sender, RoutedEventArgs e)
        {
            Bus current = ((Bus)(((Button)sender).DataContext));
            if (((Bus)(((Button)sender).DataContext)).BusStatus == Status.READY)
            {
                Window tmpWin = new Travel((Bus)(((Button)sender).DataContext));
                tmpWin.ShowDialog();
                foreach (var item in MainWindow.myBwList)
                {
                    if (item.MyBus.License == (current.License))
                    {
                        if (item.MyStatus == "Refuel")
                        {
                            Finditem<ProgressBar>(current, "refuelPB1").DataContext = item;
                        }
                        Finditem<Label>(current, "Counter").DataContext = item;
                    }
                }
                //Dispatcher.BeginInvoke(new Action(BusList.Items.Refresh));
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }  
        }

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            if (((Bus)(((Button)sender).DataContext)).BusStatus == Status.READY)
            {
                MyBw bw = new MyBw(((Bus)(((Button)sender).DataContext)), "Refuel");
                Finditem<ProgressBar>((((Button)sender).DataContext), "refuelPB1").DataContext = bw; ;
                Finditem<Label>((((Button)sender).DataContext), "Counter").DataContext = bw;
                myBwList.Add(bw);
                bw.Start();
            }
            else
            {
                MessageBox.Show("Bus already Busy !!!");
            }
        }

        private void Update(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(new Action(BusList.Items.Refresh));
        }

        private void BusData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ClickCount == 2)
            Bus current = BusList.SelectedItem as Bus;
            {
                BusData tmpWin = new BusData(current);
                tmpWin.ShowDialog();
                foreach (var item in MainWindow.myBwList)
                {
                    if (item.MyBus.License == (current.License))
                    {
                        if (item.MyStatus=="Refuel")
                        {
                            Finditem<ProgressBar>(current, "refuelPB1").DataContext = item;
                        }
                        Finditem<Label>(current, "Counter").DataContext = item;
                    }
                }
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
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

        public A Finditem<A>(object item, string str)
        {

            ListBoxItem myListBoxItem = (ListBoxItem)(BusList.ItemContainerGenerator.ContainerFromItem(item));

            // Getting the ContentPresenter of myListBoxItem
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);

            // Finding textBlock from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            A myLabel = (A)myDataTemplate.FindName(str, myContentPresenter);
            return myLabel;
        }
    }
}
