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
    public class ReservationController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /Reservation/

        public ViewResult Index()
        {
            var reservations = db.Reservations.Include("Customer").Include("ReservationStatu");
            return View(reservations.ToList());
        }

        //
        // GET: /Reservation/Details/5

        public ViewResult Details(int id)
        {
            Reservation reservation = db.Reservations.Single(r => r.Id == id);
            return View(reservation);
        }

        //
        // GET: /Reservation/Create

        public ActionResult Create()
        {
            ViewBag.IdCustomer = new SelectList(db.Customers, "Id", "Name");
            ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
            ViewBag.Hotels = db.Hotels.OrderBy(u => u.Name);
            return View();
        }

        //
        // POST: /Reservation/Create

        [HttpPost]
        public ActionResult Create(ReservationModel reservation)
        {
            if (ModelState.IsValid)
            {

                foreach (var item in db.Rooms.Where(u => reservation.Rooms.Select(a => a.IdRoom).Contains(u.Id)))
                {
                    reservation.Trip.Rooms.Add(item);
                }


                db.Reservations.AddObject(reservation.Trip);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            ViewBag.IdCustomer = new SelectList(db.Customers, "Id", "Name", reservation.Trip.IdCustomer);
            ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.Trip.IdReservationStatus);
            return View(reservation);
        }

        public ActionResult PartialHabitaciones(int IdHotel)
        {
            ViewBag.Rooms = db.Rooms.Where(u => u.IdHotel.Equals(IdHotel));
            return PartialView();

        }

        //
        // GET: /Reservation/Edit/5

        public ActionResult Edit(int id)
        {
            Reservation reservation = db.Reservations.Single(r => r.Id == id);
            ViewBag.IdCustomer = new SelectList(db.Customers, "Id", "Name", reservation.IdCustomer);
            ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.IdReservationStatus);
            return View(reservation);
        }

        //
        // POST: /Reservation/Edit/5

        [HttpPost]
        public ActionResult Edit(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Attach(reservation);
                db.ObjectStateManager.ChangeObjectState(reservation, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCustomer = new SelectList(db.Customers, "Id", "Name", reservation.IdCustomer);
            ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.IdReservationStatus);
            return View(reservation);
        }

        //
        // GET: /Reservation/Delete/5

        public ActionResult Delete(int id)
        {
            Reservation reservation = db.Reservations.Single(r => r.Id == id);
            return View(reservation);
        }

        //
        // POST: /Reservation/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Single(r => r.Id == id);
            db.Reservations.DeleteObject(reservation);
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