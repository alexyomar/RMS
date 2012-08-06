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
    public class RoomBedController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /RoomBed/

        public ViewResult Index()
        {
            return View(db.RoomBed.ToList());
        }

        //
        // GET: /RoomBed/Details/5

        public ViewResult Details(int id)
        {
            RoomBed roombed = db.RoomBed.Single(r => r.Id == id);
            return View(roombed);
        }

        //
        // GET: /RoomBed/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /RoomBed/Create

        [HttpPost]
        public ActionResult Create(RoomBed roombed)
        {
            if (ModelState.IsValid)
            {
                db.RoomBed.AddObject(roombed);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(roombed);
        }
        
        //
        // GET: /RoomBed/Edit/5
 
        public ActionResult Edit(int id)
        {
            RoomBed roombed = db.RoomBed.Single(r => r.Id == id);
            return View(roombed);
        }

        //
        // POST: /RoomBed/Edit/5

        [HttpPost]
        public ActionResult Edit(RoomBed roombed)
        {
            if (ModelState.IsValid)
            {
                db.RoomBed.Attach(roombed);
                db.ObjectStateManager.ChangeObjectState(roombed, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roombed);
        }

        //
        // GET: /RoomBed/Delete/5
 
        public ActionResult Delete(int id)
        {
            RoomBed roombed = db.RoomBed.Single(r => r.Id == id);
            return View(roombed);
        }

        //
        // POST: /RoomBed/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            RoomBed roombed = db.RoomBed.Single(r => r.Id == id);
            db.RoomBed.DeleteObject(roombed);
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