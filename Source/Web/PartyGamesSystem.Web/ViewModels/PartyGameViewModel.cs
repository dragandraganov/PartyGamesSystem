using AutoMapper;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.Infrastructure.Mapping;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using GridMvc.DataAnnotations;

namespace PartyGamesSystem.Web.ViewModels
{
    public class PartyGameViewModel : IMapFrom<PartyGame>, IHaveCustomMappings, IValidatableObject
    {
        public int Id { get; set; }

        [UIHint("MultiLineText")]
        [Required]
        public string Title { get; set; }

        [UIHint("MultiLineText")]
        [Required]
        public string Description { get; set; }

        [UIHint("MultiLineText")]
        public string NecessaryItems { get; set; }

        //TODO make custom validation for MinPlayingPeople and MaxPlayingPeople
        public int? MinPlayingPeople { get; set; }

        public int? MaxPlayingPeople { get; set; }

        [Display(Name = "Category")]
        [UIHint("DropDownList")]
        public int? CategoryId { get; set; }

        [GridColumn(Title = "Category")]
        public string CategoryName { get; set; }

        public int? ImageId { get; set; }

        public string AuthorName { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual IList<CommentViewModel> Comments { get; set; }

        public Rating CurrentUserRating { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadedImage { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PartyGame, PartyGameViewModel>()
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(t => t.Category.Name))
                .ForMember(m => m.AuthorName, opt => opt.MapFrom(t => t.Author.UserName))
                .ReverseMap();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var validImageTypes = new List<string>()
            //{
            //    "image/gif",
            //    "image/jpeg",
            //    "image/pjpeg",
            //    "image/png"
            //};

            if (this.MinPlayingPeople != null)
            {
                int number;
                if (!(int.TryParse(this.MinPlayingPeople.ToString(), out number)))
                {
                    yield return new ValidationResult("Min playing people must be a number.", new[] { "MinPlayingPeople" });
                }
            }
        }

        public string GetAverageRating()
        {
            if (this.Ratings.Count() != 0)
            {
                return String.Format("{0} / 5 from {1} users", ((double)this.Ratings.Sum(r => r.Value) / this.Ratings.Count()).ToString("F1"), this.Ratings.Count());
            }

            return "Not rated yet";
        }
    }
}