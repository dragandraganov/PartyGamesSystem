using System.Collections.Generic;
using AutoMapper;
using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.Infrastructure.Sanitizer;
using PartyGamesSystem.Web.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ISanitizer sanitizer;

        //public CommentsController(IPartyGamesSystemData data)
        //    : base(data)
        //{
        //}

        public CommentsController(IPartyGamesSystemData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Add(int partyGameId, CommentViewModel comment)
        {
            if (comment != null && ModelState.IsValid)
            {
                if (this.sanitizer.Sanitize(comment.Content) == string.Empty)
                {
                    return this.JsonError("Your comment is potentially dangerous code. Edit it.");
                }

                var newComment = Mapper.Map<Comment>(comment);

                newComment.Author = this.UserProfile;
                newComment.PartyGameId = partyGameId;

                this.Data.Comments.Add(newComment);
                this.Data.SaveChanges();

                comment.AuthorName = this.UserProfile.UserName;
                comment.CommentedOn = DateTime.Now;

                return PartialView("_CommentPartialView", comment);
            }

            return JsonError("Unexpexted error");
        }

    }
}