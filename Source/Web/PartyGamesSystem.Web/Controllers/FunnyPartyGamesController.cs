using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Controllers
{
    public class FunnyPartyGamesController : Controller
    {
        // GET: PartyGames
        public ActionResult Index()
        {
            return View();
        }
    }
}