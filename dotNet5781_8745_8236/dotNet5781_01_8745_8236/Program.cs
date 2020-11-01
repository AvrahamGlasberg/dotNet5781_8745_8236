using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_8745_8236
{
    class Program
    {
        /// <summary>
        /// this func read licsene and check is correctnce
        /// </summary>
        /// <param name="lic">licsene number of the bus</param>
        /// <returns>the correctnce of the lics number</returns>
        public static bool ReadLicense(ref int lic)
        {
            Console.Write("Enter license: ");
            if (!int.TryParse(Console.ReadLine(), out lic) || (lic.ToString().Length != 7 && lic.ToString().Length != 8))
            {
                Console.WriteLine("Illegal input");
                return false;
            }
            return true;
        }
        static void Main(string[] args)
        {
            char ch; // the user's choice
            int lic = 0; // the licsene number
            // create a list of buses
            List<Bus> lst = new List<Bus>();
            do
            {
                Console.WriteLine("\nFor adding new bus, enter 'a'. \nFor select bus for a ride enter 'r'. \nFor treatment or refueling enter 't'. \nFor printing details for all buses since last treatment enter 'p'. \nFor exit enter 'e'. ");
                char.TryParse(Console.ReadLine(), out ch); // reading char from user
                switch (ch)
                {
                    case 'a'://we assume that inputs of 2 identical license is not error, and in that case treatment/refuels will be for the first bus.
                        if (!ReadLicense(ref lic)) 
                            break;
                        Console.Write("Enter starting date: ");
                        DateTime date;
                        if(!DateTime.TryParse(Console.ReadLine(), out date) || (date.Year < 2018 && lic.ToString().Length != 7) || (date.Year >= 2018 && lic.ToString().Length != 8) || date > DateTime.Now)
                        { // incorrect info from user 
                            Console.WriteLine("Illegal input");
                            break;
                        }
                        lst.Add(new Bus(lic, date)); // insert bus to the list
                        break;
                    case 'r':
                        if (!ReadLicense(ref lic))
                            break;
                        Random rand = new Random(DateTime.Now.Millisecond); 
                        int ride = rand.Next(1200); // random number between 0 to 1200
                        bool found = false; // licsene number exist in the list
                        foreach(Bus curBus in lst)
                        {
                            if(curBus.LicNum == lic)
                            {
                                found = true;
                                if(ride + curBus.KmFromFuel > 1200) 
                                {// need to refuel
                                    Console.WriteLine("Not enough fuel!");
                                    break;
                                }
                                DateTime now = DateTime.Now; 

                                if(ride + curBus.KmFromtreat > 20000 || now.AddYears(-1) > curBus.Start)
                                { // need to get treatment
                                    Console.WriteLine("Bus need treatment!");
                                    break;
                                }
                                // updating the bus info
                                curBus.KmTotal += ride;
                                curBus.KmFromtreat += ride;
                                curBus.KmFromFuel += ride;
                                break;
                            }
                        }
                        if(!found)
                            Console.WriteLine("Bus does not exist!");
                        break;
                    case 't':
                        char choice; // treatment or refuel 
                        Console.WriteLine("Enter 't' for treatment and 'f' for fuel:");
                        if(!char.TryParse(Console.ReadLine(), out choice) || (choice != 'f' && choice != 't'))
                        {
                            Console.WriteLine("Illegal input");
                            break;
                        }
                        if (!ReadLicense(ref lic))
                            break;
                        bool exist = false;
                        foreach(Bus curBus in lst)
                        {
                            if(curBus.LicNum == lic)
                            {
                                if (choice == 't')
                                    // updating the treatment details
                                    curBus.treatment();
                                else
                                    // updating the refual details
                                    curBus.reFual();
                                exist = true;
                                break;
                            }
                        }
                        if(!exist)
                            Console.WriteLine("Bus does not exists!");
                        break;
                    case 'p':
                        foreach(Bus cur in lst)
                        {
                            cur.print();
                        }
                        break;
                    case 'e': // exit
                        break;
                    default:
                        Console.WriteLine("Illegal input");
                        break;
                }
            } while (ch != 'e');
        }
    }
}
