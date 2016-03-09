using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Francescas.WeeklyScheduler.Models
{
    public class FrancescasContext : DbContext
    {
        public FrancescasContext()
            : base("FrancescasContext")
        {

        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<WeeklyScheduleCsv> WeeklyScheduleCsv { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); 
        }
    }
}
