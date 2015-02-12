using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.Infrastructure.ActionResults;
using PartyGamesSystem.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PartyGamesSystem.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IPartyGamesSystemData Data { get; private set; }

        protected User UserProfile { get; private set; }

        public BaseController(IPartyGamesSystemData data)
        {
            this.Data = data;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.Data.Users.All().Where(u => u.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }

        public IEnumerable<SelectListItem> GetCategories()
        {
            var categories = this.Data.Categories
                       .All()
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();

            return categories;
        }

        public void AddCurrentUserRating(IEnumerable<PartyGameViewModel> allPartyGames)
        {
            if (this.UserProfile != null)
            {
                foreach (var game in allPartyGames)
                {
                    game.CurrentUserRating = game.Ratings
                        .Where(r => r.UserId == this.UserProfile.Id)
                        .FirstOrDefault();
                }
            }
        }

        [Obsolete("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.")]
        protected JsonResult Json<T>(T data)
        {
            throw new InvalidOperationException("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.");
        }

        protected StandardJsonResult JsonValidationError()
        {
            var result = new StandardJsonResult();

            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddError(validationError.ErrorMessage);
            }

            return result;
        }

        protected StandardJsonResult JsonError(string errorMessage)
        {
            var result = new StandardJsonResult();

            result.AddError(errorMessage);

            return result;
        }

        protected StandardJsonResult<T> JsonSuccess<T>(T data)
        {
            return new StandardJsonResult<T> { Data = data };
        }

        //protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        //{
        //    this.ViewBag.Settings = new SettingsManager(this.GetSettings);
        //    return base.BeginExecute(requestContext, callback, state);
        //}

        //protected IDictionary<string, string> GetSettings()
        //{
        //    return this.Settings.All().ToDictionary(x => x.Name, x => x.Value);
        //}
    }
}