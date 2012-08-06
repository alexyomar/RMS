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
    public class HotelController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Hotel/
        [Authorize]
        public ViewResult Index()
        {

            var Hotel = db.Hotel.Include("State");
            return View(Hotel.ToList());
        }

        //
        // GET: /Hotel/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Hotel hotel = db.Hotel.Single(h => h.Id == id);
            return View(hotel);
        }

        //
        // GET: /Hotel/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdState = new SelectList(db.State, "Id", "Name").OrderBy(u => u.Text);
            return View();
        }

        //
        // POST: /Hotel/Create
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotel.AddObject(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdState = new SelectList(db.State, "Id", "Name", hotel.IdState);
            return View(hotel);
        }

        //
        // GET: /Hotel/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Hotel hotel = db.Hotel.Single(h => h.Id == id);
            ViewBag.IdState = new SelectList(db.State, "Id", "Name", hotel.IdState);
            return View(hotel);
        }

        //
        // POST: /Hotel/Edit/5
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotel.Attach(hotel);
                db.ObjectStateManager.ChangeObjectState(hotel, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdState = new SelectList(db.State, "Id", "Name", hotel.IdState);
            return View(hotel);
        }

        //
        // GET: /Hotel/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Hotel hotel = db.Hotel.Single(h => h.Id == id);
            return View(hotel);
        }

        //
        // POST: /Hotel/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotel.Single(h => h.Id == id);
            db.Hotel.DeleteObject(hotel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}