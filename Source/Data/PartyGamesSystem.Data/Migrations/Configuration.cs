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

            if (context.Users.Where(u => u.Email == "kakagina@bg.bg").Count() == 0)
            {
                this.userManager = new UserManager<User>(new UserStore<User>(context));

                var user = new User();
                user.UserName = "kakagina@bg.bg";
                user.Email = user.UserName;
                this.userManager.Create(user, "Dr@g06");

                this.userManager.AddToRole(user.Id, GlobalConstants.AdminRole);
                context.SaveChanges();
            }

        }
    }
}
