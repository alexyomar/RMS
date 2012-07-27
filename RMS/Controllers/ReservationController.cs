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

                foreach (var item in reservation.Rooms)
                {
                    Room __room = db.Rooms.SingleOrDefault(u => u.Id.Equals(item.IdRoom));
                    Room __roominfante = db.Rooms.SingleOrDefault(u => u.Name.ToLower().Equals("infante") && u.IdHotel.Equals(__room.IdHotel));

                    if (item.Adultos <= __room.Capacity)
                    {
                        reservation.Trip.Rooms.Add(__room);
                        reservation.Trip.Adults = reservation.Trip.Adults + item.Adultos;
                        reservation.Trip.Childrens = reservation.Trip.Childrens + item.Infantes;

                        IEnumerable<DateTime> __tripdays = EachDay(reservation.Trip.Arrival, reservation.Trip.Departure);
                        __tripdays = __tripdays.Take(__tripdays.Count() - 1);

                        //Obtenemos los precios por dia
                        foreach (DateTime __day in __tripdays)
                        {

                            var __temporada = __room.Hotel.Periods.Where(u => __day.IsBetween(new DateTime(DateTime.Now.Year, u.BeginMonth, u.BeginDay), new DateTime(DateTime.Now.Year, u.EndMonth, u.EndDay)));
                            var __promo = __room.Promotions.Where(u => __day.IsBetween(u.DateStart, u.DateEnd) && u.Active.Equals(true));

                            if (__temporada.Count() > 0)
                            {
                                if (__promo.Count() > 0)
                                    if (__promo.FirstOrDefault().MinAdults >= item.Adultos && __promo.FirstOrDefault().MinDays >= __tripdays.Count())
                                        reservation.Trip.Price = (__promo.FirstOrDefault().HighSeasonPrice * item.Adultos) + reservation.Trip.Price;
                                    else
                                        reservation.Trip.Price = (__room.HighSeasonPrice * item.Adultos) + reservation.Trip.Price;
                                else
                                    reservation.Trip.Price = (__room.HighSeasonPrice * item.Adultos) + reservation.Trip.Price;
                                if (item.Infantes > 0)
                                    reservation.Trip.Price = (__roominfante.HighSeasonPrice * item.Infantes) + reservation.Trip.Price;
                            }
                            else
                            {
                                if (__promo.Count() > 0)
                                    reservation.Trip.Price = (__promo.FirstOrDefault().LowSeasonPrice * item.Adultos) + reservation.Trip.Price;
                                else
                                    reservation.Trip.Price = (__room.LowSeasonPrice * item.Adultos) + reservation.Trip.Price;

                                if (item.Infantes > 0)
                                    reservation.Trip.Price = (__roominfante.LowSeasonPrice * item.Infantes) + reservation.Trip.Price;
                            }
                        }

                        db.Reservations.AddObject(reservation.Trip);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.IdCustomer = new SelectList(db.Customers, "Id", "Name", reservation.Trip.IdCustomer);
                        ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.Trip.IdReservationStatus);
                        ViewBag.Error = "El numero de Adultos excede la capacidad de alguna de las habitaciones.";
                        return View(reservation);
                    }



                }
                return RedirectToAction("Index");
            }

            ViewBag.IdCustomer = new SelectList(db.Customers, "Id", "Name", reservation.Trip.IdCustomer);
            ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.Trip.IdReservationStatus);
            return View(reservation);
        }

        public ActionResult PartialHabitaciones(int IdHotel)
        {
            ViewBag.Rooms = db.Rooms.Where(u => u.IdHotel.Equals(IdHotel) && !u.Name.ToLower().Contains("infante") && u.Active.Equals(true));
            if (db.Rooms.Where(u => u.Name.ToLower().Contains("infante")).Count() > 0)
                ViewBag.ShowInfantes = true;
            else
                ViewBag.ShowInfantes = false;

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

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }


    }

    public static class MyExtensions
    {
        public static bool IsBetween(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }
    }

}
