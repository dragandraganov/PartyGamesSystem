using System;
using System.Linq;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Controllers
{
    public class CommentsController : Controller
    {
        // GET: Comments
        public ActionResult GetAllCommentsByGameId(int id)
        {
            return View();
        }
    }
}