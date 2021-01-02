using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationToAdd
    {
        public int StationId { get; set; }

        public String Address { get; set; }

        override public String ToString()
        {
            return StationId.ToString() + ",  " + Address;
        }
    }
}
