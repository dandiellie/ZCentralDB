namespace ZCentralDB.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //create an Organization
            Organization organization = new Organization
            {
                Name = "Z|Companies"
            };

            context.Organizations.AddOrUpdate(l => l.Name, organization);
            context.SaveChanges();

            //Create User=Admin with password=123456
            var admin = userManager.FindByName("admin@gmail.com");

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Organization = organization
                };

                userManager.Create(admin, "123456");
                admin = userManager.FindByName("admin@gmail.com");
            }

            context.SaveChanges();
        }
    }
}
