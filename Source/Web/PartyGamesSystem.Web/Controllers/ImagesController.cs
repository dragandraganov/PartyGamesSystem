using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartyGamesSystem.Data;

namespace PartyGamesSystem.Web.Controllers
{
    public class ImagesController : BaseController
    {
        public ImagesController(IPartyGamesSystemData data)
            : base(data)
        {
        }

        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Image(int id)
        {
            var image = this.Data.Images.GetById(id);
            if (image == null)
            {
                throw new HttpException(404, "Image not found");
            }

            return File(image.Content, "image/" + image.FileExtension);
        }
    }
}