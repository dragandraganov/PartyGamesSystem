using System;
using System.Linq;
using System.Web.Mvc;
using PartyGamesSystem.Data;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.Areas.Administration.AdminViewModels;

namespace PartyGamesSystem.Web.Areas.Administration.Controllers
{
    
    public class AdminPartyGamesController : AdminController
    {
        public AdminPartyGamesController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        // GET: Administration/AdminPartyGames
        public ActionResult Index()
        {
            var allPartyGames = this.Data.PartyGames.AllWithDeleted().Project().To<AdminPartyGameViewModel>();
            return View(allPartyGames);
        }
    }
}