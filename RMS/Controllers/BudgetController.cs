using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace RMS.Controllers
{
    public class BudgetController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        public ViewResult Index()
        {
            var reservation = db.Reservation.Include("Customer").Include("ReservationStatus").OrderByDescending(x => x.Id);
            return View(reservation.ToList());
        }

        public ViewResult Details(int id)
        {
            Reservation reservation = db.Reservation.Single(r => r.Id == id);
            return View(reservation);
        }

        public ActionResult Create()
        {
            ViewBag.Customer = db.Customer.ToList();
            ViewBag.ReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
            ViewBag.Hotels = db.Hotel.OrderBy(u => u.Name);
            return View();
        }

        [HttpPost]
        public ActionResult Create(ReservationModel reservation)
        {


            ViewBag.Hotels = db.Hotel.OrderBy(u => u.Name);


            decimal __pricebase = new decimal();
            decimal __total = new decimal();
            decimal __totalrack = new decimal();

            var __rooms = reservation.Rooms.Where(z => z.Name != null);
            List<ReservationRoom> __listrr = new List<ReservationRoom>();
            foreach (var item in __rooms)
            {

                if (item.Name != null)
                {
                    var __room = db.Room.SingleOrDefault(u => u.Id.Equals(item.IdRoom));


                    IEnumerable<DateTime> __tripdays = EachDay(reservation.Trip.Arrival, reservation.Trip.Departure);
                    __tripdays = __tripdays.Take(__tripdays.Count() - 1);
                    reservation.Trip.Adults = reservation.Trip.Adults + item.Adultos;
                    reservation.Trip.Childrens = reservation.Trip.Childrens + (item.Infantes * item.Quantity);

                    foreach (var __day in __tripdays)
                    {
                        RoomOcupation __roomfare;

                        if (item.Infantes > 0)
                            __roomfare = db.RoomOcupation.Where(u => u.DateStart <= __day && u.DateEnd >= __day && u.Active.Equals(true) && u.IdRoom.Equals(__room.Id) && u.Room.Name.Equals(item.Name) && u.Name.ToLower().Equals("niño")).FirstOrDefault();
                        else
                            __roomfare = db.RoomOcupation.Where(u => u.DateStart <= __day && u.DateEnd >= __day && u.Active.Equals(true) && u.IdRoom.Equals(__room.Id) && u.Room.Name.Equals(item.Name) && u.Capacity.Equals(item.Adultos)).FirstOrDefault();

                        if (__roomfare != null)
                        {

                            decimal __discount = new decimal();

                            if (!reservation.Trip.Discount.HasValue)
                                if (__tripdays.Count() <= 3)
                                    __discount = __roomfare.Discount1;
                                else if (__tripdays.Count() >= 4 && __tripdays.Count() < 7)
                                    __discount = __roomfare.Discount2;
                                else
                                    __discount = __roomfare.Discount3;
                            else
                                if (reservation.Trip.Discount.Value != 0)
                                    __discount = reservation.Trip.Discount.Value;

                            decimal __admon = new decimal();

                            if (!reservation.Trip.PercentAdmin.HasValue)
                                __admon = __roomfare.PercentAdmin;
                            else
                                if (reservation.Trip.PercentAdmin.Value != 0)
                                    __admon = reservation.Trip.PercentAdmin.Value;

                            //descuento
                            decimal __facturabruta = __roomfare.PriceRack - (__roomfare.PriceRack * (__discount / 100));
                            //gastos admon
                            __facturabruta = __facturabruta + (__facturabruta * (__admon / 100));
                            //adultos
                            __facturabruta = __facturabruta * item.Quantity;
                            //rack
                            __totalrack = (__roomfare.PriceRack * item.Quantity) + __totalrack;

                            __total = __total + __facturabruta;

                            decimal __agent = new decimal();


                            if (!reservation.Trip.PercentAdmin.HasValue)
                                __agent = __roomfare.PercentAgent;
                            else
                                if (reservation.Trip.PercentAgent.Value != 0)
                                    __agent = reservation.Trip.PercentAgent.Value;


                            // Porcentaje del Vendedor
                            __pricebase = __pricebase + __roomfare.Price;
                            reservation.Trip.Discount = Convert.ToInt32(__discount);
                            reservation.Trip.PercentAdmin = Convert.ToInt32(__admon);
                            reservation.Trip.PercentAgent = Convert.ToInt32(__agent);
                            reservation.Trip.PriceBase = __pricebase;

                            ReservationRoom __rr = new ReservationRoom();
                            __rr.Quantity = item.Quantity;
                            __rr.IdRoomOcupation = __roomfare.Id;
                            __listrr.Add(__rr);


                        }
                        else
                        {
                            ViewBag.Customer = db.Customer.ToList();
                            ViewBag.Error = "No hay tarifas cargadas para la habitación " + __room.Name + " para la fecha seleccionada.";
                            return PartialView();
                        }
                    }



                }

            }

            if (__total > 0)
            {
                reservation.Trip.PriceRack = __totalrack;
                reservation.Trip.Price = __total;
                reservation.Trip.PriceBase = __pricebase;
                reservation.Trip.ReservationDate = DateTime.Now;


                db.Reservation.AddObject(reservation.Trip);
                db.SaveChanges();

                foreach (var item in __listrr)
                {
                    item.IdReservation = reservation.Trip.Id;
                    db.ReservationRoom.AddObject(item);
                    db.SaveChanges();
                }




                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "No ha seleccionado ninguna habitación.";
                ViewBag.Customer = db.Customer.ToList();
                ViewBag.ReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
                return View(reservation);
            }

        }

        public ActionResult Edit(int id)
        {
            Reservation reservation = db.Reservation.Single(r => r.Id == id);
            ViewBag.IdCustomer = new SelectList(db.Customer, "Id", "Name", reservation.IdCustomer);
            ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.IdReservationStatus);
            return View(reservation);
        }

        [HttpPost]
        public ActionResult Edit(Reservation reservation)
        {
            var __toedit = db.Reservation.SingleOrDefault(x => x.Id.Equals(reservation.Id));
            if (reservation.IdReservationStatus != 0)
            {
                __toedit.IdReservationStatus = reservation.IdReservationStatus;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCustomer = new SelectList(db.Customer, "Id", "Name", reservation.IdCustomer);
            ViewBag.IdReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.IdReservationStatus);
            return View(reservation);
        }

        public ActionResult Delete(int id)
        {
            Reservation reservation = db.Reservation.Single(r => r.Id == id);
            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservation.Single(r => r.Id == id);
            db.Reservation.DeleteObject(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult PrintableVersion(int id)
        {
            Reservation reservation = db.Reservation.Single(r => r.Id == id);
            return View(reservation);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        [Authorize]
        public ActionResult PrintPDF(int id)
        {
            MemoryStream __memoria = new MemoryStream();

            // step 1: creation of a document-object
            Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);

            // step 2:
            // we create a writer that listens to the document
            // and directs a XML-stream to a file
            PdfWriter __writer = PdfWriter.GetInstance(document, __memoria);
            __writer.CloseStream = false;

            var model = db.Reservation.SingleOrDefault(u => u.Id.Equals(id));

            string strB = RenderRazorViewToString("PrintableVersion", model);

            document.Open();

            // step 3: we parse the document
            using (TextReader sReader = new StringReader(strB))
            {

                List<IElement> list = HTMLWorker.ParseToList(sReader, new StyleSheet());

                foreach (IElement elm in list)
                {

                    document.Add(elm);
                }

            }

            document.Close();

            __memoria.Flush(); //Always catches me out
            __memoria.Position = 0; //Not sure if this is required
            return File(__memoria, "application/pdf", "resumen.pdf");
        }

        //Partials

        [Authorize]
        public ActionResult PartialHabitaciones(int IdHotel)
        {
            ViewBag.Room = db.Room.Where(u => u.IdHotel.Equals(IdHotel) && u.Active.Equals(true));
            return PartialView();

        }

        public ActionResult PartialPresupuesto(ReservationModel reservation)
        {

            ViewBag.Hotels = db.Hotel.OrderBy(u => u.Name);

            decimal __pricebase = new decimal();
            decimal __total = new decimal();
            decimal __totalrack = new decimal();


            var __rooms = reservation.Rooms.Where(z => z.Name != null);

            foreach (var item in __rooms)
            {

                if (item.Name != null)
                {
                    var __room = db.Room.SingleOrDefault(u => u.Id.Equals(item.IdRoom));
                    //var __maxcapacity = __room.RoomOcupation.OrderByDescending(model => model.Capacity).First().Capacity;

                    IEnumerable<DateTime> __tripdays = EachDay(reservation.Trip.Arrival, reservation.Trip.Departure);
                    __tripdays = __tripdays.Take(__tripdays.Count() - 1);
                    reservation.Trip.Adults = reservation.Trip.Adults + item.Adultos;
                    reservation.Trip.Childrens = reservation.Trip.Childrens + item.Infantes;

                    ViewBag.Days = __tripdays.Count();


                    #region FOREACH
                    foreach (var __day in __tripdays)
                    {
                        RoomOcupation __roomfare;

                        if (item.Infantes > 0)
                            __roomfare = db.RoomOcupation.Where(u => u.DateStart <= __day && u.DateEnd >= __day && u.Active.Equals(true) && u.IdRoom.Equals(__room.Id) && u.Room.Name.Equals(item.Name) && u.Name.ToLower().Equals("niño")).FirstOrDefault();
                        else
                            __roomfare = db.RoomOcupation.Where(u => u.DateStart <= __day && u.DateEnd >= __day && u.Active.Equals(true) && u.IdRoom.Equals(__room.Id) && u.Room.Name.Equals(item.Name) && u.Capacity.Equals(item.Adultos)).FirstOrDefault();

                        if (__roomfare != null)
                        {

                            decimal __discount = new decimal();

                            if (!reservation.Trip.Discount.HasValue)
                                if (__tripdays.Count() <= 3)
                                    __discount = __roomfare.Discount1;
                                else if (__tripdays.Count() >= 4 && __tripdays.Count() < 7)
                                    __discount = __roomfare.Discount2;
                                else
                                    __discount = __roomfare.Discount3;
                            else
                                if (reservation.Trip.Discount.Value != 0)
                                    __discount = reservation.Trip.Discount.Value;

                            decimal __admon = new decimal();

                            if (!reservation.Trip.PercentAdmin.HasValue)
                                __admon = __roomfare.PercentAdmin;
                            else
                                if (reservation.Trip.PercentAdmin.Value != 0)
                                    __admon = reservation.Trip.PercentAdmin.Value;

                            //descuento
                            decimal __facturabruta = __roomfare.PriceRack - (__roomfare.PriceRack * (__discount / 100));
                            //gastos admon
                            __facturabruta = __facturabruta + (__facturabruta * (__admon / 100));
                            //adultos
                            __facturabruta = __facturabruta * item.Quantity;
                            //rack
                            __totalrack = (__roomfare.PriceRack * item.Quantity) + __totalrack;

                            __total = __total + __facturabruta;

                            decimal __agent = new decimal();


                            if (!reservation.Trip.PercentAdmin.HasValue)
                                __agent = __roomfare.PercentAgent;
                            else
                                if (reservation.Trip.PercentAgent.Value != 0)
                                    __agent = reservation.Trip.PercentAgent.Value;


                            // Porcentaje del Vendedor
                            __pricebase = __pricebase + __roomfare.Price;
                            reservation.Trip.Discount = Convert.ToInt32(__discount);
                            reservation.Trip.PercentAdmin = Convert.ToInt32(__admon);
                            reservation.Trip.PercentAgent = Convert.ToInt32(__agent);
                            reservation.Trip.PriceBase = __pricebase;

                            ReservationRoom __rr = new ReservationRoom();
                            __rr.Quantity = item.Quantity;
                            __rr.RoomOcupation = __roomfare;
                            reservation.Trip.ReservationRoom.Add(__rr);

                        }
                        else
                        {

                            ViewBag.Error = "No hay tarifas cargadas para la habitación " + __room.Name + " para la fecha seleccionada.";
                            return PartialView();
                        }
                    }
                    #endregion


                }

            }

            if (__total > 0)
            {
                reservation.Trip.PriceBase = __pricebase;
                reservation.Trip.PriceRack = __totalrack;
                reservation.Trip.Price = __total;
                reservation.Trip.ReservationDate = DateTime.Now;
                return PartialView(reservation.Trip);
            }
            else
            {
                ViewBag.Error = "No ha seleccionado ninguna habitación.";
                return PartialView();
            }


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
}