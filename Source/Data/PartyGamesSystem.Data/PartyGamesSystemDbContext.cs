﻿using Microsoft.AspNet.Identity.EntityFramework;
using PartyGamesSystem.Data.Models;
using System;
using System.Linq;
using System.Data.Entity;
using PartyGamesSystem.Data.Migrations;
using PartyGamesSystem.Data.Contracts.Models;

namespace PartyGamesSystem.Data
{
    public class PartyGamesSystemDbContext : IdentityDbContext<User>
    {
        public PartyGamesSystemDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PartyGamesSystemDbContext, Configuration>());
        }

        public IDbSet<PartyGame> PartyGames { get; set; }

        public IDbSet<Image> Images { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public static PartyGamesSystemDbContext Create()
        {
            return new PartyGamesSystemDbContext();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return base.SaveChanges();
        }


        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            var entries= this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));
            
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                        entity.PreserveCreatedOn = true;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (
                var entry in
                    this.ChangeTracker.Entries()
                        .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}
