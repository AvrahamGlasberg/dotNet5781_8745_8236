using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_8745_8236
{
    class Bus
    {
        private int _kmTotal;
        public int KmTotal { get { return _kmTotal; } set { _kmTotal = value; } }

        private int _kmFromFuel;
        public int KmFromFuel { get { return _kmFromFuel; } set { _kmFromFuel = value; } }
        private int _kmFromtreat;
        public int KmFromtreat { get { return _kmFromtreat; } set { _kmFromtreat = value; } }
        private int _licNum;
        public int LicNum { get { return _licNum; } }

        private DateTime _start;
        public DateTime Start { get { return _start; } set { _start = value; } }
        public Bus(int licNum, DateTime date) 
        { 
        _kmTotal = 0;
        _kmFromFuel = 0;
         _kmFromtreat = 0;
        _licNum = licNum;
        _start = date;
        }
        public void reFual() { _kmFromFuel = 0; }
        public void treatment() { _kmFromtreat = 0; _start = DateTime.Now; }

        public void print()
        {
            string strLic = LicNum.ToString();
            if(strLic.Length == 7)
            {
                strLic = strLic.Insert(2, "-");
                strLic = strLic.Insert(6, "-");
            }
            else
            {
                strLic = strLic.Insert(3, "-");
                strLic = strLic.Insert(6, "-");
            }
            Console.WriteLine("{0} drove {1} Km since last treatment.", strLic, KmFromtreat );
        }







    }


}
