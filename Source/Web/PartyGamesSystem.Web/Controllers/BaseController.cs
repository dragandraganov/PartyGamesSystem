using PartyGamesSystem.Data;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IPartyGamesSystemData Data { get; private set; }

        public BaseController(IPartyGamesSystemData data)
        {
            this.Data = data;
        }
    }
}