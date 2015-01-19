using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using PartyGamesSystem.Web.Infrastructure.Mapping;
using PartyGamesSystem.Data.Models;
using System.ComponentModel;
using GridMvc.DataAnnotations;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PartyGamesSystem.Web.Areas.Administration.AdminViewModels
{
    public class AdminPartyGameViewModel : IMapFrom<PartyGame>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [UIHint("MultiLineText")]
        public string Title { get; set; }

        [UIHint("MultiLineText")]
        public string Description { get; set; }

        [UIHint("MultiLineText")]
        public string NecessaryItems { get; set; }

        public int? MinPlayingPeople { get; set; }

        public int? MaxPlayingPeople { get; set; }

        //TODO implement Image property of Model
        //public virtual Image Image { get; set; }

        [Display(Name = "Category")]
        [UIHint("DropDownList")]
        public int? CategoryId { get; set; }

        [GridColumn(Title = "Category")]
        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public HttpPostedFileBase UploadedImage { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PartyGame, AdminPartyGameViewModel>()
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(t => t.Category.Name))
                .ReverseMap();
        }
    }
}