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
    public class RoomsController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Rooms/

        public ViewResult Index(int IdHotel)
        {
            var rooms = db.Rooms.Include("Hotel").Where(u => u.IdHotel.Equals(IdHotel));
            return View(rooms.ToList());
        }

        //
        // GET: /Rooms/Details/5

        public ViewResult Details(int id)
        {
            Room room = db.Rooms.Single(r => r.Id == id);
            return View(room);
        }

        //
        // GET: /Rooms/Create

        public ActionResult Create(int IdHotel)
        {
            ViewBag.Hotel = db.Hotels.Where(u => u.Id.Equals(IdHotel)).SingleOrDefault();
            return View();
        }

        //
        // POST: /Rooms/Create

        [HttpPost]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.AddObject(room);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdHotel = room.IdHotel });
            }

            ViewBag.IdHotel = new SelectList(db.Hotels, "Id", "Name", room.IdHotel);
            return View(room);
        }

        //
        // GET: /Rooms/Edit/5

        public ActionResult Edit(int id)
        {
            Room room = db.Rooms.Single(r => r.Id == id);
            ViewBag.IdHotel = new SelectList(db.Hotels, "Id", "Name", room.IdHotel);
            return View(room);
        }

        //
        // POST: /Rooms/Edit/5

        [HttpPost]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Attach(room);
                db.ObjectStateManager.ChangeObjectState(room, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdHotel = room.IdHotel });
            }
            ViewBag.IdHotel = new SelectList(db.Hotels, "Id", "Name", room.IdHotel);
            return View(room);
        }

        //
        // GET: /Rooms/Delete/5

        public ActionResult Delete(int id)
        {
            Room room = db.Rooms.Single(r => r.Id == id);
            return View(room);
        }

        //
        // POST: /Rooms/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Single(r => r.Id == id);
            db.Rooms.DeleteObject(room);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdHotel = room.IdHotel });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}