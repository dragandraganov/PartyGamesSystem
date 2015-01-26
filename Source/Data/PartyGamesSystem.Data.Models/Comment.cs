using PartyGamesSystem.Data.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyGamesSystem.Data.Models
{
    public class Comment : IAuditInfo, IDeletableEntity
    {
        public Comment()
        {
            this.Likes=new HashSet<Like>();
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public int PartyGameId { get; set; }

        public virtual PartyGame PartyGame { get; set; }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int LikeId { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public DateTime CreatedOn {get;set;}

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
