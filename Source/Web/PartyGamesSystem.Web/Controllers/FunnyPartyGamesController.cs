using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.ViewModels;
using PartyGamesSystem.Data;

namespace PartyGamesSystem.Web.Controllers
{
    public class FunnyPartyGamesController : BaseController
    {
        public FunnyPartyGamesController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        // GET: PartyGames
        public ActionResult Index(string query)
        {
            var contro = RouteData.Values;
            if (query==null)
            {
                query = string.Empty;
            }
            var allPartyGames = this.Data.PartyGames
                .All()
                .Where(pg => pg.Title.Contains(query))
                .Project()
                .To<PartyGameViewModel>();
            return View(allPartyGames);
        }
    }
}