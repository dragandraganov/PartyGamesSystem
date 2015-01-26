using AutoMapper;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyGamesSystem.Web.ViewModels
{
    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int PartyGameId { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
               .ForMember(m => m.AuthorName, opt => opt.MapFrom(t => t.Author.UserName))
               .ReverseMap();
        }
    }
}