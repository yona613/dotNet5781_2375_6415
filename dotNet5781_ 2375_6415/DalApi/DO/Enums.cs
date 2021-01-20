using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// activity of entity
    /// </summary>
    public enum Activity { On, Off}
    /// <summary>
    /// status of bus
    /// </summary>
    public enum Status { Ready, Travelling, Refueling, Testing }
    /// <summary>
    /// area of line
    /// </summary>
    public enum Area { General, North, South, Center, Jerusalem };
    /// <summary>
    /// permit of user/manager
    /// </summary>
    public enum Permit { Admin, User }
}