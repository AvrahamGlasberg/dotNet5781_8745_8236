using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace dotNet5781_03B_8745_8236
{
    public enum BusState
    {
        Ready, Driving, Refueling, Treatment
    }
    public class Bus
    {
        private static MainWindow mainW;
        private BusData busDat;
        public BusData BusDat { get { return busDat; } set { busDat = value; } }

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

        private BackgroundWorker worker;
        private int counter;
        public int Counter { get {return counter;} }
        private int drives = 0;
        public int Drives { get {return drives;} }

        private int totalPass = 0;
        public int TotalPass { get {return totalPass;} }

        private int totalEarnings = 0;
        public int TotalEarnings { get {return totalEarnings;} }

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
                _lastTreat = DateTime.Now;
            else
                _lastTreat = lastTreat;
            state = BusState.Ready;
            counter = 0;
            busDat = null;
        }

        /// <summary>
        /// This function refuels the bus and lowers it's KmFromfuel to 0.
        /// </summary>
        public void ReFual() 
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync(12);
            state = BusState.Refueling;
            _kmFromFuel = 0;
            mainW.UpdateColor(this);
            if (BusDat != null)
                BusDat.updateColor();
            UpdateEarns(-500);
        }

        /// <summary>
        /// This function treatments the bus and lowers it's KmFromtreat to 0.
        /// </summary>
        public void Treatment()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync(144);
            state = BusState.Treatment;
            _kmFromtreat = 0;
            //how we undrstood the assignment.
            _kmFromFuel = 0;
            //
            _lastTreat = DateTime.Now;
            mainW.UpdateColor(this);
            if (BusDat != null)
                BusDat.updateColor();
            UpdateEarns(-2000);
        }

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
        public static void UpdateWin(MainWindow window)
        {
            mainW = window;
        }

        public void Drive(int Length, int passengers)
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync(Length);
            state = BusState.Driving;
            mainW.UpdateColor(this);
            if (BusDat != null)
                BusDat.updateColor();

            UpdateEarns(passengers * 20 - Length);
            totalPass += passengers;
            drives++;
        }
        private void UpdateEarns(int value)
        {
            totalEarnings += value;
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int Length = (int)e.Argument;
            counter = Length * 2;
            for (int i = 1; i <= Length * 2; i++)
            {
                System.Threading.Thread.Sleep(500);
                worker.ReportProgress(i * 100 / (Length*2));
            }
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            mainW.UpdatePB(_licNum, progress);
            mainW.UpdateTB(_licNum, counter / 2);
            if (busDat != null)
                busDat.UpdateTime(counter / 2);
            counter--;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            state = BusState.Ready;
            mainW.UpdatePB(_licNum, 0);
            mainW.UpdateTime(_licNum);
            mainW.UpdateColor(this);
            if (busDat != null)
            {
                busDat.UpdateInfo();
                BusDat.updateColor();
            }
        }
        public void AddKm(int distance)
        {
            _kmFromFuel += distance;
            _kmFromtreat += distance;
        }
    }
}

