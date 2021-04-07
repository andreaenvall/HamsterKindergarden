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
        public static HamsterContext hk = new HamsterContext();
        public static ActivitieCage ac = new ActivitieCage();
        public static HamsterCage hamstercage = new HamsterCage();
        public static Queue<Hamster> ActivityQueue = new Queue<Hamster>();
        public static List<Hamster> HamstersInActivity = new List<Hamster>();
        public static Activities_Log Activities_Log = new Activities_Log();
        //public static Log log = new Log();
        public static DateTime Clock { get => WhatTimeIsIt(); }
        public static DateTime Start { get => new DateTime(2015, 12, 31, 07, 00, 00); }
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
                foreach(var hb in hk.hamstercage)
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
                                    //foreach(var count in hk.hamstercage.Where(x=> x.Id == cageid).Take(1)) // INVALID COLUMN NAME??
                                    //{
                                    //    count.HamsterCount = count.Hamsters.Count();
                                    //}


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
                var time = WhatTimeIsIt();


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
                var time = WhatTimeIsIt();


                var countminutes = hk.hamster.Where(x => x.Countminutes > 60);

                foreach (var h in countminutes)
                {

                    HamstersInActivity.Remove(h);
                    
                    h.HamsterCageId = FindFreeCageSpot(h);

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
               
                int id = 0;
                Gender g = Gender.M;

                foreach(var c in hk.hamstercage.Where(x=> x.HamsterCount < 3))
                {
                    if(c.HamsterCount > 2)
                    {
                        continue;
                    }
                    if(c.HamsterCount == 0)
                    {
                        if(hamster.Gender == Gender.M)
                        {
                            c.IsMale = true;
                        }
                       
                        return c.Id;
                    }
                    

                    foreach(var h in hk.hamster.Where(x=> x.HamsterCageId == c.Id))
                    {
                        if (h == null)
                        {
                            if (hamster.Gender == Gender.M && c.IsMale == true)
                            {
                                return c.Id;
                            }
                            else if(hamster.Gender == Gender.K && c.IsMale == false)
                            {
                                return c.Id;
                            }
                        }
                        if (h.Gender == hamster.Gender)
                        {
                            id = c.Id;
                            return c.Id;
                            
                        }
                        else { break; }
                    }
                    continue;
                    
                }

                return id;

                //for (int i = 1; i <= hk.hamstercage.Count(); i++)
                //{
                //    foreach (var x in hk.hamster)
                //    {
                //        if (x.HamsterCageId == i)
                //        {
                //            count++;
                //            //g = x.Gender;

                //        }
                //    }
                //    if (count < 3) //hamster.Gender == g
                //    {
                //        return i;
                //    }
                //    else { continue; }
                //}

                //return null;

                //var query = hk.hamster.Where(x => x.HamsterCageId == i);

                //foreach(var h in query)
                //{
                //     count++;

                //    if(h.Gender == Gender.M)
                //    {
                //        g = Gender.M;

                //    }

                //}
                //if(count < 3 &&  hamster.Gender == g)
                //{
                //    return i;
                //}

                //var query = hk.hamstercage.OrderBy(x => x.Hamsters).Where(x => x.Hamsters.Count() < 3);



                //foreach (var i in query)
                //{
                //    if (i.Hamsters.Count < i.MaxNumberOfHamsters)
                //    {
                //        foreach (Hamster h in i.Hamsters.Take(1))
                //        {
                //            if (h.Gender == hamster.Gender)
                //            {
                //                return i.Id;

                //                //i.Hamsters.Add(h);
                //                //hk.SaveChanges();
                //            }
                //            else
                //                break;

                //        }

                //    }
                //    else { continue; }
                //}


                hk.SaveChanges();
            }

    
        }

        public static void CountTime()
        {
            lock (hk)
            {

                Countminutes += 6;
                
                foreach (var h in HamstersInActivity)
                {
                    if (h.Countminutes == null)
                    {
                        h.Countminutes = 0;
                    }

                    h.Countminutes = Countminutes;


                }
                
            }
        }
    

        public static void callmethods()
        {
            Thread t1 = new Thread(new ThreadStart(MoveHamstersToActivity));
            Thread.Sleep(10);
            Thread t2 = new Thread(new ThreadStart(RemoveHamsterFromActivity));
            Thread.Sleep(100);
            Thread t3 = new Thread(new ThreadStart(CountTime));
            Thread.Sleep(150);
            Thread t4 = new Thread(new ThreadStart(FrontEnd));
            Thread.Sleep(200);
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
        }
        public static void callmethods2()
        {
            CountTime();
            Thread.Sleep(300);
        }

        public static DateTime WhatTimeIsIt()
        {
            
                TimeSpan time = new TimeSpan(00, 00, Countminutes, 00);
                DateTime combined = Start.Add(time);

                return combined;
             
        }

        public static void IsHamsterEmty()
        {
            if(hk.hamster.Count() == 0) { FillWithHamsters(); }
            if(hk.hamstercage.Count() == 0) { CreateCages(); }
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
                    Console.Write(" --------------- \n" );

                }

                foreach (var h in hk.activitieCages)
                {
                    Console.SetCursorPosition(50, 0);
                    Console.WriteLine("  Workout Area");
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

            }
        }

    }

}

