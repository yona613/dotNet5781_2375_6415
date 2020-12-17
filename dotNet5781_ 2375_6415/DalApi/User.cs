using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public enum Permit { ADMIN, USER}
    public class User
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private Permit permission;

        public Permit Permission
        {
            get { return permission; }
            set { permission = value; }
        }
    }
}
