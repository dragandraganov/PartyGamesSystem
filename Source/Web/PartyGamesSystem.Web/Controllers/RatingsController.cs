using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.ViewModels;
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

        public void Vote(int gameId, int ratingId)
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
            if (ratingId > -1) //TODO check is this game already voted from current user
            {
                this.ModifyRating(ratingId, ratingValue);
            }

            else
            {
                this.AddNewRating(gameId, ratingValue);
            }
        }

        private void ModifyRating(int ratingId, int ratingValue)
        {
            var ratingEntity = this.Data.Ratings
                .All()
                .Where(r => r.Id == ratingId)
                .FirstOrDefault();
            ratingEntity.Value = ratingValue;
            this.Data.Ratings.Update(ratingEntity);
            this.Data.SaveChanges();
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