using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;
using PartyGamesSystem.Web.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;

namespace PartyGamesSystem.Web.Controllers
{
    public class RatingsController : BaseController
    {
        public RatingsController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        public ActionResult Vote(int gameId, int ratingId)
        {
            var votedGame = this.Data
               .PartyGames.All()
               .Where(pg => pg.Id == gameId).Project()
               .To<PartyGameViewModel>()
               .FirstOrDefault();

            var ratingEntity = this.Data.Ratings
               .All()
               .Where(r => r.Id == ratingId)
               .FirstOrDefault();

            votedGame.CurrentUserRating = ratingEntity;

            if (!this.Request.IsAjaxRequest())
            {
                return PartialView("_PartyGameSingleView", votedGame); //TODO Return appropriate message
            }

            string rating = this.Request["rating"];

            if (rating == null)
            {
                return PartialView("_PartyGameSingleView", votedGame);
            }

            int ratingValue = int.Parse(rating);
            if (ratingId > -1)
            {
                this.ModifyRating(ratingValue, ratingEntity, votedGame);
            }

            else
            {
                this.AddNewRating(gameId, ratingValue, votedGame);
            }

            return PartialView("_PartyGameSingleView", votedGame);
        }

        private void ModifyRating(int ratingValue, Rating ratingEntity, PartyGameViewModel votedGame)
        {
            ratingEntity.Value = ratingValue;
            this.Data.Ratings.Update(ratingEntity);
            this.Data.SaveChanges();
            votedGame.CurrentUserRating = ratingEntity;
        }

        public void AddNewRating(int partyGameId, int value, PartyGameViewModel votedGame)
        {
            var newRating = new Rating();
            newRating.Value = value;
            newRating.PartyGameId = partyGameId;
            string currentUserId = this.UserProfile.Id;
            newRating.UserId = currentUserId;
            this.Data.Ratings.Add(newRating);
            this.Data.SaveChanges();
            votedGame.CurrentUserRating = newRating;
            votedGame.Ratings.Add(newRating);
        }
    }
}