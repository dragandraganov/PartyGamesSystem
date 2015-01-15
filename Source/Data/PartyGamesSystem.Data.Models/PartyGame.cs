using PartyGamesSystem.Data.Contracts.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PartyGamesSystem.Data.Models
{
    public class PartyGame : IAuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn {get;set;}

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
