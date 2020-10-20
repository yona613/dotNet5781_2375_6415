using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00__2375_6415
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome2375();
            Welcome6415();
            Console.ReadKey();
        }

        static partial void Welcome6415();

        private static void Welcome2375()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
