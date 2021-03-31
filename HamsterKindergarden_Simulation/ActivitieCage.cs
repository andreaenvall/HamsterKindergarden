using System;
using System.Collections.Generic;
using System.Text;

namespace HamsterKindergarden_Simulation
{
    public class ActivitieCage
    {
        public virtual List<Hamster> activityqueue { get; set; }
        public virtual List<Hamster> activities { get; set; }
        public int maxHamstersInActivities { get; } = 6;
        
        public int Tickcounter { get; set; }

        
    }
}
