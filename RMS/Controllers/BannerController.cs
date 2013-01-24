using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;
using System.IO;

namespace RMS.Controllers
{
    public class BannerController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Banner/
        [Authorize]
        public ViewResult Index()
        {
            var banner = db.BannerType;
            return View(banner.ToList());
        }

        //
        // GET: /Banner/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Banner banner = db.Banner.Single(b => b.Id == id);
            return View(banner);
        }

        //
        // GET: /Banner/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdBannerType = new SelectList(db.BannerType, "Id", "Name");
            return View();
        }

        //
        // POST: /Banner/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Banner banner, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                banner.Timestamp = DateTime.Now;

                var filename = Guid.NewGuid();

                bool IsExist = Directory.Exists(Server.MapPath("~/Content/Upload/Banners/" + banner.IdBannerType));
                if (!IsExist)
                    Directory.CreateDirectory(Server.MapPath("~/Content/Upload/Banners/" + banner.IdBannerType));


                var path = Server.MapPath("~/Content/Upload/Banners/" + banner.IdBannerType + "/" + filename.ToString().Substring(0, 8) + ".jpg");
                imagen.SaveAs(path);
                banner.URL = "~/Content/Upload/Banners/" + banner.IdBannerType + "/" + filename.ToString().Substring(0, 8) + ".jpg";
                banner.Featured = false;
                db.Banner.AddObject(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdBannerType = new SelectList(db.BannerType, "Id", "Name", banner.IdBannerType);
            return View(banner);
        }

        //
        // POST: /Banner/Edit/5
        [Authorize]

        public ActionResult Edit(int Id)
        {

            Banner banner = db.Banner.Single(b => b.Id == Id);
            banner.Timestamp = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");

        }


        //
        // POST: /Banner/Delete/5
        [Authorize]

        public ActionResult Delete(int id)
        {
            Banner banner = db.Banner.Single(b => b.Id == id);
            db.Banner.DeleteObject(banner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}