namespace MetopeMVCApp.Migrations
{
    using ASP.MetopeNspace.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASP.MetopeNspace.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ASP.MetopeNspace.Models.ApplicationDbContext";
        }

        protected override void Seed(ASP.MetopeNspace.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            //Seed the Admin.  to seed another, update this code then PM> Add-Migration Init,   
            // then PM> update-database
            if (!context.Users.Any(u => u.UserName == "BluestreamAdmin1"))
            {
                var user = new ApplicationUser { UserName = "BluestreamAdmin1", Email = "admin@blue-stream.com" };
                IdentityResult result = userManager.Create(user, "xxxxx123"); 

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");
                context.SaveChanges();
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
