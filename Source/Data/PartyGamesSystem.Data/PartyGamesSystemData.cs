using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Data.Contracts.Repository;

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

        public IRepository<PartyGame> PartyGames
        {
            get
            {
                return this.GetRepository<PartyGame>();

            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                return this.GetRepository<Category>();

            }
        }

        public IRepository<Image> Images
        {
            get
            {
                return this.GetRepository<Image>();
            }
        }

        public IRepository<User> Users
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

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
