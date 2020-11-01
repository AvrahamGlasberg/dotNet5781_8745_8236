using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_8745_8236
{
    class Bus
    {
        private int _kmTotal;//total km the bus drove
        public int KmTotal { get { return _kmTotal; } set { _kmTotal = value; } }//property

        private int _kmFromFuel;//km the bus drove since last refuel
        public int KmFromFuel { get { return _kmFromFuel; } set { _kmFromFuel = value; } }//property
        private int _kmFromtreat;//km the bus drove since last treatment
        public int KmFromtreat { get { return _kmFromtreat; } set { _kmFromtreat = value; } }//property
        private int _licNum;//licesne number
        public int LicNum { get { return _licNum; } }//property

        private DateTime _start;//date after last treatment/started working
        public DateTime Start { get { return _start; } set { _start = value; } }//property

        /// <summary>
        /// constructor for Bus cless
        /// </summary>
        /// <param name="licNum">the Bus's licesnse number</param>
        /// <param name="date">the date that the bus started working </param>
        public Bus(int licNum, DateTime date) 
        { 
        _kmTotal = 0;
        _kmFromFuel = 0;
         _kmFromtreat = 0;
        _licNum = licNum;
        _start = date;
        }

        /// <summary>
        /// This function refuels the bus and lowers it's KmFromfuel to 0.
        /// </summary>
        public void reFual() { _kmFromFuel = 0; }

        /// <summary>
        /// This function treatments the bus and lowers it's KmFromtreat to 0.
        /// </summary>
        public void treatment() { _kmFromtreat = 0; _start = DateTime.Now; }

        /// <summary>
        /// This function prints the bus's license numbber and the Km since last treatment.
        /// </summary>
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
