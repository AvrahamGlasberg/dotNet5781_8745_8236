using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace dotNet5781_03B_8745_8236
{
    /// <summary>
    /// save the status of the bus 
    /// </summary>
    public enum BusState
    {
        Ready, Driving, Refueling, Treatment
    }
    /// <summary>
    /// bus class to represent the data and functions of bus 
    /// </summary>
    public class Bus
    {
        /// <summary>
        /// this feiled is for updating changes in the main win
        /// </summary>
        private static MainWindow mainW;
        /// <summary>
        /// object of type win for present the data of the bus
        /// </summary>
        private BusData busDat;
        public BusData BusDat { get { return busDat; } set { busDat = value; } }// property
        /// <summary>
        /// represent the status of the bus
        /// </summary>
        private BusState state; 
        public BusState State { get { return state; } set { state = value; }  } // property

        /// <summary>
        /// total km the bus drove
        /// </summary>
        private int _kmTotal;
        public int KmTotal { get { return _kmTotal; } set { _kmTotal = value; } }//property
        /// <summary>
        /// km the bus drove since last refuel
        /// </summary>
        private int _kmFromFuel;
        public int KmFromFuel { get { return _kmFromFuel; } set { _kmFromFuel = value; } }//property
        /// <summary>
        /// km the bus drove since last treatment
        /// </summary>
        private int _kmFromtreat;
        public int KmFromtreat { get { return _kmFromtreat; } set { _kmFromtreat = value; } }//property
        /// <summary>
        /// licesne number
        /// </summary>
        private int _licNum;
        public int LicNum { get { return _licNum; } }//property
        /// <summary>
        /// date after last treatment/started working
        /// </summary>
        private DateTime _start; 
        public DateTime Start { get { return _start; } set { _start = value; } }//property
        /// <summary>
        /// //date from last treatment
        /// </summary>
        private DateTime _lastTreat; 
        public DateTime LastTreat { get { return _lastTreat; } set { _lastTreat = value; } }//property
        /// <summary>
        /// we choice this method of threads because its the most efficint one in this program. 
        /// in this mathod every bus have a diffrent BackgroundWorker for its current thread.
        /// Dan z. use this mathod too in program with same needed and he, as well, use a diffrent BackgroundWorker for every object of is class.
        /// </summary>
        private BackgroundWorker worker;
        /// <summary>
        /// counter the time that the bus is ready for new drive
        /// </summary>
        private int counter;
        public int Counter { get {return counter;} } // property
        /// <summary>
        /// counter for amount of driving 
        /// </summary>
        private int drives = 0;
        public int Drives { get {return drives;} }// property
        /// <summary>
        /// counting amount of passengers 
        /// </summary>
        private int totalPass = 0;
        public int TotalPass { get {return totalPass;} }// property
        /// <summary>
        /// counting the amount of earning 
        /// </summary>
        private int totalEarnings = 0;
        public int TotalEarnings { get {return totalEarnings;} } //property

        /// <summary>
        /// constructor for Bus cless
        /// </summary>
        /// <param name="licNum">the Bus's licesnse number</param>
        /// <param name="date">the date that the bus started working </param>
        /// <param name="kmFromFuel">the km that droven from last fuel</param>
        /// <param name="kmFromtreat">the km that droven from last treatment</param>
        /// <param name="kmT">the total km </param>
        /// <param name="lastTreat">the date of the last treatment</param>
        /// <param name="startDate">the date of the start acivity of the bus</param>
        public Bus(int licNum, DateTime startDate, int kmT = 0, int kmFromFuel = 0, int kmFromtreat = 0, DateTime lastTreat = default(DateTime))
        {
            _kmTotal = kmT;
            _kmFromFuel = kmFromFuel;
            _kmFromtreat = kmFromtreat;
            _licNum = licNum;
            _start = startDate;
            if (lastTreat == default(DateTime))
                _lastTreat = DateTime.Now; // the default is the date of the inserting the bus
            else
                _lastTreat = lastTreat;
            state = BusState.Ready;
            counter = 0;
            busDat = null; // for check if the bus data win is open or not
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
            _kmFromFuel = 0;//that how we undrstood the assignment - when make treatment make refuel too.
            _lastTreat = DateTime.Now;
            mainW.UpdateColor(this); 
            if (BusDat != null)
                BusDat.updateColor();
            UpdateEarns(-2000);
        }
        /// <summary>
        /// this function convert the license number of the bus to string
        /// </summary>
        /// <returns>the license number as a string</returns>
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
        /// <summary>
        /// this func updating the main win
        /// </summary>
        /// <param name="window">Main Window of the appliction</param>
        public static void UpdateWin(MainWindow window)
        {
            mainW = window;
        }
        /// <summary>
        /// this function make a thread for make the operetion of driving of bus
        /// </summary>
        /// <param name="Length">the length of the drive</param>
        /// <param name="passengers">the number of passengers in the current drive </param>
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
        /// <summary>
        /// this function updating the total earns of the bus
        /// </summary>
        /// <param name="value">the amount of earn to add for the total</param>
        private void UpdateEarns(int value)
        {
            totalEarnings += value;
        }
        /// <summary>
        /// this function make the progress of the operation that need to happend when we start threads
        /// </summary>
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
        /// <summary>
        /// this function make the changes while th thread runing
        /// its update the prograss bar and update the time of ready.
        /// </summary> 
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            mainW.UpdatePB(_licNum, progress);
            mainW.UpdateTB(_licNum, counter / 2);
            if (busDat != null)
                busDat.UpdateTime(counter / 2);
            counter--;
        }
        /// <summary>
        /// this function happend when thread done is runing 
        /// the function clean the progress bar, update the bus to be ready and all the implicit as a result of that.
        /// </summary>
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
        /// <summary>
        /// this function update the km of the bus according to the drive distance
        /// </summary>
        /// <param name="distance">the drive distance</param>
        public void AddKm(int distance)
        {
            _kmFromFuel += distance;
            _kmFromtreat += distance;
            _kmTotal += distance;
        }
    }
}

