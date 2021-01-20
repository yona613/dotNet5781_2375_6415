using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace BLApi
{
    public class BLFactory
    {
        /// <summary>
        /// Returns instance of bl
        /// </summary>
        /// <returns></returns>
        public static IBL GetBL()
        {
            return BLImp.Instance;
        }
    }
}