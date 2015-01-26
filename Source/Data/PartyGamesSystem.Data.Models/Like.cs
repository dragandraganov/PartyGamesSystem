using PartyGamesSystem.Data.Contracts.Models;
using System;
using System.Linq;

namespace PartyGamesSystem.Data.Models
{
    public class Like : IAuditInfo, IDeletableEntity
    {
        public int Id { get; set; }

        public int AuthorID { get; set; }

        public virtual User Author { get; set; }

        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
