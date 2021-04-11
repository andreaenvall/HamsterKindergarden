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

       
        static Timer timer = null;


       
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

