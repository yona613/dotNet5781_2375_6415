using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public static class Tools
    {
        public static void foo<T> (this T t)
        {
            Console.WriteLine("yona");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(() => Console.WriteLine("yona"));
            for (int i = 1; i <= 3; i++)
            {
                thread.Start();
            }
        }
    }
}
