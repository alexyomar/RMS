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
            var reservation = db.Reservation.Include("Customer").Include("ReservationStatus");
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
            if (ModelState.IsValid)
            {
                decimal __total = new decimal();
                decimal __totalrack = new decimal();
                foreach (var item in reservation.Rooms)
                {

                    if (item.IdRoom != 0)
                    {
                        var __room = db.Room.SingleOrDefault(u => u.Id.Equals(item.IdRoom));
                        var __maxcapacity = __room.RoomOcupation.OrderByDescending(model => model.Capacity).First().Capacity;



                        if (item.Adultos <= __maxcapacity && item.Infantes <= __maxcapacity)
                        {
                            IEnumerable<DateTime> __tripdays = EachDay(reservation.Trip.Arrival, reservation.Trip.Departure);
                            __tripdays = __tripdays.Take(__tripdays.Count() - 1);
                            reservation.Trip.Adults = reservation.Trip.Adults + item.Adultos;
                            reservation.Trip.Childrens = reservation.Trip.Childrens + item.Infantes;

                            foreach (var __day in __tripdays)
                            {
                                var __roomfare = db.RoomOcupation.Where(u => u.DateStart <= __day && u.DateEnd >= __day && u.Capacity.Equals(item.Adultos) && u.Active.Equals(true)).FirstOrDefault();
                                var __roomfarechild = db.RoomOcupation.Where(u => u.DateStart <= __day && u.DateEnd >= __day && u.Name.ToLower().Equals("niño")).FirstOrDefault();

                                if (__roomfare != null)
                                {
                                    decimal __discount = new decimal();
                                    if (reservation.Trip.Discount > 0)
                                        __discount = reservation.Trip.Discount;
                                    else if (__tripdays.Count() <= 3)
                                        __discount = __roomfare.Discount1;
                                    else if (__tripdays.Count() >= 4 && __tripdays.Count() < 7)
                                        __discount = __roomfare.Discount2;
                                    else
                                        __discount = __roomfare.Discount3;

                                    __discount = __discount / 100;
                                    decimal __admon = (reservation.Trip.PercentAdmin.Equals(0) ? __roomfare.PercentAdmin : reservation.Trip.PercentAdmin);
                                    __admon = __admon / 100;

                                    //descuento
                                    decimal __facturabruta = __roomfare.PriceRack - (__roomfare.PriceRack * __discount);
                                    //gastos admon
                                    __facturabruta = __facturabruta + (__facturabruta * __admon);
                                    //adultos
                                    __facturabruta = __facturabruta * item.Adultos;
                                    //rack
                                    __totalrack = (__roomfare.Price * item.Adultos) + __totalrack;

                                    __total = __total + __facturabruta;


                                    if (item.Infantes > 0)
                                    {
                                        if (__roomfarechild != null)
                                        {
                                            if (reservation.Trip.Discount > 0)
                                                __discount = reservation.Trip.Discount;
                                            else if (__tripdays.Count() <= 2)
                                                __discount = __roomfarechild.Discount1;
                                            else if (__tripdays.Count() <= 6)
                                                __discount = __roomfarechild.Discount2;
                                            else
                                                __discount = __roomfarechild.Discount3;

                                            __discount = __discount / 100;
                                            __admon = (reservation.Trip.PercentAdmin > 0 ? reservation.Trip.PercentAdmin : __roomfarechild.PercentAdmin) / 100;

                                            //descuento
                                            __facturabruta = __roomfarechild.PriceRack - (__roomfarechild.PriceRack * __discount);
                                            //gastos admon
                                            __facturabruta = __facturabruta + (__facturabruta * __admon);
                                            //adultos
                                            __facturabruta = __facturabruta * item.Infantes;
                                            //rack
                                            __totalrack = (__roomfare.Price * item.Infantes) + __totalrack;

                                            __total = __total + __facturabruta;
                                        }
                                        else
                                        {
                                            ViewBag.Customer = db.Customer.ToList();
                                            ViewBag.ReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
                                            ViewBag.Error = "No hay tarifas cargadas para niños en una de las habitaciones selecionadas.";
                                            return View(reservation);
                                        }
                                    }

                                    reservation.Trip.Discount = Convert.ToInt32(__discount * 100);
                                    reservation.Trip.PercentAdmin = __roomfare.PercentAdmin;
                                    reservation.Trip.PercentAgent = __roomfare.PercentAgent;

                                    reservation.Trip.RoomOcupation.Add(__roomfare);


                                }
                                else
                                {
                                    ViewBag.Customer = db.Customer.ToList();
                                    ViewBag.ReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
                                    ViewBag.Error = "No hay tarifas cargadas para una de las habitaciones selecionada.";
                                    return View(reservation);
                                }
                            }

                        }
                        else
                        {
                            ViewBag.Customer = db.Customer.ToList();
                            ViewBag.ReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
                            ViewBag.Error = "El numero de adultos ó niños excede la capacidad de alguna de las habitaciones.";
                            return View(reservation);
                        }

                    }

                }

                reservation.Trip.PriceRack = __totalrack;
                reservation.Trip.Price = __total;
                reservation.Trip.ReservationDate = DateTime.Now;
                db.Reservation.AddObject(reservation.Trip);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.Error = "Alguno de los datos suministrados está errado.";
            ViewBag.Customer = db.Customer.ToList();
            ViewBag.ReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
            return View(reservation);
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
            if (ModelState.IsValid)
            {
                db.Reservation.Attach(reservation);
                db.ObjectStateManager.ChangeObjectState(reservation, EntityState.Modified);
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