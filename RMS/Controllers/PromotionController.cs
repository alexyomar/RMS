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
    public class PromotionController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Promotion/

        public ViewResult Index(int IdRoom)
        {
            var promotions = db.Promotions.Include("Room").Where(x => x.IdRoom.Equals(IdRoom));
            return View(promotions.ToList());
        }

        //
        // GET: /Promotion/Details/5

        public ViewResult Details(int id)
        {
            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            return View(promotion);
        }

        //
        // GET: /Promotion/Create

        public ActionResult Create(int IdRoom)
        {
            ViewBag.IdRoom = IdRoom;
            return View();
        }

        //
        // POST: /Promotion/Create

        [HttpPost]
        public ActionResult Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                db.Promotions.AddObject(promotion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRoom = new SelectList(db.Rooms, "Id", "Name", promotion.IdRoom);
            return View(promotion);
        }

        //
        // GET: /Promotion/Edit/5

        public ActionResult Edit(int id)
        {
            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            ViewBag.IdRoom = new SelectList(db.Rooms, "Id", "Name", promotion.IdRoom);
            return View(promotion);
        }

        //
        // POST: /Promotion/Edit/5

        [HttpPost]
        public ActionResult Edit(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                db.Promotions.Attach(promotion);
                db.ObjectStateManager.ChangeObjectState(promotion, EntityState.Modified);
                int IdRoom = promotion.IdRoom;
                db.SaveChanges();
                return RedirectToAction("Index", new { IdRoom = IdRoom });
            }
            ViewBag.IdRoom = new SelectList(db.Rooms, "Id", "Name", promotion.IdRoom);
            return View(promotion);
        }

        //
        // GET: /Promotion/Delete/5

        public ActionResult Delete(int id)
        {
            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            return View(promotion);
        }

        //
        // POST: /Promotion/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            Promotion promotion = db.Promotions.Single(p => p.Id == id);
            int IdRoom = promotion.IdRoom;
            db.Promotions.DeleteObject(promotion);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdRoom = IdRoom });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}