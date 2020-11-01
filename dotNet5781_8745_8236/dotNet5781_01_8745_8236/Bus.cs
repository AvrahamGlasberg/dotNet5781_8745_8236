using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_8745_8236
{
    class Bus
    {
        private int kmTotal;
        private int kmFromFuel;
        private int kmFromtreat;
        private string licNum;
        private DateTime start;
        Bus(string _licNum, DateTime date) 
        { 
        kmTotal = 0;
        kmFromFuel = 0;
         kmFromtreat = 0;
        licNum = _licNum;
        start = date;
        }
        public void reFual() { kmFromFuel = 0; }
        public void treatment() { kmFromtreat = 0; start = DateTime.Now; }







    }


}
