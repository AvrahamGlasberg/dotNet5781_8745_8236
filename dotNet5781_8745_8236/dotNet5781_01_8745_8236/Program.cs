using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_8745_8236
{
    class Program
    {
        static void Main(string[] args)
        {
            char ch;
            do
            {
                Console.WriteLine("For adding new bus, enter a. \nFor select bus for a ride enter r. \nFor treatment or refueling enter t. \nFor printing details for all buses since last treatment enter p. \nFor exit enter e. ");
                char.TryParse(Console.ReadLine(), out ch);
                switch (ch)
                {
                    case 'a':
                        break;
                    case 'r':
                        break;
                    case 't':
                        break;
                    case 'p':
                        break;
                    case 'e':
                        break;
                    default:
                        Console.WriteLine("Illegal Input");
                        break;
                }
            } while (ch != 'e');
        }
    }
}
