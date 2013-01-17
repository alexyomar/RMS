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
    public class RoomAccomodationController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /RoomAccomodation/
        [Authorize]
        public ViewResult Index(int Id)
        {
            ViewBag.Room = db.Room.SingleOrDefault(model => model.Id.Equals(Id));
            var roomoccupationbed = db.RoomOccupationBed.Include("Room").Include("RoomBed").Where(model => model.IdRoom.Equals(Id));
            return View(roomoccupationbed.ToList());
        }

        //
        // GET: /RoomAccomodation/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            RoomOccupationBed roomoccupationbed = db.RoomOccupationBed.Single(r => r.IdRoom == id);
            return View(roomoccupationbed);
        }

        //
        // GET: /RoomAccomodation/Create
        [Authorize]
        public ActionResult Create(int Id)
        {
            ViewBag.Room = db.Room.SingleOrDefault(model => model.Id.Equals(Id));
            ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name");
            return View();
        }

        //
        // POST: /RoomAccomodation/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(RoomOccupationBed roomoccupationbed)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.RoomOccupationBed.AddObject(roomoccupationbed);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { Id = roomoccupationbed.IdRoom });
                }
            }
            catch (Exception)
            {
                ViewBag.Room = db.Room.SingleOrDefault(model => model.Id.Equals(roomoccupationbed.IdRoom));
                ViewBag.IdRoom = roomoccupationbed.IdRoom;
                ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name", roomoccupationbed.IdRoomBed);
                ViewBag.Error = "Ya existe esa asignación de cama.";
                return View(roomoccupationbed);
            }

            ViewBag.Room = db.Room.SingleOrDefault(model => model.Id.Equals(roomoccupationbed.IdRoom));
            ViewBag.IdRoom = roomoccupationbed.IdRoom;
            ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name", roomoccupationbed.IdRoomBed);
            return View(roomoccupationbed);
        }

        //
        // GET: /RoomAccomodation/Edit/5
        [Authorize]
        public ActionResult Edit(int IdRoom, int IdRoomBed)
        {
            RoomOccupationBed roomoccupationbed = db.RoomOccupationBed.Single(r => r.IdRoom == IdRoom && r.IdRoomBed == IdRoomBed);
            ViewBag.IdRoom = IdRoom;
            ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name", roomoccupationbed.IdRoomBed);
            return View(roomoccupationbed);
        }

        //
        // POST: /RoomAccomodation/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(RoomOccupationBed roomoccupationbed)
        {
            if (ModelState.IsValid)
            {
                db.RoomOccupationBed.Attach(roomoccupationbed);
                db.ObjectStateManager.ChangeObjectState(roomoccupationbed, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = roomoccupationbed.IdRoom });
            }
            ViewBag.IdRoom = roomoccupationbed.IdRoom;
            ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name", roomoccupationbed.IdRoomBed);
            return View(roomoccupationbed);
        }

        //
        // GET: /RoomAccomodation/Delete/5
        [Authorize]
        public ActionResult Delete(int IdRoom, int IdRoomBed)
        {
            RoomOccupationBed roomoccupationbed = db.RoomOccupationBed.Single(r => r.IdRoom == IdRoom && r.IdRoomBed == IdRoomBed);
            return View(roomoccupationbed);
        }

        //
        // POST: /RoomAccomodation/Delete/5
         [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int IdRoom, int IdRoomBed)
        {
            RoomOccupationBed roomoccupationbed = db.RoomOccupationBed.Single(r => r.IdRoom == IdRoom && r.IdRoomBed == IdRoomBed);
            db.RoomOccupationBed.DeleteObject(roomoccupationbed);
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = roomoccupationbed.IdRoom });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}