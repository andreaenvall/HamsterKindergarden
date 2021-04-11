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
       
        

       
    }
}
