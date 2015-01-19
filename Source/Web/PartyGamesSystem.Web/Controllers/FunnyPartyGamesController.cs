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
        public ActionResult Index()
        {
            var allPartyGames = this.Data.PartyGames.All().Project().To<PartyGameViewModel>();
            return View(allPartyGames);
        }
    }
}