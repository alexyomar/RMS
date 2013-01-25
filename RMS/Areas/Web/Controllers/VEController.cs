using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS.Areas.Web.Controllers
{
    public class VEController : Controller
    {
        //
        // GET: /Web/VE/

        public RMS.Models.RegionalEntities __db = new RMS.Models.RegionalEntities();

        //VIEWS
        public ActionResult Index()
        {
            ViewBag.Banners = __db.Banner.Where(u => u.IdBannerType.Equals(1));
            ViewBag.Destacado = __db.Banner.Where(u => u.IdBannerType.Equals(2)).OrderByDescending(u => u.Timestamp).FirstOrDefault();
            ViewBag.Content = __db.Content.Where(u => u.Active.Equals(true) && u.Featured.Equals(true)).OrderByDescending(u => u.TimestampEdit);

            return View();
        }
        public ActionResult Seccion(int Id, string name)
        {
            IEnumerable<RMS.Models.Content> __data = __db.Content.Where(u => u.IdContentType.Equals(Id));

            ViewBag.Title = __db.ContentType.Where(u => u.Id.Equals(Id)).First().Name + " - Regional Marketing";
            return View(__data);
        }
        public ActionResult Presupuesto()
        {
            return View();
        }
        public ActionResult Cotizaciones()
        {
            return View();
        }

        public ActionResult Nosotros()
        {
            return View();
        }

        public ActionResult Contactenos()
        {
            return View();
        }

        public ActionResult PartialContenido(int Id)
        {
            RMS.Models.Content __data = __db.Content.Where(u => u.Id.Equals(Id)).Single();
            return View(__data);
        }


    }
}
