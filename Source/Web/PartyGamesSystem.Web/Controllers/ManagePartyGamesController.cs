using System;
using System.Linq;
using System.Web.Mvc;
using PartyGamesSystem.Data;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.ViewModels;
using AutoMapper;
using PartyGamesSystem.Data.Models;
using System.IO;
using PartyGamesSystem.Web.Infrastructure.Sanitizer;
using System.Web;
using System.Collections.Generic;

namespace PartyGamesSystem.Web.Controllers
{
    [Authorize]
    public class ManagePartyGamesController : BaseController
    {
        private readonly ISanitizer sanitizer;

        public ManagePartyGamesController(IPartyGamesSystemData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        // GET: All User's Games
        public ActionResult Index(string query, string index = "ownGames")
        {
            var allPartyGames = new List<PartyGameViewModel>();

            if (query == null)
            {
                query = string.Empty;
            }

            var entityPartyGames = this.Data.PartyGames
                .All()
                .Where(pg => pg.Title.Contains(query));

            if (index == "ownGames")
            {
                allPartyGames = entityPartyGames
                     .Where(pg => pg.Author.Id == this.UserProfile.Id)
                     .Project()
                     .To<PartyGameViewModel>().ToList();
            }

            else
            {
                allPartyGames = Mapper.Map<ICollection<PartyGame>, ICollection<PartyGameViewModel>>(this.UserProfile.FavouritePartyGames).ToList();
            }

            base.AddCurrentUserRating(allPartyGames);

            return View(allPartyGames);
        }

        //GET: Create new game
        public ActionResult Create()
        {
            var newPartyGameViewModel = new PartyGameViewModel
            {
                Categories = this.GetCategories()
            };
            return View(newPartyGameViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PartyGameViewModel partyGame)
        {
            if (partyGame != null && ModelState.IsValid)
            {
                if (this.sanitizer.Sanitize(partyGame.Description) != partyGame.Description)
                {
                    ModelState.AddModelError(string.Empty, "Your description contains potentially dangerous code. Edit it.");
                    partyGame.Categories = this.GetCategories();
                    return View(partyGame);
                }

                if (partyGame.NecessaryItems != null && this.sanitizer.Sanitize(partyGame.NecessaryItems) != partyGame.NecessaryItems)
                {
                    ModelState.AddModelError(string.Empty, "The description of the necessary items contains  potentially dangerous code. Edit it.");
                    partyGame.Categories = this.GetCategories();
                    return View(partyGame);
                }

                var newPartyGame = Mapper.Map<PartyGame>(partyGame);
                newPartyGame.Author = this.UserProfile;

                if (partyGame.UploadedImage != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        partyGame.UploadedImage.InputStream.CopyTo(memory);
                        var content = memory.GetBuffer();

                        newPartyGame.Image = new AppFile
                        {
                            Content = content,
                            FileExtension = partyGame.UploadedImage.FileName.Split(new[] { '.' }).Last()
                        };
                    }
                }

                else
                {
                    using (var memory = new MemoryStream())
                    {
                        string path = HttpContext.Server.MapPath("~/Content/Images/logo-gray.png");

                        using (var file = new FileStream(path, FileMode.Open))
                        {
                            file.CopyTo(memory);
                            var content = memory.GetBuffer();
                            newPartyGame.Image = new AppFile
                            {
                                Content = content,
                                FileExtension = "png"
                            };
                        }
                    }
                }

                if (partyGame.UploadedAudio != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        partyGame.UploadedAudio.InputStream.CopyTo(memory);
                        var content = memory.GetBuffer();

                        newPartyGame.Audio = new AppFile
                        {
                            Content = content,
                            FileExtension = partyGame.UploadedAudio.FileName.Split(new[] { '.' }).Last()
                        };
                    }
                }

                this.Data.PartyGames.Add(newPartyGame);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "ManagePartyGames");
            }

            partyGame.Categories = this.GetCategories();
            return View(partyGame);
        }

        //GET: Edit party game
        public ActionResult Edit(int id)
        {
            var existingPartyGame = this.Data
                .PartyGames
                .All()
                .Where(pg => pg.Id == id)
                .Project()
                .To<PartyGameViewModel>()
                .FirstOrDefault();

            if (existingPartyGame == null)
            {
                return new HttpNotFoundResult("Party game not found");
            }

            if (existingPartyGame.AuthorName != this.UserProfile.UserName)
            {
                return new HttpNotFoundResult("You are not the author of this game!");
            }

            var allCategories = this.GetCategories();
            existingPartyGame.Categories = new SelectList(allCategories, "Value", "Text");
            return View(existingPartyGame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartyGameViewModel partyGame)
        {
            if (partyGame.AuthorName != this.UserProfile.UserName)
            {
                return new HttpNotFoundResult("You are not the author of this game!");
            }


            if (partyGame != null && ModelState.IsValid)
            {
                if (this.sanitizer.Sanitize(partyGame.Description) != partyGame.Description)
                {
                    ModelState.AddModelError(string.Empty, "Your description contains potentially dangerous code. Edit it.");
                    partyGame.Categories = this.GetCategories();
                    return View(partyGame);
                }

                if (partyGame.NecessaryItems != null && this.sanitizer.Sanitize(partyGame.NecessaryItems) != partyGame.NecessaryItems)
                {
                    ModelState.AddModelError(string.Empty, "The description of the necessary items contains  potentially dangerous code. Edit it.");
                    partyGame.Categories = this.GetCategories();
                    return View(partyGame);
                }

                var existingPartyGame = this.Data
                   .PartyGames
                   .GetById(partyGame.Id);

                Mapper.Map(partyGame, existingPartyGame);

                if (partyGame.UploadedImage != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        partyGame.UploadedImage.InputStream.CopyTo(memory);
                        var content = memory.GetBuffer();

                        existingPartyGame.Image = new AppFile
                        {
                            Content = content,
                            FileExtension = partyGame.UploadedImage.FileName.Split(new[] { '.' }).Last()
                        };
                    }
                }

                if (partyGame.UploadedAudio != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        partyGame.UploadedAudio.InputStream.CopyTo(memory);
                        var content = memory.GetBuffer();

                        existingPartyGame.Audio = new AppFile
                        {
                            Content = content,
                            FileExtension = partyGame.UploadedAudio.FileName.Split(new[] { '.' }).Last()
                        };
                    }
                }

                existingPartyGame.Ratings = this.Data.Ratings
                    .All()
                    .Where(r => r.PartyGameId == existingPartyGame.Id)
                    .ToList();

                existingPartyGame.Comments = this.Data.Comments
                    .All()
                    .Where(r => r.PartyGameId == existingPartyGame.Id)
                    .ToList();

                this.Data.PartyGames.Update(existingPartyGame);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "ManagePartyGames");
            }

            partyGame.Categories = this.GetCategories();

            return View(partyGame);
        }

        public ActionResult Delete(int id)
        {
            var existingPartyGame = this.Data
                .PartyGames
                .All()
                .Where(pg => pg.Id == id)
                .Project()
                .To<PartyGameViewModel>()
                .FirstOrDefault();
            if (existingPartyGame == null)
            {
                return new HttpNotFoundResult("Party game not found");
            }

            if (existingPartyGame.AuthorName != this.UserProfile.UserName)
            {
                return new HttpNotFoundResult("You are not the author of this game!");
            }

            return View(existingPartyGame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PartyGameViewModel partyGame)
        {
            if (partyGame.AuthorName != this.UserProfile.UserName)
            {
                return new HttpNotFoundResult("You are not the author of this game!");
            }

            if (partyGame != null && ModelState.IsValid)
            {
                var existingPartyGame = this.Data
                    .PartyGames
                    .GetById(partyGame.Id);
                this.Data.PartyGames.Delete(existingPartyGame);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "ManagePartyGames");
            }

            return View(partyGame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToFavorites(int gameId)
        {
            var existingGame = this.Data.PartyGames.All().FirstOrDefault(pg => pg.Id == gameId);

            if (existingGame != null && ModelState.IsValid)
            {
                var currentUser = this.UserProfile;
                currentUser.FavouritePartyGames.Add(existingGame);
                this.Data.SaveChanges();
                var gameModel = Mapper.Map<PartyGame, PartyGameViewModel>(existingGame);
                gameModel.IsFavoritedByCurrentUser = true;
                return RedirectToAction("Details", "PartyGames", new { id = gameId });
                //return View("~/Views/PartyGames/Details.cshtml", gameModel);
            }

            return new HttpNotFoundResult("Party game not found");
        }
    }
}