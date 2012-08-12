using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Xml;

namespace RMS.Controllers
{
    public class ReservationController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        ////
        //// GET: /Reservation/
        //[Authorize]
        //public ViewResult Index()
        //{

        //    var Reservation = db.Reservation.Include("Customer").Include("ReservationStatus");
        //    return View(Reservation.OrderByDescending(u => u.ReservationDate).ToList());
        //}

        ////
        //// GET: /Reservation/Details/5
        //[Authorize]
        //public ViewResult Details(int id)
        //{
        //    Reservation reservation = db.Reservation.Single(r => r.Id == id);
        //    return View(reservation);
        //}

        ////
        //// GET: /Reservation/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    ViewBag.Customer = db.Customer.OrderBy(u => u.Name);
        //    ViewBag.ReservationStatus = new SelectList(db.ReservationStatus, "Id", "Name");
        //    ViewBag.Hotels = db.Hotel.OrderBy(u => u.Name);
        //    return View();
        //}

        ////
        //// POST: /Reservation/Create
        //[Authorize]
        //[HttpPost]
        //public ActionResult Create(ReservationModel reservation)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        foreach (var item in reservation.Rooms)
        //        {
        //            if (item.IdRoom != 0)
        //            {
        //                Room __room = db.Room.SingleOrDefault(u => u.Id.Equals(item.IdRoom));
        //                Room __roominfante = new Room();
        //                if (item.Infantes > 0)
        //                    __roominfante = db.Room.SingleOrDefault(u => u.Name.ToLower().Equals("infante") && u.IdHotel.Equals(__room.IdHotel));

        //                if (item.Adultos <= __room.Capacity)
        //                {
        //                    decimal __priceroom = new decimal();

        //                    reservation.Trip.Room.Add(__room);
        //                    reservation.Trip.Adults = reservation.Trip.Adults + item.Adultos;
        //                    reservation.Trip.Childrens = reservation.Trip.Childrens + item.Infantes;

        //                    IEnumerable<DateTime> __tripdays = EachDay(reservation.Trip.Arrival, reservation.Trip.Departure);
        //                    __tripdays = __tripdays.Take(__tripdays.Count() - 1);

        //                    //Obtenemos los precios por dia
        //                    foreach (DateTime __day in __tripdays)
        //                    {

        //                        var __temporada = __room.Hotel.Period.Where(u => __day.IsBetween(new DateTime(DateTime.Now.Year, u.BeginMonth, u.BeginDay), new DateTime(DateTime.Now.Year, u.EndMonth, u.EndDay)));
        //                        var __promo = __room.Promotion.Where(u => __day.IsBetween(u.DateStart, u.DateEnd) && u.Active.Equals(true));

        //                        if (__temporada.Count() > 0)
        //                        {
        //                            if (__promo.Count() > 0)
        //                                if (__promo.FirstOrDefault().MinAdults >= item.Adultos && __promo.FirstOrDefault().MinDays >= __tripdays.Count())
        //                                    __priceroom = (__promo.FirstOrDefault().HighSeasonPrice * item.Adultos) + reservation.Trip.Price;
        //                                else
        //                                    __priceroom = (__room.HighSeasonPrice * item.Adultos) + reservation.Trip.Price;
        //                            else
        //                                __priceroom = (__room.HighSeasonPrice * item.Adultos) + reservation.Trip.Price;
        //                            if (item.Infantes > 0)
        //                                __priceroom = (__roominfante.HighSeasonPrice * item.Infantes) + reservation.Trip.Price;
        //                        }
        //                        else
        //                        {
        //                            if (__promo.Count() > 0)
        //                                __priceroom = (__promo.FirstOrDefault().LowSeasonPrice * item.Adultos) + reservation.Trip.Price;
        //                            else
        //                                __priceroom = (__room.LowSeasonPrice * item.Adultos) + reservation.Trip.Price;

        //                            if (item.Infantes > 0)
        //                                __priceroom = (__roominfante.LowSeasonPrice * item.Infantes) + reservation.Trip.Price;
        //                        }
        //                    }

        //                    //Descuentos por dias

        //                    if (__tripdays.Count() >= 7)
        //                        __priceroom = __priceroom - (__priceroom * (__room.Discount3.HasValue ? __room.Discount3.Value : 200) / 100);
        //                    else
        //                        if (__tripdays.Count() <= 7 && __tripdays.Count() >= 5)
        //                            __priceroom = __priceroom - (__priceroom * (__room.Discount3.HasValue ? __room.Discount2.Value : 200) / 100);
        //                        else
        //                            if (__tripdays.Count() <= 5 && __tripdays.Count() >= 3)
        //                                __priceroom = __priceroom - (__priceroom * (__room.Discount3.HasValue ? __room.Discount1.Value : 200) / 100);

        //                    // Precio Final
        //                    reservation.Trip.Price = __priceroom + reservation.Trip.Price;

        //                }
        //                else
        //                {
        //                    ViewBag.IdCustomer = new SelectList(db.Customer, "Id", "Name", reservation.Trip.IdCustomer);
        //                    ViewBag.IdReservationtatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.Trip.IdReservationStatus);
        //                    ViewBag.Error = "El numero de Adultos excede la capacidad de alguna de las habitaciones.";
        //                    return View(reservation);
        //                }



        //            }
        //        }

        //        reservation.Trip.ReservationDate = DateTime.Now;
        //        db.Reservation.AddObject(reservation.Trip);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.IdCustomer = new SelectList(db.Customer, "Id", "Name", reservation.Trip.IdCustomer);
        //    ViewBag.IdReservationtatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.Trip.IdReservationStatus);
        //    return View(reservation);
        //}
        //[Authorize]
        //public ActionResult PartialHabitaciones(int IdHotel)
        //{
        //    ViewBag.Room = db.Room.Where(u => u.IdHotel.Equals(IdHotel) && !u.Name.ToLower().Contains("infante") && u.Active.Equals(true));
        //    if (db.Room.Where(u => u.Name.ToLower().Contains("infante")).Count() > 0)
        //        ViewBag.ShowInfantes = true;
        //    else
        //        ViewBag.ShowInfantes = false;

        //    return PartialView();

        //}

        ////
        //// GET: /Reservation/Edit/5
        //[Authorize]
        //public ActionResult Edit(int id)
        //{
        //    Reservation reservation = db.Reservation.Single(r => r.Id == id);
        //    ViewBag.IdCustomer = new SelectList(db.Customer, "Id", "Name", reservation.IdCustomer);
        //    ViewBag.IdReservationtatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.IdReservationStatus);
        //    return View(reservation);
        //}

        ////
        //// POST: /Reservation/Edit/5
        //[Authorize]
        //[HttpPost]
        //public ActionResult Edit(Reservation reservation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Reservation.Attach(reservation);
        //        db.ObjectStateManager.ChangeObjectState(reservation, EntityState.Modified);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.IdCustomer = new SelectList(db.Customer, "Id", "Name", reservation.IdCustomer);
        //    ViewBag.IdReservationtatus = new SelectList(db.ReservationStatus, "Id", "Name", reservation.IdReservationStatus);
        //    return View(reservation);
        //}

        ////
        //// GET: /Reservation/Delete/5
        //[Authorize]
        //public ActionResult Delete(int id)
        //{
        //    Reservation reservation = db.Reservation.Single(r => r.Id == id);
        //    return View(reservation);
        //}

        ////
        //// POST: /Reservation/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Reservation reservation = db.Reservation.Single(r => r.Id == id);
        //    db.Reservation.DeleteObject(reservation);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //[Authorize]
        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
        //[Authorize]
        //public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        //{
        //    for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
        //        yield return day;
        //}
        //[Authorize]
        //public ActionResult PrintableVersion(int id)
        //{
        //    Reservation reservation = db.Reservation.Single(r => r.Id == id);
        //    return View(reservation);
        //}

        //public string RenderRazorViewToString(string viewName, object model)
        //{
        //    ViewData.Model = model;
        //    using (var sw = new StringWriter())
        //    {
        //        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
        //        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}
        //[Authorize]
        //public ActionResult PrintPDF(int id)
        //{
        //    MemoryStream __memoria = new MemoryStream();

        //    // step 1: creation of a document-object
        //    Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);

        //    // step 2:
        //    // we create a writer that listens to the document
        //    // and directs a XML-stream to a file
        //    PdfWriter __writer = PdfWriter.GetInstance(document, __memoria);
        //    __writer.CloseStream = false;

        //    var model = db.Reservation.SingleOrDefault(u => u.Id.Equals(id));

        //    string strB = RenderRazorViewToString("PrintableVersion", model);

        //    document.Open();

        //    // step 3: we parse the document
        //    using (TextReader sReader = new StringReader(strB))
        //    {

        //        List<IElement> list = HTMLWorker.ParseToList(sReader, new StyleSheet());

        //        foreach (IElement elm in list)
        //        {

        //            document.Add(elm);
        //        }

        //    }

        //    document.Close();

        //    __memoria.Flush(); //Always catches me out
        //    __memoria.Position = 0; //Not sure if this is required
        //    return File(__memoria, "application/pdf", "resumen.pdf");
        //}


    }

    public static class MyExtensions
    {
        public static bool IsBetween(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }
    }

}
