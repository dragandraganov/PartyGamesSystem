using PartyGamesSystem.Common;
using PartyGamesSystem.Data;
using PartyGamesSystem.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public abstract class AdminController : BaseController
    {
        public AdminController(IPartyGamesSystemData data)
            : base(data)
        {
        }
    }
}