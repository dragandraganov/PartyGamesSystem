using PartyGamesSystem.Data.Contracts.Repository;
using PartyGamesSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.ViewModels;

namespace PartyGamesSystem.Web.Controllers
{
    public class FunnyPartyGamesController : Controller
    {
        private readonly IDeletableEntityRepository<PartyGame> partyGames;

        public FunnyPartyGamesController(IDeletableEntityRepository<PartyGame> partyGames)
        {
            this.partyGames = partyGames;
        }

        // GET: PartyGames
        public ActionResult Index()
        {
            var allPartyGames = this.partyGames.All().Project().To<PartyGameViewModel>();
            return View(allPartyGames);
        }
    }
}