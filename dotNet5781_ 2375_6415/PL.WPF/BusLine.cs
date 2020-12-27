using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.WPF
{
    class BusLine
    {
        public int LineNumber { get; set; }
        public Area LineArea { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
    }
}
