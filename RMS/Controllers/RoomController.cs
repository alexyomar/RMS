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
    public class RoomController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Room/
        [Authorize]
        public ViewResult Index(int Id)
        {
            var room = db.Room.Include("Hotel").Where(u => u.IdHotel.Equals(Id));
            ViewBag.Hotel = db.Hotel.SingleOrDefault(u => u.Id.Equals(Id));
            return View(room.ToList());
        }

        //
        // GET: /Room/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Room room = db.Room.Single(r => r.Id == id);
            return View(room);
        }

        //
        // GET: /Room/Create
        [Authorize]
        public ActionResult Create(int Id)
        {
            ViewBag.Hotel = db.Hotel.SingleOrDefault(u => u.Id.Equals(Id));
            return View();
        }

        //
        // POST: /Room/Create

        [HttpPost]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Room.AddObject(room);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = room.IdHotel });
            }

            ViewBag.Hotel = db.Hotel.SingleOrDefault(u => u.Id.Equals(room.IdHotel));
            return View(room);
        }

        //
        // GET: /Room/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Room room = db.Room.Single(r => r.Id == id);
            return View(room);
        }

        //
        // POST: /Room/Edit/5

        [HttpPost]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Room.Attach(room);
                db.ObjectStateManager.ChangeObjectState(room, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = room.IdHotel });
            }
            return View(room);
        }

        //
        // GET: /Room/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Room room = db.Room.Single(r => r.Id == id);
            return View(room);
        }

        //
        // POST: /Room/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Room.Single(r => r.Id == id);
            db.Room.DeleteObject(room);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = room.IdHotel });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}