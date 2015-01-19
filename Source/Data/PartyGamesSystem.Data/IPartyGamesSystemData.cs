﻿namespace PartyGamesSystem.Data
{
    using PartyGamesSystem.Data.Contracts.Repository;
    using PartyGamesSystem.Data.Models;
    using System;
    using System.Data.Entity;

    public interface IPartyGamesSystemData
    {
        DbContext Context { get; }

        IDeletableEntityRepository<PartyGame> PartyGames { get; }

        IDeletableEntityRepository<Category> Categories { get; }

        IDeletableEntityRepository<Image> Images { get; }

        IDeletableEntityRepository<User> Users { get; }

        void Dispose();

        int SaveChanges();
    }
}
