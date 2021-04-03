using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HamsterKindergarden_Simulation
{
    public class ActivitieCage
    {
        public int id { get; set; }
        public virtual List<Hamster> activities { get; set; }
        public int maxHamstersInActivities { get; } = 6;
        public int Tickcounter { get; set; }

        //public List<Hamster> MoveHamsterToActivity(Queue<Hamster> h)
        //{
        //   List<Hamster> ActivityHamster = new List<Hamster>();
        //   if(maxHamstersInActivities < activities.Count)
        //    {
        //        for(int i =0; i < maxHamstersInActivities-activities.Count; i++)
        //        {
        //            Hamster hamster = h.Dequeue();
        //            ActivityHamster.Add(hamster);
        //        }
        //    }
        //    activities = ActivityHamster;

        //    return activities;
        //}
    }
}
