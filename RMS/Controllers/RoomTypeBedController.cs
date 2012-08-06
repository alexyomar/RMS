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
    public class RoomTypeBedController : Controller
    {

        private RegionalEntities db = new RegionalEntities();

        public ViewResult Index()
        {
            var roomtypebed = db.RoomTypeBed.Include("RoomBed").Include("RoomType");
            return View(roomtypebed.ToList());
        }


        public ActionResult Create()
        {
            ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name");
            ViewBag.IdRoomType = new SelectList(db.RoomType, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoomTypeBed roomtypebed)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.RoomTypeBed.AddObject(roomtypebed);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name", roomtypebed.IdRoomBed);
                ViewBag.IdRoomType = new SelectList(db.RoomType, "Id", "Name", roomtypebed.IdRoomType);
                return View(roomtypebed);
            }
            catch (Exception)
            {
                ViewBag.Error = "El tipo de ocupación que desea crear ya existe.";
                ViewBag.IdRoomBed = new SelectList(db.RoomBed, "Id", "Name", roomtypebed.IdRoomBed);
                ViewBag.IdRoomType = new SelectList(db.RoomType, "Id", "Name", roomtypebed.IdRoomType);
                return View(roomtypebed);
            }
        }



        public ActionResult Delete(int IdRoomType, int IdRoomBed)
        {
            RoomTypeBed roomtypebed = db.RoomTypeBed.Single(r => r.IdRoomType == IdRoomType && r.IdRoomBed == IdRoomBed);
            return View(roomtypebed);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int IdRoomType, int IdRoomBed)
        {
            RoomTypeBed roomtypebed = db.RoomTypeBed.Single(r => r.IdRoomType == IdRoomType && r.IdRoomBed == IdRoomBed);
            db.RoomTypeBed.DeleteObject(roomtypebed);
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