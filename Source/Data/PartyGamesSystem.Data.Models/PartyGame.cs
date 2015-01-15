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

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string NecessaryItems { get; set; }

        public int? MinPlayingPeople { get; set; }

        public int? MaxPlayingPeople { get; set; }

        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public DateTime CreatedOn {get;set;}

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
