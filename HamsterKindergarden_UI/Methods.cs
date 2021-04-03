using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HamsterKindergarden_Db;
using HamsterKindergarden_Simulation;

namespace HamsterKindergarden_UI
{
    public class Methods
    {
        public static HamsterContext hk = new HamsterContext();
        public static ActivitieCage ac = new ActivitieCage();
        public static Queue<Hamster> activites = new Queue<Hamster>();
        public static List<Hamster> ActivityHamster = new List<Hamster>();
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

        public static void FillCages()
        {
            List<Hamster> hc = new List<Hamster>();
            List<Hamster> femalelist = new List<Hamster>();
            HamsterCage cage = new HamsterCage();


            var query = hk.hamster.OrderBy(x => x.Gender);
            foreach (var i in query)
            {
                hc.Add(i);
            }

            var query1 = hc.GroupBy(gender => gender.Gender);

            int hamstercount = 0;
            foreach (var group in query1)
            {
                foreach(var hamster in group)
                {
                    hamstercount++;
                }

                for(int i= 0; i < hamstercount/3; i++)
                {
                    hk.hamstercage.Add(cage.Fillcages(group));
                    
                }

            }

            hk.SaveChanges();
        }

        public static void MoveHamstersToActivity()
        {
           var query = hk.hamster.OrderBy(x => x.Gender);

            if (activites.Count() == 0)
            {
                foreach (var h in query)
                {
                    activites.Enqueue(h);
                }
            }
            int counter = ac.maxHamstersInActivities - ActivityHamster.Count;

            if ( ActivityHamster.Count() < ac.maxHamstersInActivities)
            {
                for (int i = 0; i < counter; i++)
                {
                    Hamster hamster = activites.Dequeue();
                    ActivityHamster.Add(hamster);
                }
                //ac.activities = ActivityHamster;
                hk.activitieCages.Add(new ActivitieCage { activities = ActivityHamster });
            }
           
            
            
            foreach (var h in ActivityHamster)
            {
                
                h.HamsterCageId = null;
                h.AktivitesCounter++;
                
            }

            hk.SaveChanges();

        }

        public static void RemoveHamsterFromActivity()
        {
            for(int i = 0; i < ac.activities.Count(); i++)
            {
                if(ac.activities[i].Countminutes > 60)
                {
                    ac.activities[i].HamsterCageId = FindFreeCageSpot();

                    ac.activities.Remove(ac.activities[i]);
                    
                }


            }

        }

        public static int? FindFreeCageSpot()
        {
            foreach(var i in hk.hamstercage)
            {
                if(i.Hamsters.Count() < i.MaxNumberOfHamsters)
                { 
                    return i.Id;
                }
            }
            return null;
        }


    }

}

