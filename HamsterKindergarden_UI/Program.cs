using System;
using System.Collections.Generic;
using System.IO;
using HamsterKindergarden_Simulation;
using HamsterKindergarden_Db;
using System.Threading;
using System.Threading.Tasks;

namespace HamsterKindergarden_UI
{
    public class Program
    {

        
        static void Main(string[] args)
        {

            Methods.IsHamsterEmty();
            Methods.FillCages();

            Console.WriteLine("How many miliseconds do you want one tick to be?");
            int seconds = int.Parse(Console.ReadLine());
            Console.WriteLine("How many days do you want the simulation to last?");
            int days = int.Parse(Console.ReadLine());
            Methods.days = days-1;

            

            
            TimeTicker.Activities += (sender, e) => ActivityThreads(sender, e);
            TimeTicker.Activities += (sender, e) => ActivityThreads2(sender, e);
            TimeTicker.StartTimer(seconds);
            

        }
        public static void ActivityThreads(object sender, EventArgs args) //används tillsammans med AktivityEventHandler
        {
            Methods.callmethods();
           
        }
        public static void ActivityThreads2(object sender, EventArgs args)
        {
            DateTime dt = Methods.Start.AddDays(Methods.days).AddHours(9).AddMinutes(49);

            if (Methods.Clock >= dt)
            {
                Thread t3 = new Thread(new ThreadStart(Methods.EndRaport));
                t3.Start();

                TimeTicker.EndTimer();
            }


        }
       




      






    
}
}
