using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi
{
    public class BLFactory
    {
        public static IBL GetBL()
        {
            return new BLImp();
        }
    }
}
