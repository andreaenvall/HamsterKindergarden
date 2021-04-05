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

        static int i = 0;
        static void Main(string[] args)
        {

            //Methods.FillWithHamsters();
            
            Methods.FillCages();
            //Thread.Sleep(10);
            //await Methods.MoveHamstersToActivity();
            //await Methods.RemoveHamsterFromActivity();
            //Task<Hamster> hamstertask = Methods.MoveHamstersToActivity();




            TimeTicker.Activities += (sender, e) => ActivityThreads(sender, e);
            TimeTicker.StartTimer(1000);

            //TimeTicker.Activities += (sender, e) => Methods.MoveHamstersToActivity(sender, e);
            //TimeTicker.Activities += (sender, e) => ActivityThreads3(sender, e);




        }
        public static void ActivityThreads(object sender, EventArgs args) //används tillsammans med AktivityEventHandler
        {
            Methods.callmethods();
            //Methods.MoveHamstersToActivity(sender, e);

            //Thread t1 = new Thread(new ThreadStart(Methods.callmethods));

            //Thread t2 = new Thread(new ThreadStart(Methods.RemoveHamsterFromActivity));

            //var task = Task.Run(async () => await Methods.MoveHamstersToActivity());
            //var task2 = Task.Run(async () => await Methods.RemoveHamsterFromActivity());


            //t1.Start();
            
            ////t1.Join();

            //t2.Start();




            //await Task.Run(() => Methods.MoveHamstersToActivity());

        }
        public static void ActivityThreads2(object sender, EventArgs args)
        {
            // Start2();


        }
        public static void ActivityThreads3(object sender, EventArgs args)
        {
            i++;
            Methods.callmethods2();
            Console.WriteLine(i);
        }




      






    
}
}
