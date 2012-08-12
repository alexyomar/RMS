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
    public class RoomRateController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /RoomRate/

        public ViewResult Index(int id)
        {
            var roomocupation = db.RoomOcupation.Include("Room").Where(u => u.IdRoom.Equals(id));
            ViewBag.Room = db.Room.SingleOrDefault(u => u.Id.Equals(id));
            return View(roomocupation.ToList());
        }

        //
        // GET: /RoomRate/Details/5

        public ViewResult Details(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            return View(roomocupation);
        }

        //
        // GET: /RoomRate/Create

        public ActionResult Create(int id)
        {
            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name");
            ViewBag.Room = db.Room.SingleOrDefault(u => u.Id.Equals(id));
            return View();
        }

        //
        // POST: /RoomRate/Create

        [HttpPost]
        public ActionResult Create(RoomOcupation roomocupation)
        {
            if (ModelState.IsValid)
            {
                db.RoomOcupation.AddObject(roomocupation);
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = roomocupation.IdRoom });
            }

            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name", roomocupation.IdRoom);
            return View(roomocupation);
        }

        //
        // GET: /RoomRate/Edit/5

        public ActionResult Edit(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name", roomocupation.IdRoom);
            return View(roomocupation);
        }

        //
        // POST: /RoomRate/Edit/5

        [HttpPost]
        public ActionResult Edit(RoomOcupation roomocupation)
        {
            if (ModelState.IsValid)
            {
                db.RoomOcupation.Attach(roomocupation);
                db.ObjectStateManager.ChangeObjectState(roomocupation, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = roomocupation.IdRoom });
            }
            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name", roomocupation.IdRoom);
            return View(roomocupation);
        }

        //
        // GET: /RoomRate/Delete/5

        public ActionResult Delete(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            return View(roomocupation);
        }

        //
        // POST: /RoomRate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            db.RoomOcupation.DeleteObject(roomocupation);
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = roomocupation.IdRoom });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}