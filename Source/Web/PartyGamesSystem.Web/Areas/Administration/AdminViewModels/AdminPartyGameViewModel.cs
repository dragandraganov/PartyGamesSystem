using System;
using System.Linq;
using AutoMapper;
using PartyGamesSystem.Web.Infrastructure.Mapping;
using PartyGamesSystem.Data.Models;
using System.ComponentModel;

namespace PartyGamesSystem.Web.Areas.Administration.AdminViewModels
{
    public class AdminPartyGameViewModel : IMapFrom<PartyGame>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string NecessaryItems { get; set; }

        public int? MinPlayingPeople { get; set; }

        public int? MaxPlayingPeople { get; set; }

        //TODO implement Image property of Model
        //public virtual Image Image { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PartyGame, AdminPartyGameViewModel>()
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(t => t.Category.Name))
                .ReverseMap();
        }
    }
}