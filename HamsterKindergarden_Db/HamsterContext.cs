using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HamsterKindergarden_Simulation;

namespace HamsterKindergarden_Db
{
    class HamsterContext : DbContext
    {
        public DbSet<Hamster> hamster { get; set; }
        public DbSet<HamsterCage> hamstercage { get; set; }
        public DbSet<ActivitieCage> activitieCages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server==LAPTOP-K2JKI9TE\\SQLEXPRESS;Database=HamsterDb;Trusted_Connection=True;").UseLazyLoadingProxies();
        }
    }
}
