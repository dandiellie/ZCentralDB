using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCentralDB.Models;

namespace ZCentralDB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Vendor> Vendors { get; set; }
        public IDbSet<Organization> Organizations { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Detail> Details { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
