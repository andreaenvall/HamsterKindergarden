using System;
using System.Collections.Generic;
using System.IO;

namespace HamsterKindergarden_Simulation
{
    public enum Gender
    {
        M, K
    }
    public class Hamster
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public virtual Gender Gender { get; set; }
        public string Owner { get; set; }
        public virtual DateTime CheckedIn { get; set; } 
        public string LatestActivities { get; set; }
        public int AktivitesCounter { get; set; }
        public Nullable<int> HamsterCageId { get; set; }

        public int? ActivitieCageid { get; set; }
        public int? Countminutes { get; set; }

        public virtual List<Log> ActivityList { get; set; }

       
    }



    /// <summary>
    /// Sets the starttime to a specific date and time. Returns a datetime.
    /// </summary>
    /// <returns></returns>
   
        
    
}
