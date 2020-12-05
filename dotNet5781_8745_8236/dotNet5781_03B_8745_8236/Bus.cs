using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_8745_8236
{
    public enum BusState
    {
        Ready, Driving, Refueling, Treatment
    }
    public class Bus
    {

        private BusState state;
        public BusState State { get { return state; } set { state = value; }  }


        private int _kmTotal;//total km the bus drove
        public int KmTotal { get { return _kmTotal; } set { _kmTotal = value; } }//property

        private int _kmFromFuel;//km the bus drove since last refuel
        public int KmFromFuel { get { return _kmFromFuel; } set { _kmFromFuel = value; } }//property
        private int _kmFromtreat;//km the bus drove since last treatment
        public int KmFromtreat { get { return _kmFromtreat; } set { _kmFromtreat = value; } }//property
        private int _licNum;//licesne number
        public int LicNum { get { return _licNum; } }//property

        private DateTime _start; //date after last treatment/started working
        public DateTime Start { get { return _start; } set { _start = value; } }//property
        private DateTime _lastTreat; //date from last treatment
        public DateTime LastTreat { get { return _lastTreat; } set { _lastTreat = value; } }//property
        /// <summary>
        /// constructor for Bus cless
        /// </summary>
        /// <param name="licNum">the Bus's licesnse number</param>
        /// <param name="date">the date that the bus started working </param>
        public Bus(int licNum, DateTime startDate, int kmT = 0, int kmFromFuel = 0, int kmFromtreat = 0, DateTime lastTreat = default(DateTime))
        {
            _kmTotal = kmT;
            _kmFromFuel = kmFromFuel;
            _kmFromtreat = kmFromtreat;
            _licNum = licNum;
            _start = startDate;
            if (lastTreat == default(DateTime))
                _lastTreat = startDate;
            else
                _lastTreat = lastTreat;
            state = BusState.Ready;
        }

        /// <summary>
        /// This function refuels the bus and lowers it's KmFromfuel to 0.
        /// </summary>
        public void reFual() { _kmFromFuel = 0; }

        /// <summary>
        /// This function treatments the bus and lowers it's KmFromtreat to 0.
        /// </summary>
        public void treatment() { _kmFromtreat = 0; _lastTreat = DateTime.Now; }

        public string LicToString()
        {
            string strLic = LicNum.ToString();
            if (strLic.Length == 7)
            {
                strLic = strLic.Insert(2, "-");
                strLic = strLic.Insert(6, "-");
            }
            else
            {
                strLic = strLic.Insert(3, "-");
                strLic = strLic.Insert(6, "-");
            }
            return strLic;
        }
    }
}

