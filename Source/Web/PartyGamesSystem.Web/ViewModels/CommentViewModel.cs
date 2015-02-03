namespace PartyGamesSystem.Web.ViewModels
{
    using AutoMapper;
    using PartyGamesSystem.Data.Models;
    using PartyGamesSystem.Web.Infrastructure.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public CommentViewModel()
        {

        }

        public CommentViewModel(int partyGameId)
        {
            this.PartyGameId = partyGameId;
        }

        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int PartyGameId { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public DateTime CommentedOn { get; set; }

        public int PreviousCommentsCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
               .ForMember(cm => cm.AuthorName, opt => opt.MapFrom(c => c.Author.UserName))
               .ForMember(cm => cm.CommentedOn, opt => opt.MapFrom(c => c.CreatedOn))
               .ReverseMap();
        }
    }
}