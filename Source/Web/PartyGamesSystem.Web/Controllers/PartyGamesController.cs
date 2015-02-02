using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.ViewModels;
using PartyGamesSystem.Data;
using System.Web;

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

            base.AddCurrentUserRating(allPartyGames);

            return View(allPartyGames);
        }
        //GET: Party game details
        public ActionResult Details(int id)
        {
            var existingPartyGame = this.Data
                .PartyGames
                .AllWithDeleted()
                .Where(pg => pg.Id == id)
                .Project()
                .To<PartyGameViewModel>()
                .FirstOrDefault();
            if (existingPartyGame == null)
            {
                throw new HttpException(404, "Party game not found");
            }
            return View(existingPartyGame);
        }

    }
}