using AutoMapper;
using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Controllers
{
    public class CommentsController : BaseController
    {
        public CommentsController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Add(int partyGameId, CommentViewModel comment )
        {
            if (comment != null && ModelState.IsValid)
            {
                var newComment = Mapper.Map<Comment>(comment);
                newComment.Author = this.UserProfile;
                newComment.PartyGameId = partyGameId;

                this.Data.Comments.Add(newComment);
                this.Data.SaveChanges();

                comment.AuthorName = this.UserProfile.UserName;
                comment.CommentedOn = DateTime.Now;

                return PartialView("_CommentPartialView", comment);
            }

            return RedirectToAction("Details", "PartyGames", new { id = partyGameId });
        }
    }
}