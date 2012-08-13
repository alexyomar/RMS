using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;

namespace RMS.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        private RegionalEntities db = new RegionalEntities();

        public ActionResult Index()
        {
            ViewData.Model = db.Hotel.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Index(int fare)
        {

            return RedirectToAction("RoomRate", new { Id = fare, vista = true });

        }

        public ActionResult RoomRate(int Id, bool print)
        {
            var __example = db.RoomOcupation.SingleOrDefault(u => u.Id.Equals(Id));
            ViewData.Model = db.RoomOcupation.Where(u => u.DateStart.Equals(__example.DateStart) && u.DateEnd.Equals(__example.DateEnd)).ToList();
            ViewBag.Export = !print;
            return View();
        }

        public ActionResult RoomRateHigh(int Id, bool print)
        {
            ViewData.Model = db.Room.Where(u => u.IdHotel.Equals(Id)).ToList();
            ViewBag.Export = !print;
            return View();
        }

        public ActionResult RoomRatePromotion(int Id, bool print)
        {
            ViewData.Model = db.Room.Where(u => u.IdHotel.Equals(Id)).ToList();

            ViewBag.Export = !print;

            return View();
        }

        public JsonResult GetRoom(int id)
        {
            var __habitaciones = db.Room.Where(model => model.IdHotel.Equals(id)).OrderBy(model => model.Name);
            return Json(new SelectList(__habitaciones, "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoomRate(int id)
        {
            var __rates = from items in db.RoomOcupation
                          where items.IdRoom.Equals(id)
                          group items by new { items.DateStart, items.DateEnd }
                              into g
                              select g;




            List<fares> __fares = new List<fares>();

            foreach (var item in __rates)
            {
                __fares.Add(new fares() { Id = item.First().Id, Date = item.First().DateStart.ToShortDateString() + " - " + item.First().DateEnd.ToShortDateString() });
            }

            return Json(new SelectList(__fares, "Id", "Date"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintableRoom(int hotel, int temporada)
        {
            ViewBag.Hotel = hotel;
            ViewBag.Temporada = temporada;

            ViewData.Model = db.Room.Where(u => u.IdHotel.Equals(hotel)).ToList();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=tarifas.xls");

            return View();


        }

    }

    public class fares
    {

        public int Id { get; set; }
        public String Date { get; set; }

    }
}
