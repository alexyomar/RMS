using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;

namespace RMS.Controllers
{
    public class PromoController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Promo/

        public ViewResult Index(int IdRoom)
        {
            ViewBag.Room = db.Rooms.SingleOrDefault(u => u.Id.Equals(IdRoom));
            var promotions = db.Promotions.Include("Room");
            return View(promotions.ToList());
        }

        //
        // GET: /Promo/Details/5

        public ViewResult Details(int id)
        {
            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            return View(promotion);
        }

        //
        // GET: /Promo/Create

        public ActionResult Create(int IdRoom)
        {
            ViewBag.Room = db.Rooms.SingleOrDefault(u => u.Id.Equals(IdRoom));
            return View();
        }

        //
        // POST: /Promo/Create

        [HttpPost]
        public ActionResult Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                db.Promotions.AddObject(promotion);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdRoom = promotion.IdRoom });
            }

            ViewBag.Room = db.Rooms.SingleOrDefault(u => u.Id.Equals(promotion.IdRoom));
            return View(promotion);
        }

        //
        // GET: /Promo/Edit/5

        public ActionResult Edit(int id)
        {
            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            return View(promotion);
        }

        //
        // POST: /Promo/Edit/5

        [HttpPost]
        public ActionResult Edit(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                db.Promotions.Attach(promotion);
                db.ObjectStateManager.ChangeObjectState(promotion, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdRoom = promotion.IdRoom });
            }
            return View(promotion);
        }

        //
        // GET: /Promo/Delete/5

        public ActionResult Delete(int id)
        {
            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            return View(promotion);
        }

        //
        // POST: /Promo/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            db.Promotions.DeleteObject(promotion);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdRoom = promotion.IdRoom });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}