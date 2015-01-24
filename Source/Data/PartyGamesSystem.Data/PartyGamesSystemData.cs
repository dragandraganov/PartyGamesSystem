using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Data.Contracts.Repository;
using PartyGamesSystem.Data.Contracts.Models;

namespace PartyGamesSystem.Data
{
    public class PartyGamesSystemData : IPartyGamesSystemData
    {
        private readonly DbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public PartyGamesSystemData(DbContext context)
        {
            this.context = context;
        }

        public DbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IDeletableEntityRepository<PartyGame> PartyGames
        {
            get
            {
                return this.GetRepository<PartyGame>();

            }
        }

        public IDeletableEntityRepository<Category> Categories
        {
            get
            {
                return this.GetRepository<Category>();

            }
        }

        public IDeletableEntityRepository<Image> Images
        {
            get
            {
                return this.GetRepository<Image>();
            }
        }

        public IDeletableEntityRepository<Comment> Comments
        {
            get
            {
                return this.GetRepository<Comment>();
            }
        }

        public IDeletableEntityRepository<Rating> Ratings
        {
            get
            {
                return this.GetRepository<Rating>();
            }
        }

        public IDeletableEntityRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IDeletableEntityRepository<T> GetRepository<T>() where T : class, IDeletableEntity
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(DeletableEntityRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeof(T)];
        }
    }
}
