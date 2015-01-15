using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PartyGamesSystem.Data.Models
{
    public class Category
    {
        private ICollection<PartyGame> tickets;

        public Category()
        {
            this.tickets = new HashSet<PartyGame>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<PartyGame> PartyGames
        {
            get { return this.tickets; }
            set { this.tickets = value; }
        }
    }
}
