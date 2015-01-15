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

        public string Title { get; set; }

        public string Description { get; set; }

        public string ListOfNecessaryItems { get; set; }

        public int? MinPlayingPeople { get; set; }

        public int? MaxPlayingPeople { get; set; }

        public int? ImageId { get; set; }

        public string CategoryName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PartyGame, PartyGameViewModel>()
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(t => t.Category.Name))
                .ForMember(m => m.ListOfNecessaryItems, opt => opt.MapFrom(t => String.Join(", ",t.NecessaryItems.AsEnumerable())))
                .ReverseMap();
        }
    }
}