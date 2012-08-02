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
    public class SeasonController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Season/
        [Authorize]
        public ViewResult Index(int IdHotel)
        {
            ViewBag.Hotel = db.Hotels.SingleOrDefault(u => u.Id.Equals(IdHotel));
            var periods = db.Periods.Include("Hotel");
            return View(periods.ToList());
        }

        //
        // GET: /Season/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Period period = db.Periods.Single(p => p.Id == id);
            return View(period);
        }

        //
        // GET: /Season/Create
        [Authorize]
        public ActionResult Create(int IdHotel)
        {
            ViewBag.Hotel = db.Hotels.SingleOrDefault(u => u.Id.Equals(IdHotel));
            return View();
        }

        //
        // POST: /Season/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Period period)
        {
            if (ModelState.IsValid)
            {
                db.Periods.AddObject(period);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdHotel = period.IdHotel });
            }

            ViewBag.IdHotel = new SelectList(db.Hotels, "Id", "Name", period.IdHotel);
            return View(period);
        }

        //
        // GET: /Season/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Period period = db.Periods.Single(p => p.Id == id);
            ViewBag.IdHotel = new SelectList(db.Hotels, "Id", "Name", period.IdHotel);
            return View(period);
        }

        //
        // POST: /Season/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Period period)
        {
            if (ModelState.IsValid)
            {
                db.Periods.Attach(period);
                db.ObjectStateManager.ChangeObjectState(period, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdHotel = period.IdHotel });
            }
            ViewBag.IdHotel = new SelectList(db.Hotels, "Id", "Name", period.IdHotel);
            return View(period);
        }

        //
        // GET: /Season/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Period period = db.Periods.Single(p => p.Id == id);
            return View(period);
        }

        //
        // POST: /Season/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Period period = db.Periods.Single(p => p.Id == id);
            db.Periods.DeleteObject(period);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdHotel = period.IdHotel });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}