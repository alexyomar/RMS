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
        // GET: /Room/
        [Authorize]
        public ViewResult Index(int IdHotel)
        {
            ViewBag.Hotel = db.Hotel.SingleOrDefault(u => u.Id.Equals(IdHotel));
            var Room = db.Room.Include("Hotel").Where(u => u.IdHotel.Equals(IdHotel));
            return View(Room.ToList());
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
        public ActionResult Create(int IdHotel)
        {
            ViewBag.Hotel = db.Hotel.Where(u => u.Id.Equals(IdHotel)).SingleOrDefault();
            ViewBag.IdRoomType = new SelectList(db.RoomType.OrderBy(h => h.Name), "Id", "Name");
            return View();
        }

        //
        // POST: /Room/Create
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Room.AddObject(room);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdHotel = room.IdHotel });
            }

            ViewBag.IdHotel = new SelectList(db.Hotel, "Id", "Name", room.IdHotel);
            return View(room);
        }

        //
        // GET: /Room/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Room room = db.Room.Single(r => r.Id == id);
            ViewBag.Hotel = db.Hotel.Where(u => u.Id.Equals(room.IdHotel)).SingleOrDefault();
            ViewBag.IdHotel = new SelectList(db.Hotel, "Id", "Name", room.IdHotel);
            ViewBag.IdRoomType = new SelectList(db.RoomType.OrderBy(h => h.Name), "Id", "Name",  room.IdRoomType);
            return View(room);
        }

        //
        // POST: /Room/Edit/5
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Room.Attach(room);
                db.ObjectStateManager.ChangeObjectState(room, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { IdHotel = room.IdHotel });
            }
            ViewBag.IdHotel = new SelectList(db.Hotel, "Id", "Name", room.IdHotel);
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Room.Single(r => r.Id == id);
            db.Room.DeleteObject(room);
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