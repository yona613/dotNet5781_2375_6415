using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Obsolete("Class not used in program, needs more implementation")]
    public class UserTrip
    {
        public string UserName { get; set; }

        public int LineNumber { get; set; }

        public int FirstStation { get; set; }

        public DateTime StartTime { get; set; }

        public int LastStation { get; set; }

        public DateTime StopTime { get; set; }

    }
}