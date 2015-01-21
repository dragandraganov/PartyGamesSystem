using AutoMapper;
using PartyGamesSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PartyGamesSystem.Web.Infrastructure.Mapping;

namespace PartyGamesSystem.Web.Areas.Administration.AdminViewModels
{
    public class AdminCategoryViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<PartyGame> PartyGames { get; set; }
       
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Category, AdminCategoryViewModel>()
                .ReverseMap();
        }
    }
}