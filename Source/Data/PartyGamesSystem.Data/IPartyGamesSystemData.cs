namespace PartyGamesSystem.Data
{
    using PartyGamesSystem.Data.Contracts.Repository;
    using PartyGamesSystem.Data.Models;
    using System;
    using System.Data.Entity;

    public interface IPartyGamesSystemData
    {
        DbContext Context { get; }

        IRepository<PartyGame> PartyGames { get; }

        IRepository<Category> Categories { get; }

        IRepository<Image> Images { get; }

        IRepository<User> Users { get; }

        void Dispose();

        int SaveChanges();
    }
}
