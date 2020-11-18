using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2375_6415
{
    /// <summary>
    /// A new exception class for cases where we didn't find the product that the user entered
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}