using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace PartyGamesSystem.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IPartyGamesSystemData Data { get; private set; }

        protected User UserProfile { get; private set; }

        public BaseController(IPartyGamesSystemData data)
        {
            this.Data = data;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.Data.Users.All().Where(u => u.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}