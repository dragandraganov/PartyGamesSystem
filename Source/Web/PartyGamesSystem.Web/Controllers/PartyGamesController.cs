using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.ViewModels;
using PartyGamesSystem.Data;

namespace PartyGamesSystem.Web.Controllers
{
    public class PartyGamesController : BaseController
    {
        public PartyGamesController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        // GET: PartyGames
        public ActionResult Index(string query)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            var allPartyGames = this.Data.PartyGames
                .All(pg => pg.Ratings)
                .Where(pg => pg.Title.Contains(query))
                .Project()
                .To<PartyGameViewModel>()
                .OrderBy(g => g.Id)
                .ToList();

            if (this.UserProfile != null)
            {
                foreach (var game in allPartyGames)
                {
                    game.CurrentUserRating = game.Ratings.Where(r => r.UserId == this.UserProfile.Id).FirstOrDefault();
                }
            }

            return View(allPartyGames);
        }
    }
}