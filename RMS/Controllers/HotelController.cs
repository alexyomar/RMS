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

        public ViewResult Index()
        {

            var hotels = db.Hotels.Include("State");
            return View(hotels.ToList());
        }

        //
        // GET: /Hotel/Details/5

        public ViewResult Details(int id)
        {
            Hotel hotel = db.Hotels.Single(h => h.Id == id);
            return View(hotel);
        }

        //
        // GET: /Hotel/Create

        public ActionResult Create()
        {
            ViewBag.IdState = new SelectList(db.States, "Id", "Name").OrderBy(u => u.Text);
            return View();
        } 

        //
        // POST: /Hotel/Create

        [HttpPost]
        public ActionResult Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotels.AddObject(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdState = new SelectList(db.States, "Id", "Name", hotel.IdState);
            return View(hotel);
        }
        
        //
        // GET: /Hotel/Edit/5
 
        public ActionResult Edit(int id)
        {
            Hotel hotel = db.Hotels.Single(h => h.Id == id);
            ViewBag.IdState = new SelectList(db.States, "Id", "Name", hotel.IdState);
            return View(hotel);
        }

        //
        // POST: /Hotel/Edit/5

        [HttpPost]
        public ActionResult Edit(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotels.Attach(hotel);
                db.ObjectStateManager.ChangeObjectState(hotel, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdState = new SelectList(db.States, "Id", "Name", hotel.IdState);
            return View(hotel);
        }

        //
        // GET: /Hotel/Delete/5
 
        public ActionResult Delete(int id)
        {
            Hotel hotel = db.Hotels.Single(h => h.Id == id);
            return View(hotel);
        }

        //
        // POST: /Hotel/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Hotel hotel = db.Hotels.Single(h => h.Id == id);
            db.Hotels.DeleteObject(hotel);
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