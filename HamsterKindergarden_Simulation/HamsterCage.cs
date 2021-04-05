using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HamsterKindergarden_Simulation
{
    public class HamsterCage
    {
        public int Id { get; set; }
        public virtual List<Hamster> Hamsters { get; set; }
        public int MaxNumberOfHamsters { get; } = 3;

        public HamsterCage()
        {

        }

        public HamsterCage Fillcages(IGrouping<Gender, Hamster> h)
        {
            List<Hamster> hamster = new List<Hamster>();



            foreach (var i in h)
            {
                if (i.LatestActivities != "Checked In")
                {
                    if (hamster.Count < MaxNumberOfHamsters || hamster.Count == null)
                    {


                        hamster.Add(i);
                        i.LatestActivities = "Checked In";


                        //h.RemoveAt(i);

                    }
                    else
                    {
                        break;
                    }

                }
            }
            HamsterCage hc = new HamsterCage() { Hamsters = hamster };

            return hc;


            }



        }
}

