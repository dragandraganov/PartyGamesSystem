using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PartyGamesSystem.Data;
using PartyGamesSystem.Data.Models;

namespace PartyGamesSystem.Web.Areas.Administration.Controllers
{
    public class PartyGamesController : Controller
    {
        private PartyGamesSystemDbContext db = new PartyGamesSystemDbContext();

        // GET: Administration/PartyGames
        public ActionResult Index()
        {
            var partyGames = db.PartyGames.Include(p => p.Category).Include(p => p.Image);
            return View(partyGames.ToList());
        }

        // GET: Administration/PartyGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartyGame partyGame = db.PartyGames.Find(id);
            if (partyGame == null)
            {
                return HttpNotFound();
            }
            return View(partyGame);
        }

        // GET: Administration/PartyGames/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.ImageId = new SelectList(db.Images, "Id", "FileExtension");
            return View();
        }

        // POST: Administration/PartyGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,NecessaryItems,MinPlayingPeople,MaxPlayingPeople,ImageId,CategoryId,CreatedOn,PreserveCreatedOn,ModifiedOn,IsDeleted,DeletedOn")] PartyGame partyGame)
        {
            if (ModelState.IsValid)
            {
                db.PartyGames.Add(partyGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", partyGame.CategoryId);
            ViewBag.ImageId = new SelectList(db.Images, "Id", "FileExtension", partyGame.ImageId);
            return View(partyGame);
        }

        // GET: Administration/PartyGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartyGame partyGame = db.PartyGames.Find(id);
            if (partyGame == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", partyGame.CategoryId);
            ViewBag.ImageId = new SelectList(db.Images, "Id", "FileExtension", partyGame.ImageId);
            return View(partyGame);
        }

        // POST: Administration/PartyGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,NecessaryItems,MinPlayingPeople,MaxPlayingPeople,ImageId,CategoryId,CreatedOn,PreserveCreatedOn,ModifiedOn,IsDeleted,DeletedOn")] PartyGame partyGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partyGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", partyGame.CategoryId);
            ViewBag.ImageId = new SelectList(db.Images, "Id", "FileExtension", partyGame.ImageId);
            return View(partyGame);
        }

        // GET: Administration/PartyGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartyGame partyGame = db.PartyGames.Find(id);
            if (partyGame == null)
            {
                return HttpNotFound();
            }
            return View(partyGame);
        }

        // POST: Administration/PartyGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PartyGame partyGame = db.PartyGames.Find(id);
            db.PartyGames.Remove(partyGame);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
