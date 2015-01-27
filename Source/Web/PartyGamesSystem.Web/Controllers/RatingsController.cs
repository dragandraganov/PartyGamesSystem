using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PartyGamesSystem.Web.Controllers
{
    public class RatingsController : BaseController
    {
        public RatingsController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        public void Vote(int partyGameId)
        {
            if (!this.Request.IsAjaxRequest())
            {
                return;
            }

            string rating = this.Request["rating"];

            if (rating == null) //TODO check is this game already voted from current user and modify Rating
            {
                return;
            }

            int ratingValue = int.Parse(this.Request["rating"]);
            if (false) //TODO check is this game already voted from current user
            {

            }
            else
            {
                this.AddNewRating(partyGameId, ratingValue);
            }
        }

        public void AddNewRating(int partyGameId, int value)
        {
            var newRating = new Rating();
            newRating.Value = value;
            newRating.PartyGameId = partyGameId;
            string currentUserId = this.UserProfile.Id;
            newRating.UserId = currentUserId;
            this.Data.Ratings.Add(newRating);
            this.Data.SaveChanges();
        }
    }
}