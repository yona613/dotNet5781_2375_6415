using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace dotNet5781_03B_2375_6415
{
    public class MyTimer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TimeRest { get; set; }


    }
}
