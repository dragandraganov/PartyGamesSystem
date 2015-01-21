using System;
using System.Linq;
using System.Web.Mvc;
using PartyGamesSystem.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.Areas.Administration.AdminViewModels;
using PartyGamesSystem.Data.Models;
using System.IO;
using System.Collections.Generic;
using System.Web;
using PartyGamesSystem.Common;

namespace PartyGamesSystem.Web.Areas.Administration.Controllers
{
    [Authorize(Roles=GlobalConstants.AdminRole)]
    public class AdminPartyGamesController : AdminController
    {
        public AdminPartyGamesController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        // GET: Administration/AdminPartyGames/AllPartyGames
        public ActionResult Index()
        {
            var allPartyGames = this.Data
                .PartyGames
                .AllWithDeleted()
                .Project()
                .To<AdminPartyGameViewModel>();
            return View(allPartyGames);
        }

        //GET: Add new party game
        public ActionResult Add()
        {
            var newPartyGameViewModel = new AdminPartyGameViewModel
            {
                Categories = this.GetCategories()
            };

            return View(newPartyGameViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AdminPartyGameViewModel partyGame)
        {
            if (partyGame != null && ModelState.IsValid)
            {
                var newPartyGame = Mapper.Map<PartyGame>(partyGame);
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

                return RedirectToAction("Index", "AdminPartyGames");
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
                .To<AdminPartyGameViewModel>()
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
        public ActionResult Edit(AdminPartyGameViewModel partyGame)
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

                return RedirectToAction("Index", "AdminPartyGames");
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
                .To<AdminPartyGameViewModel>()
                .FirstOrDefault();
            if (existingPartyGame == null)
            {
                throw new HttpException(404, "Party game not found");
            }
            //var allCategories = this.GetCategories();
            //existingPartyGame.Categories = new SelectList(allCategories, "Value", "Text");
            return View(existingPartyGame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AdminPartyGameViewModel partyGame)
        {
            if (partyGame != null && ModelState.IsValid)
            {
                var existingPartyGame = this.Data
                    .PartyGames
                    .GetById(partyGame.Id);
                this.Data.PartyGames.Delete(existingPartyGame);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "AdminPartyGames");
            }

            return View(partyGame);
        }

        public ActionResult HardDelete(int id)
        {
            return this.Delete(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HardDelete(AdminPartyGameViewModel partyGame)
        {
            if (partyGame != null && ModelState.IsValid)
            {
                var existingPartyGame = this.Data
                    .PartyGames
                    .GetById(partyGame.Id);
                this.Data.PartyGames.ActualDelete(existingPartyGame);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "AdminPartyGames");
            }

            return View(partyGame);
        }

        public ActionResult Image(int id)
        {
            var image = this.Data.Images.GetById(id);
            if (image == null)
            {
                throw new HttpException(404, "Image not found");
            }

            return File(image.Content, "image/" + image.FileExtension);
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var categories = this.Data.Categories
                       .All()
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();

            return categories;
        }
    }
}