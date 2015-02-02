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

namespace PartyGamesSystem.Web.Controllers
{
    [Authorize]
    public class ManagePartyGamesController : BaseController
    {
        public ManagePartyGamesController(IPartyGamesSystemData data)
            : base(data)
        {
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
                .To<PartyGameViewModel>();
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
                var newPartyGame = Mapper.Map<PartyGame>(partyGame);
                newPartyGame.Author = this.UserProfile;
                if (partyGame.UploadedImage != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        partyGame.UploadedImage.InputStream.CopyTo(memory);
                        var content = memory.GetBuffer();

                        newPartyGame.Image = new Image
                        {
                            Content = content,
                            FileExtension = partyGame.UploadedImage.FileName.Split(new[] { '.' }).Last()
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
                .AllWithDeleted()
                .Where(pg => pg.Id == id)
                .Project()
                .To<PartyGameViewModel>()
                .FirstOrDefault();
            if (existingPartyGame == null)
            {
                throw new HttpException(404, "Party game not found");
            }
            var allCategories = this.GetCategories();
            existingPartyGame.Categories = new SelectList(allCategories, "Value", "Text");
            return View(existingPartyGame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartyGameViewModel partyGame)
        {
            if (partyGame != null && ModelState.IsValid)
            {
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

                        existingPartyGame.Image = new Image
                        {
                            Content = content,
                            FileExtension = partyGame.UploadedImage.FileName.Split(new[] { '.' }).Last()
                        };
                    }
                }
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
                .AllWithDeleted()
                .Where(pg => pg.Id == id)
                .Project()
                .To<PartyGameViewModel>()
                .FirstOrDefault();
            if (existingPartyGame == null)
            {
                throw new HttpException(404, "Party game not found");
            }
            return View(existingPartyGame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PartyGameViewModel partyGame)
        {
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