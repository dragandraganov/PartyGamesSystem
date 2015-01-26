using System;
using System.Linq;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Controllers
{
    public class CommentsController : Controller
    {
        //TODO: Just for test - remove in production
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comments
        public ActionResult GetAllCommentsByGameId(int id)
        {
            return View();
        }
    }
}