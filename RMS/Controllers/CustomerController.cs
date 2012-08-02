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
    public class CustomerController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Customer/
        [Authorize]
        public ViewResult Index()
        {
            return View(db.Customers.ToList());
        }

        //
        // GET: /Customer/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Customer customer = db.Customers.Single(c => c.Id == id);
            return View(customer);
        }

        //
        // GET: /Customer/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customer/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.AddObject(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        //
        // GET: /Customer/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Customer customer = db.Customers.Single(c => c.Id == id);
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Attach(customer);
                db.ObjectStateManager.ChangeObjectState(customer, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //
        // GET: /Customer/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Customer customer = db.Customers.Single(c => c.Id == id);
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Single(c => c.Id == id);
            db.Customers.DeleteObject(customer);
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