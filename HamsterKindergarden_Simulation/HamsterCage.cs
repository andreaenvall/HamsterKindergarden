﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HamsterKindergarden_Simulation
{
    public class HamsterCage
    {
        public int CageId { get; set; }
        public virtual List<Hamster> Hamsters { get; set; }
        public int MaxNumberOfHamsters { get; } = 3;



    }
}
