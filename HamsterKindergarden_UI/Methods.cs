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
        public static List<Hamster> ActivityQueue = new List<Hamster>();
        public static List<Hamster> HamstersInActivity = new List<Hamster>();
        public static int Countminutes { get; set; }

        public static void FillWithHamsters()
        {
            HamsterContext hk = new HamsterContext();
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
                    hamster1.ActivitieCageid = null;
                    hamster1.HamsterCageId = null;
                    hamster1.LatestActivities = "";
                    hamster1.Countminutes = 0;
                    hc.Add(hamster1);
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

                            if (h.LatestActivities != "Checked In")
                            {
                                if (ha.Count < cage.MaxNumberOfHamsters)
                                {
                                    ha.Add(h);
                                    h.HamsterCageId = cageid;
                                    h.LatestActivities = "Checked In";

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
            hk.activitieCages.Add(new ActivitieCage { activities = ActivityQueue });
        }
        
        public static void MoveHamstersToActivity()
        {
            lock (hk)
            {

                var HamstersAlreadyinActivity = hk.hamster.OrderBy(x => x.ActivitieCageid);

                foreach(Hamster h in HamstersAlreadyinActivity)
                {
                    if(h.ActivitieCageid != null)
                    {
                        HamstersInActivity.Add(h);
                    }
                }

                int? counter = ac.maxHamstersInActivities - HamstersInActivity.Count();
                int acId = 0;
                var activitycageid = hk.activitieCages.Select(x => x.id);
                foreach(var a in activitycageid.Take(1))
                {
                    acId = a;
                }

                if (ActivityQueue.Count == 0)
                {
                    foreach (var h in hk.hamster.OrderBy(x => x.Gender).ToList())
                    {

                        ActivityQueue.Add(h);

                    }
                }

                    if (HamstersInActivity.Count < ac.maxHamstersInActivities)
                    {
                    bool gender = true;
                    for (int i = 0; i < counter; i++)
                    {
                        
                        for (int j = 0; j < HamstersInActivity.Count; j++)
                        {
                            if (ActivityQueue[i].Gender != HamstersInActivity[j].Gender)
                            {
                                gender = false;
                            }
                        }
                        if (gender == true)
                        {
                            Hamster hamster = ActivityQueue.ElementAt(i);
                            if (hamster == null) { break; }
                            HamstersInActivity.Add(hamster);
                            hamster.ActivitieCageid = acId;
                            hamster.HamsterCageId = null;
                            hamster.AktivitesCounter++;
                            hamster.LatestActivities = "In Training ";
                            ActivityQueue.RemoveAt(i);
                            ActivityQueue.Add(hamster);

                        }


                    }

                       
                    }



                    
                    Console.WriteLine("add");

                    hk.SaveChanges();
               

            }
        }

        public static void RemoveHamsterFromActivity()
        {
            // HamsterContext hk2 = new HamsterContext();
            lock (hk)
            {
                for (int i = 0; i < ac.maxHamstersInActivities; i++)
                {
                    var freespot = FindFreeCageSpot();
                    var query = hk.hamster.Where(x => x.Countminutes > 12);

                    foreach(Hamster h in query)
                    {
                        HamstersInActivity.Remove(h);
                        h.HamsterCageId = freespot;
                        h.ActivitieCageid = null;
                        h.LatestActivities = "Chillin in cage after training";
                        h.Countminutes = 0;

                    }

                   

                }
                hk.SaveChanges();
                Console.WriteLine("remove");


            }
        }

        public static int? FindFreeCageSpot()
        {
            lock (hk)
            {
               

                    foreach (var i in hk.hamstercage.ToList())
                    {
                        if (i.Hamsters.Count() < i.MaxNumberOfHamsters )
                        {
                            return i.Id;
                        }
                    }
                    return null;
                
            }


        }

        public static void CountTime()
        {
            lock (hk)
            {
                

                foreach (var h in HamstersInActivity)
                {
                    if (h.Countminutes == null)
                    {
                        h.Countminutes = 0;
                    }

                    h.Countminutes += 6;


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
            t1.Start();
            t2.Start();
            t3.Start();
        }
        public static void callmethods2()
        {
            CountTime();
            Thread.Sleep(300);
        }

    }

}

