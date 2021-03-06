﻿using PartyGamesSystem.Data.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PartyGamesSystem.Data.Models
{
    public class Category : IAuditInfo, IDeletableEntity
    {
        private ICollection<PartyGame> partyGames;

        public Category()
        {
            this.partyGames = new HashSet<PartyGame>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreatedOn {get;set;}

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<PartyGame> PartyGames
        {
            get { return this.partyGames; }
            set { this.partyGames = value; }
        }
    }
}
