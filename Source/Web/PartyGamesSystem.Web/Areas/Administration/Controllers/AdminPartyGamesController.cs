using System;
using System.Linq;
using System.Web.Mvc;
using PartyGamesSystem.Data;
using AutoMapper;
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

        // GET: Administration/AdminPartyGames/AllPartyGames
        public ActionResult Index()
        {
            //TODO Grid control in the view
            var allPartyGames = this.Data.PartyGames.AllWithDeleted().Project().To<AdminPartyGameViewModel>();
            return View(allPartyGames);
        }

        //GET: Edit party game
        public ActionResult Edit(int id)
        {
            var existingPartyGame = Mapper.Map<AdminPartyGameViewModel>(this.Data.PartyGames.GetById(id));
            return View(existingPartyGame);
        }
    }
}