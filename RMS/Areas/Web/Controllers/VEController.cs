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

        public RMS.Models.RegionalEntities __db = new Models.RegionalEntities();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Seccion(int Id, string name)
        {
            IEnumerable<RMS.Models.Content> __data = __db.Content.Where(u => u.IdContentType.Equals(Id));
            return View(__data);
        }

        public ActionResult PartialContenido(int Id)
        {
            RMS.Models.Content __data = __db.Content.Where(u => u.Id.Equals(Id)).Single();
            return View(__data);
        }


    }
}
