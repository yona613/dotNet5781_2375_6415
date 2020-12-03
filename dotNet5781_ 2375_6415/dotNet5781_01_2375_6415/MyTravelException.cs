using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_2375_6415
{
    /// <summary>
    /// A new exception class for travel handling exceptions
    /// </summary>
    [Serializable]
    public class MyTravelException : Exception
    {
        public MyTravelException() : base() { }
        public MyTravelException(string message) : base(message) { }
        public MyTravelException(string message, Exception inner) : base(message, inner) { }
        protected MyTravelException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
