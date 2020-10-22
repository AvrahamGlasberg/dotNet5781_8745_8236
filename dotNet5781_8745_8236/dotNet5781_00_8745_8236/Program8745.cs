using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_8745_8236
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome8745();
            Welcome8236();
            Console.ReadKey();
        }
        static partial void Welcome8236();

        private static void Welcome8745()
        {
            string name;
            Console.WriteLine("Enter your name: ");
            name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
