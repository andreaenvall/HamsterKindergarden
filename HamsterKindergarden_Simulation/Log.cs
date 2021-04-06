using System;
using System.Collections.Generic;
using System.Text;

namespace HamsterKindergarden_Simulation
{
    public enum Activities_Log
    {
        CheckIn, CheckOut, TrainingStart, TrainingEnd
    }
    public class Log
    {
        public int Id { get; set; }
        public Activities_Log HamsterActivity { get; set; }
        public DateTime ActivityTime { get; set; }
        public TimeSpan ActivityDuration { get; set; }
    }
}
