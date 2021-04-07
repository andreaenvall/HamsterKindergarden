using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HamsterKindergarden_Db;
using HamsterKindergarden_Simulation;

namespace HamsterKindergarden_UI
{
    public static class Methods
    {
        public static int days;
        public static HamsterContext hk = new HamsterContext();
        public static ActivitieCage ac = new ActivitieCage();
        public static HamsterCage hamstercage = new HamsterCage();
        public static Queue<Hamster> ActivityQueue = new Queue<Hamster>();
        public static List<Hamster> HamstersInActivity = new List<Hamster>();
        public static Activities_Log Activities_Log = new Activities_Log();
        //public static Log log = new Log();
        public static DateTime Clock { get => WhatTimeIsIt(days); }
        public static DateTime Start { get => new DateTime(2015, 12, 31, 15, 00, 00); }
        public static int Countminutes { get; set; }


        public static void FillWithHamsters()
        {

            using (var reader = new StreamReader("Hamsterlista30.csv"))
            {
                //List<Hamster> exhamstrar = new List<Hamster>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    hk.hamster.Add(new Hamster { Name = values[0], Age = int.Parse(values[1]), Gender = (Gender)Enum.Parse(typeof(Gender), values[2]), Owner = values[3], CheckedIn = new DateTime(2015, 12, 31, 07, 00, 00) });

                }
                hk.SaveChanges();
            }

        }

        public static void CreateCages()
        {
            lock (hk)
            {
                for (int i = 0; i < hk.hamster.Count() / 3; i++)
                {
                    hk.hamstercage.Add(new HamsterCage());
                }
                hk.SaveChanges();
            }
        }
        public static void FillCages()
        {
            lock (hk)
            {

                List<Hamster> hc = new List<Hamster>();
                List<Hamster> ha = new List<Hamster>();
                HamsterCage cage = new HamsterCage();
                List<int> hamsterId = new List<int>();

                var query3 = hk.hamstercage.OrderBy(x => x.Id).Select(x => x.Id);

                var query = hk.hamster.OrderBy(x => x.Gender);

                foreach (var hamster1 in query)
                {
                    hamster1.AktivitesCounter = 0;
                    hamster1.ActivitieCageid = null;
                    hamster1.HamsterCageId = null;
                    hamster1.LatestActivities = "";
                    hamster1.ActivityList.Clear();
                    hamster1.Countminutes = 0;
                    hc.Add(hamster1);
                }
                foreach (var hb in hk.hamstercage)
                {
                    hb.HamsterCount = 3;
                }

                foreach (var id in query3)
                {
                    int? hamsterID = id;
                    hamsterId.Add(id);

                }

                for (int i = 0; i < hamsterId.Count(); i++)
                {
                    ha.Clear();
                    int? cageid = hamsterId[i];
                    var query1 = hc.GroupBy(gender => gender.Gender);



                    foreach (var group in query1)
                    {
                        foreach (var h in group)
                        {

                            if (h.LatestActivities == "")
                            {
                                if (ha.Count < cage.MaxNumberOfHamsters)
                                {
                                    ha.Add(h);
                                    h.HamsterCageId = cageid;
                                    h.LatestActivities = "Checked In";
                                    h.CheckedIn = new DateTime(2015, 12, 12, 07, 00, 00);




                                    h.ActivityList.Add(new Log { HamsterActivity = Activities_Log.CheckIn, ActivityTime = Clock });

                                }
                                else
                                {
                                    break;
                                }



                            }

                        }

                    }


                }

            }

            hk.SaveChanges();
        }

        public static void CreateActivitieCage()
        {
            lock (hk)
            {
                hk.activitieCages.Add(new ActivitieCage());
                hk.SaveChanges();
            }
        }

        public static void MoveHamstersToActivity()
        {
            lock (hk)
            {
                var time = WhatTimeIsIt(days);


                int? counter = ac.maxHamstersInActivities - HamstersInActivity.Count();
                int acId = 0;

                var activitycageid = hk.activitieCages.Select(x => x.id);

                foreach (var a in activitycageid.Take(1))
                {
                    acId = a;
                }

                if (ActivityQueue.Count == 0)
                {
                    foreach (var h in hk.hamster.OrderBy(x => x.Gender).ToList())
                    {

                        ActivityQueue.Enqueue(h);

                    }
                }

                if (HamstersInActivity.Count < ac.maxHamstersInActivities)
                {
                    bool gender = true;

                    for (int i = 0; i < counter; i++)
                    {

                        for (int j = 0; j < HamstersInActivity.Count; j++)
                        {
                            if (ActivityQueue.Peek().Gender != HamstersInActivity[j].Gender)
                            {
                                gender = false;
                            }
                        }
                        if (gender == true || HamstersInActivity.Count == 0)
                        {
                            Hamster hamster = ActivityQueue.Dequeue();

                            if (hamster == null) { break; }

                            HamstersInActivity.Add(hamster);
                            foreach (var count in hk.hamstercage.Where(x => x.Id == hamster.HamsterCageId).Take(1))
                            {
                                count.HamsterCount--;
                            }
                            hamster.ActivitieCageid = acId;
                            hamster.HamsterCageId = null;
                            hamster.AktivitesCounter++;

                            hamster.LatestActivities = "In Training ";
                            hamster.ActivityList.Add(new Log { HamsterActivity = Activities_Log.TrainingStart, ActivityTime = time });
                            //ActivityQueue.RemoveAt(i);
                            ActivityQueue.Enqueue(hamster);

                        }
                        else { break; }


                    }


                }






                hk.SaveChanges();


            }
        }

        public static void RemoveHamsterFromActivity()
        {
            lock (hk)
            {
                var time = WhatTimeIsIt(days);


                var countminutes = hk.hamster.Where(x => x.Countminutes > 60);


                foreach (var h in countminutes)
                {
                    if (h.Countminutes < 60)
                    {
                        continue;
                    }

                    HamstersInActivity.Remove(h);

                    h.HamsterCageId = FindFreeCageSpot(h);
                    //FindFreeCageSpot(h);

                    foreach (var count in hk.hamstercage.Where(x => x.Id == h.HamsterCageId).Take(1))
                    {
                        count.HamsterCount++;
                    }
                    h.ActivitieCageid = null;
                    h.LatestActivities = "Chillin in cage after training";
                    h.Countminutes = 0;
                    h.ActivityList.Add(new Log { HamsterActivity = Activities_Log.TrainingEnd, ActivityTime = time });


                }




                hk.SaveChanges();



            }
        }

        public static int? FindFreeCageSpot(Hamster hamster)
        {
            lock (hk)
            {
                int count = 0;
                int countfalse = 0;
                int? id = 0;
                for (int i = 1; i <= 10; i++)
                {
                    count = 0;
                    countfalse = 0;
                    foreach (Hamster h in ActivityQueue)
                    {
                        if (h.Gender != hamster.Gender)
                        {
                            if (h.HamsterCageId == i)
                            {
                                countfalse++;
                            }

                        }

                        else if (h.Gender == hamster.Gender)
                        {
                            if (h.HamsterCageId == i)
                            {
                                count++;
                                id = i;
                            }

                        }

                    }
                    if (count >= 3)
                    {
                        continue;
                    }
                    if (countfalse > 0)
                    {
                        continue;
                    }

                    return i;

                }

                return null;

            }


        }

        public static void CountTime()
        {
            lock (hk)
            {

                Countminutes += 6;

                foreach (var h in HamstersInActivity)
                {


                    h.Countminutes += 6;


                }

            }
        }


        public static void callmethods()
        {
            Thread t1 = new Thread(new ThreadStart(MoveHamstersToActivity));
            Thread.Sleep(10);
            Thread t2 = new Thread(new ThreadStart(RemoveHamsterFromActivity));
            Thread.Sleep(20);
            Thread t4 = new Thread(new ThreadStart(CountTime));
            Thread.Sleep(30);
            Thread t5 = new Thread(new ThreadStart(FrontEnd));
            Thread.Sleep(40);
            //Thread t3 = new Thread(new ThreadStart(EndRaport));
           // Thread.Sleep(150);
            t1.Start();
            t2.Start();
            //t3.Start();
            t4.Start();
            t5.Start();
        }
        


        public static DateTime WhatTimeIsIt(int days)
        {


            TimeSpan time = new TimeSpan(00, 00, Countminutes, 00);
            DateTime combined = Start.Add(time);




            if (combined.Hour == 17 && combined.Minute > 0)
            {
                //EndTime();
                
                Countminutes += 840;
                //TimeTicker.EndTimer();
                FillCagesNewDay();




            }


            return combined;

        }

        public static void IsHamsterEmty()
        {
            if (hk.hamster.Count() == 0) { FillWithHamsters(); }
            if (hk.hamstercage.Count() == 0) { CreateCages(); }
            if (hk.activitieCages.Count() == 0) { CreateActivitieCage(); }

        }

        public static void FrontEnd()
        {
            lock (hk)
            {
                int cursor = 1;
                Console.Clear();
                foreach (var h in hk.hamstercage)
                {
                    Console.WriteLine($"     Bur: {h.Id}  ");
                    Console.Write(" --------------- \n");
                    foreach (var x in h.Hamsters)
                    {

                        Console.Write("|   ");
                        Console.Write($"{x.Name}\t");
                        Console.Write("|   \n");
                    }
                    Console.Write(" --------------- \n");

                }

                foreach (var h in hk.activitieCages)
                {
                    Console.SetCursorPosition(50, 0);
                    Console.WriteLine("  Workout Area ");
                    Console.SetCursorPosition(50, 1);
                    Console.WriteLine(" ______________ \n");
                    foreach (var x in h.activities)
                    {
                        cursor++;
                        Console.SetCursorPosition(50, cursor);
                        Console.Write($"|  {x.Name}\t | ");

                    }
                    Console.SetCursorPosition(50, 8);
                    Console.WriteLine(" ______________ \n");

                }

                Console.SetCursorPosition(50, 12);
                Console.WriteLine($"{Clock}");

                Console.SetCursorPosition(50, 14);
                cursor = 14;
                //foreach (var h in hk.AktivityLog.Take(10))
                //{
                //    cursor++;
                //    Console.SetCursorPosition(50, cursor);

                //    if (h.HamsterActivity == Activities_Log.CheckIn)
                //    {
                //        Console.WriteLine($"{h.ActivityTime}");
                //    }

                //}

                var query = from h in hk.hamster
                            join ac in hk.AktivityLog on h.ID equals ac.HamsterID
                            select new { hamster = h.Name, aktivitet = ac.HamsterActivity, aktivitetstid = ac.ActivityTime };
                Console.SetCursorPosition(50, 14);
                Console.WriteLine("10 RECENTS UPDATES");
                cursor = 14;
                foreach (var h in query.OrderByDescending(x => x.aktivitetstid).Take(10))
                {
                    cursor++;
                    Console.SetCursorPosition(50, cursor);
                    Console.WriteLine($"{h.hamster}, {h.aktivitet}, {h.aktivitetstid}");
                }


            }
        }

        public static void FillCagesNewDay()
        {
            lock (hk)
            {

                List<Hamster> hc = new List<Hamster>();
                List<Hamster> ha = new List<Hamster>();
                HamsterCage cage = new HamsterCage();
                List<int> hamsterId = new List<int>();
                HamstersInActivity.Clear();

                var query3 = hk.hamstercage.OrderBy(x => x.Id).Select(x => x.Id);

                var query = hk.hamster.OrderBy(x => x.Gender);

                foreach (var hamster1 in query)
                {

                    hamster1.ActivitieCageid = null;
                    hamster1.HamsterCageId = null;
                    hamster1.LatestActivities = "";
                    hamster1.Countminutes = 0;
                    hc.Add(hamster1);
                }
                foreach (var hb in hk.hamstercage)
                {
                    hb.HamsterCount = 3;
                }

                foreach (var id in query3)
                {
                    int? hamsterID = id;
                    hamsterId.Add(id);

                }

                for (int i = 0; i < hamsterId.Count(); i++)
                {
                    ha.Clear();
                    int? cageid = hamsterId[i];
                    var query1 = hc.GroupBy(gender => gender.Gender);



                    foreach (var group in query1)
                    {
                        foreach (var h in group)
                        {

                            if (h.LatestActivities == "")
                            {
                                if (ha.Count < cage.MaxNumberOfHamsters)
                                {
                                    ha.Add(h);
                                    h.HamsterCageId = cageid;
                                    h.LatestActivities = "Checked In";
                                    h.CheckedIn = new DateTime(2015, 12, 12, 07, 00, 00);




                                    h.ActivityList.Add(new Log { HamsterActivity = Activities_Log.CheckIn, ActivityTime = Clock });

                                }
                                else
                                {
                                    break;
                                }



                            }

                        }

                    }


                }

            }

            hk.SaveChanges();
        }

        public static void EndTime()
        {
            DateTime dt = Start.AddDays(days).AddHours(2);
            if (dt == Clock)
            {
                TimeTicker.EndTimer();
            }
        }

        public static void EndRaport()
        {
            lock (hk)
            {
                //DateTime dt = Start.AddDays(days).AddHours(2);

                //if (Clock == dt)
                //{

                Console.Clear();
                foreach (var h in hk.hamster)
                {
                    h.ActivityList.Add(new Log { HamsterActivity = Activities_Log.CheckOut, ActivityTime = Clock });

                }
                hk.SaveChanges();

                int cursor = 0;
                var query = from h in hk.hamster
                            join ac in hk.AktivityLog on h.ID equals ac.HamsterID
                            select new { hamster = h.Name, antalakt = h.AktivitesCounter, aktivitet = ac.HamsterActivity, aktivitetstid = ac.ActivityTime };
                // Console.SetCursorPosition(40, 10);

                Console.WriteLine("\t HamsterKindergarden SimulationRaport");
                Console.WriteLine("\t ____________________________________");
                Console.WriteLine();
                cursor = 14;
                foreach (var x in hk.hamster)
                {
                    Console.WriteLine($"\t Hamster {x.Name}");
                    Console.WriteLine();
                    Console.Write($"\t Sum of workouts: {x.AktivitesCounter} \n");

                    foreach (var h in query.OrderByDescending(x => x.hamster).ToList())
                    {
                        cursor++;
                        //Console.SetCursorPosition(30, cursor);
                        if(h.hamster == x.Name)
                        {
                            Console.WriteLine($"\t {h.aktivitet}, {h.aktivitetstid}");
                        }

                       
                    }
                    Console.WriteLine();
                }
                Console.ReadLine();


                foreach (var hamster1 in hk.hamster)
                {

                    hamster1.ActivitieCageid = null;
                    hamster1.HamsterCageId = null;
                    hamster1.LatestActivities = "Checked Out";
                    hamster1.Countminutes = 0;
                    hamster1.ActivityList.Clear();

                }




                //TimeTicker.EndTimer();

            }
        }
    }

    }



