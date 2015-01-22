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
        public ActionResult Index()
        {
            var allPartyGames = this.Data.PartyGames
                .All()
                .Where(pg => pg.Author.Id == this.UserProfile.Id)
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

                return RedirectToAction("Index", "FunnyPartyGames");
            }

            partyGame.Categories = this.GetCategories();
            return View(partyGame);
        }
    }
}