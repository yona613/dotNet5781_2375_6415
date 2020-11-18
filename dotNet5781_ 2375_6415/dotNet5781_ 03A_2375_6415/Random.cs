using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781__03A_2375_6415
{
    /// <summary>
    /// Class to implement random number
    /// </summary>
    public static class myRandom
    {
        public static Random r = new Random(DateTime.Now.Millisecond);
    }
}
