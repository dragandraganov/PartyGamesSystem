using Microsoft.AspNet.Identity.EntityFramework;
using PartyGamesSystem.Data.Models;
using System;
using System.Linq;

namespace PartyGamesSystem.Data
{
    public class PartyGamesSystemDbContext : IdentityDbContext<User>
    {
        public PartyGamesSystemDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static PartyGamesSystemDbContext Create()
        {
            return new PartyGamesSystemDbContext();
        }
    }
}
