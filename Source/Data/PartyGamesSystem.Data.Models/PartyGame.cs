namespace PartyGamesSystem.Data.Models
{
    using System.Collections.Generic;
    using PartyGamesSystem.Data.Contracts.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PartyGame : IAuditInfo, IDeletableEntity
    {
        public PartyGame()
        {
            this.Comments = new HashSet<Comment>();
            this.Ratings = new HashSet<Rating>();
            this.FavoredItUsers = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string NecessaryItems { get; set; }

        public int? MinPlayingPeople { get; set; }

        public int? MaxPlayingPeople { get; set; }

        //public string AuthorId { get; set; }

        [InverseProperty("CreatedPartyGames")]
        public virtual User Author { get; set; }

        public int? ImageId { get; set; }

        public virtual AppFile Image { get; set; }

        public int? AudioId { get; set; }

        public virtual AppFile Audio { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<User> FavoredItUsers { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
