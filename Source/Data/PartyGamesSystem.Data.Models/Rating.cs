using PartyGamesSystem.Data.Contracts.Models;
using System;
using System.Linq;

namespace PartyGamesSystem.Data.Models
{
    public class Rating : IAuditInfo, IDeletableEntity
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public int PartyGameId { get; set; }

        public virtual PartyGame PartyGame { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
