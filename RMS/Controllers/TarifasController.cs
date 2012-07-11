using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Regional.Models;

namespace Regional.Controllers
{ 
    public class TarifasController : Controller
    {
        private RMSContext db = new RMSContext();

        //
        // GET: /Tarifas/

        public ViewResult Index()
        {
            return View(db.Rooms.ToList());
        }

        //
        // GET: /Tarifas/Details/5

        public ViewResult Details(int id)
        {
            Room room = db.Rooms.Find(id);
            return View(room);
        }

        //
        // GET: /Tarifas/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Tarifas/Create

        [HttpPost]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(room);
        }
        
        //
        // GET: /Tarifas/Edit/5
 
        public ActionResult Edit(int id)
        {
            Room room = db.Rooms.Find(id);
            return View(room);
        }

        //
        // POST: /Tarifas/Edit/5

        [HttpPost]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        //
        // GET: /Tarifas/Delete/5
 
        public ActionResult Delete(int id)
        {
            Room room = db.Rooms.Find(id);
            return View(room);
        }

        //
        // POST: /Tarifas/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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