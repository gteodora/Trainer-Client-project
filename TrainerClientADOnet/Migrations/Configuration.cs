namespace TrainerClientADOnet.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using TrainerClientADOnet.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TrainerClientADOnet.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TrainerClientADOnet.Models.ApplicationDbContext";
        }

        protected override void  Seed(TrainerClientADOnet.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


                    if (!context.Roles.Any(r => r.Name == "Admin"))
                    {
                        var store = new RoleStore<IdentityRole>(context);
                        var manager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(store);
                        var role = new IdentityRole { Name = "Admin" };

                        manager.Create(role);
                    }
            


                    if (!context.Users.Any(u => u.UserName == "durke@gmail.com"))
                    {
                        var store = new UserStore<ApplicationUser>(context);
                        var manager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
                        var user = new ApplicationUser { UserName = "durke@gmail.com", Email= "durke@gmail.com"};

                        manager.Create(user, "Teodora!1");
                context.Users.Add(user);
                manager.AddToRole(user.Id, "Admin");
                    }


         //   if (!context.Roles.Any(u => u.Name == "Trener"))
         //   {
                        if (!context.Users.Any(u => u.UserName == "eeeee@gmail.com"))
                    {
                        var store = new UserStore<ApplicationUser>(context);
                        var manager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
                        var user = new ApplicationUser { UserName = "eeeee@gmail.com", Email = "eeeee@gmail.com" };

                        manager.Create(user, "Teodora!1");
                        context.Users.Add(user);
                        manager.AddToRole(user.Id, "Trener");
                    }
            //  }



               if (!context.Roles.Any(u => u.Name == "Trener"))
               {
                    if (!context.Users.Any(u => u.UserName == "aa@gmail.com"))
                    {
                        var store = new UserStore<ApplicationUser>(context);
                        var manager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
                        var user = new ApplicationUser { UserName = "aa@gmail.com", Email = "aa@gmail.com" };

                        manager.Create(user, "Teodora!1");
                        context.Users.Add(user);
                        manager.AddToRole(user.Id, "Trener");
                    }
              }

            /////////////////////
            ///

            if (!context.Roles.Any(r => r.Name == "Klijent"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Klijent" };

                manager.Create(role);
            }



            if (!context.Users.Any(u => u.UserName == "klijent@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "klijent@gmail.com", Email = "klijent@gmail.com" };

                manager.Create(user, "Teodora!1");
                context.Users.Add(user);
                manager.AddToRole(user.Id, "Klijent");
            }

        }
    }
    }
