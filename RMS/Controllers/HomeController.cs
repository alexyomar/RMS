using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;
using System.Web.Security;

namespace RMS.Controllers
{
    public class HomeController : Controller
    {
        private RegionalEntities db = new RegionalEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            Membership.CreateUser("alexyomar", "a17302339", "alexyomar@gmail.com");
            return View();
        }

        public ActionResult Database()
        {
            db.CreateDatabase();
            return View();
        }
    }
}
