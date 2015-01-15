using AutoMapper;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.Infrastructure.Mapping;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace PartyGamesSystem.Web.ViewModels
{
    public class PartyGameViewModel : IMapFrom<PartyGame>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string[] NecessaryItems { get; set; }

        public int? MinPlayingPeople { get; set; }

        public int? MaxPlayingPeople { get; set; }

        public int? ImageId { get; set; }

        public string CategoryName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PartyGame, PartyGameViewModel>()
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(t => t.Category.Name))
                .ReverseMap();
        }
    }
}