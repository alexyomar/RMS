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
    public class RoomTypeController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /RoomType/

        public ViewResult Index()
        {
            return View(db.RoomType.ToList());
        }

        //
        // GET: /RoomType/Details/5

        public ViewResult Details(int id)
        {
            RoomType roomtype = db.RoomType.Single(r => r.Id == id);
            return View(roomtype);
        }

        //
        // GET: /RoomType/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /RoomType/Create

        [HttpPost]
        public ActionResult Create(RoomType roomtype)
        {
            if (ModelState.IsValid)
            {
                db.RoomType.AddObject(roomtype);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(roomtype);
        }
        
        //
        // GET: /RoomType/Edit/5
 
        public ActionResult Edit(int id)
        {
            RoomType roomtype = db.RoomType.Single(r => r.Id == id);
            return View(roomtype);
        }

        //
        // POST: /RoomType/Edit/5

        [HttpPost]
        public ActionResult Edit(RoomType roomtype)
        {
            if (ModelState.IsValid)
            {
                db.RoomType.Attach(roomtype);
                db.ObjectStateManager.ChangeObjectState(roomtype, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomtype);
        }

        //
        // GET: /RoomType/Delete/5
 
        public ActionResult Delete(int id)
        {
            RoomType roomtype = db.RoomType.Single(r => r.Id == id);
            return View(roomtype);
        }

        //
        // POST: /RoomType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            RoomType roomtype = db.RoomType.Single(r => r.Id == id);
            db.RoomType.DeleteObject(roomtype);
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