using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Implements Automatic key for busInTravel and LineTiming
    /// </summary>
    public static class Config
    {
        private static int number = 1;
        public static int Number
        {
            get
            {
                return number++;
            }                
        }
    }
}
