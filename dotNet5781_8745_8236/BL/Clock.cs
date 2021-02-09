using System;

namespace BL
{
    internal class Clock
    {
        #region singelton
        static readonly Clock instance = new Clock();
        static Clock() { }// static ctor to ensure instance init is done just before first usage
        Clock() { } // default => private
        public static Clock Instance { get => instance; }// The public Instance property to use
        #endregion

        /// <summary>
        /// The simulator rate, private
        /// </summary>
        private int rate;
        /// <summary>
        /// The simulator rate
        /// </summary>
        public int Rate { get { return rate; } set { rate = value; } }
        /// <summary>
        /// The observer's action, private
        /// </summary>
        private event Action<TimeSpan> clockObserver;
        /// <summary>
        /// The observer's action
        /// </summary>
        public event Action<TimeSpan> ClockObserver {
            add
            {
                clockObserver = value;
            }
            remove
            {
                clockObserver -= value;
            } }
        /// <summary>
        /// The current simulator's time, private
        /// </summary>
        private TimeSpan time;
        /// <summary>
        /// The current simulator's time
        /// </summary>
        public TimeSpan Time 
        { 
            get 
            {
                return time; 
            }
            set
            {
                time = value;
                clockObserver?.Invoke(time);
            } 
        }
    }
}
