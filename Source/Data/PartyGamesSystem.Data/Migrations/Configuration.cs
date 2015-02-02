namespace PartyGamesSystem.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PartyGamesSystem.Common;
    using PartyGamesSystem.Data.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<PartyGamesSystemDbContext>
    {

        private UserManager<User> userManager;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            // TODO: Remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PartyGamesSystemDbContext context)
        {

            if (context.Users.FirstOrDefault(u => u.Email == "bibito@bg.bg") == null)
            {
                this.userManager = new UserManager<User>(new UserStore<User>(context));

                if (context.Roles.FirstOrDefault(r=>r.Name==GlobalConstants.AdminRole)==null)
                {
                    var adminRole = new IdentityRole(GlobalConstants.AdminRole);
                    context.Roles.Add(adminRole);
                    context.SaveChanges();
                }

                var user = new User();
                user.UserName = "bibito@bg.bg";
                user.Email = user.UserName;
                this.userManager.Create(user, "Dr@g06");
                this.userManager.AddToRole(user.Id, GlobalConstants.AdminRole);
                context.SaveChanges();
            }

        }
    }
}
