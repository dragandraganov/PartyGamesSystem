using System;
using System.Linq;
using System.Web.Mvc;
using PartyGamesSystem.Data;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.ViewModels;
using AutoMapper;
using PartyGamesSystem.Data.Models;
using System.IO;
using System.Web;
using PartyGamesSystem.Common;
using PartyGamesSystem.Web.Infrastructure.Sanitizer;

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
        public ActionResult Index(string query)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            var allPartyGames = this.Data.PartyGames
                .All()
                .Where(pg => pg.Author.Id == this.UserProfile.Id)
                .Where(pg => pg.Title.Contains(query))
                .Project()
                .To<PartyGameViewModel>()
                .ToList();

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
                if (this.sanitizer.Sanitize(partyGame.Description) == string.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Your description contains avoid potentially dangerous html tags. Edit it.");
                    partyGame.Categories = this.GetCategories();
                    return View(partyGame);
                }

                if (partyGame.NecessaryItems != null && this.sanitizer.Sanitize(partyGame.NecessaryItems) == string.Empty)
                {
                    ModelState.AddModelError(string.Empty, "The description of the necessary items contain potentially html tags. Edit it.");
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
                var existingPartyGame = this.Data
                    .PartyGames
                    .GetById(partyGame.Id);

                if (this.sanitizer.Sanitize(partyGame.Description) == string.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Your description contains avoid potentially dangerous characters. Edit it.");
                    partyGame.Categories = this.GetCategories();
                    return View(partyGame);
                }

                if (partyGame.NecessaryItems != null && this.sanitizer.Sanitize(partyGame.NecessaryItems) == string.Empty)
                {
                    ModelState.AddModelError(string.Empty, "The description of the necessary items contain potentially dangerous characters. Edit it.");
                    partyGame.Categories = this.GetCategories();
                    return View(partyGame);
                }

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

    }
}