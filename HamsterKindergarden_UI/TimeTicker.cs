using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HamsterKindergarden_Simulation
{
    public static class TimeTicker
    {

        public delegate void ActivitiesEventHandler(object source, EventArgs args);
        public static event ActivitiesEventHandler Activities;

        //Sample 02: Declare the Timer Reference
        static Timer timer = null;


        //Sample 03: Timer Callback - 

        //  Just Ticks in the Console
        static void TickTimer(object state)
        {
            Activities?.Invoke(state, EventArgs.Empty); //6 minutes past

        }
        public static void StartTimer(int seconds)
        {
            bool a = true;
            bool b = true;
            while (a)
            {
                //Sample 04: Create and Start The Timer
                if (b)
                {
                    timer = new Timer(
                new TimerCallback(TickTimer),
                null,
                seconds,
                seconds);
                    b = false;
                }
            }
        }

        public static void EndTimer()
        {
            timer.Dispose();

        }
    }
}

