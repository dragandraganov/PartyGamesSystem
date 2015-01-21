using System;
using System.Linq;
using System.Web.Mvc;
using PartyGamesSystem.Data;
using AutoMapper.QueryableExtensions;
using PartyGamesSystem.Web.Areas.Administration.AdminViewModels;
using PartyGamesSystem.Common;
using AutoMapper;
using PartyGamesSystem.Data.Models;
using System.Web;

namespace PartyGamesSystem.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class AdminCategoriesController : AdminController
    {
        public AdminCategoriesController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        // GET: Administration/AdminCategories
        public ActionResult Index()
        {
            var allCategories = this.Data
                .Categories
                .AllWithDeleted()
                .Project()
                .To<AdminCategoryViewModel>();

            return View(allCategories);
        }

        //GET: Add new category
        public ActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AdminCategoryViewModel categoryViewModel)
        {
            if (categoryViewModel != null && ModelState.IsValid)
            {
                var newCategory = Mapper.Map<Category>(categoryViewModel);
                this.Data.Categories.Add(newCategory);
                this.Data.SaveChanges();
                return RedirectToAction("Index", "AdminCategories");
            }

            return View();
        }

        //GET: Edit party game
        public ActionResult Edit(int id)
        {
            var existingCategory = this.Data
                .Categories
                .AllWithDeleted()
                .Where(pg => pg.Id == id)
                .Project()
                .To<AdminCategoryViewModel>()
                .FirstOrDefault();
            if (existingCategory == null)
            {
                throw new HttpException(404, "Category not found");
            }

            return View(existingCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminCategoryViewModel category)
        {
            if (category != null && ModelState.IsValid)
            {
                var existingCategory = this.Data
                    .Categories
                    .GetById(category.Id);
                Mapper.Map(category, existingCategory);
                
                this.Data.Categories.Update(existingCategory);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "AdminCategories");
            }

            return View(category);
        }

        //GET: Delete category

        public ActionResult Delete(int id)
        {
            var existingCategory = this.Data
                .Categories
                .AllWithDeleted()
                .Where(pg => pg.Id == id)
                .Project()
                .To<AdminCategoryViewModel>()
                .FirstOrDefault();
            if (existingCategory == null)
            {
                throw new HttpException(404, "Category not found");
            }
            return View(existingCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AdminCategoryViewModel category)
        {
            if (category != null && ModelState.IsValid)
            {
                var existingCategory = this.Data
                    .Categories
                    .GetById(category.Id);
                this.Data.Categories.Delete(existingCategory);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "AdminCategories");
            }

            return View(category);
        }

        public ActionResult HardDelete(int id)
        {
            return this.Delete(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HardDelete(AdminCategoryViewModel category)
        {
            if (category != null && ModelState.IsValid)
            {
                var existingCategory = this.Data
                    .Categories
                    .GetById(category.Id);
                Mapper.Map(category, existingCategory);
                this.Data.Categories.ActualDelete(existingCategory);
                this.Data.SaveChanges();

                return RedirectToAction("Index", "AdminCategories");
            }

            return View(category);
        }
    }
}