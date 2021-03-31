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
        public virtual List<DateTime> Activities { get; set; }
        public int AktivitesCounter { get; set; }


        public void FillWithHamsters()
        {
            using (var reader = new StreamReader("Hamsterlista30.csv"))
            {
                List<Hamster> exhamstrar = new List<Hamster>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    exhamstrar.Add(new Hamster { Name = values[0], Age = int.Parse(values[1]), Gender = (Gender)Enum.Parse(typeof(Gender), values[2]), Owner = values[3] });
                    
                }
            }
        }
    }
}
