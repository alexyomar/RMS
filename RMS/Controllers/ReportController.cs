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
        public ActionResult Index(int hotel, int temporada)
        {

            switch (temporada)
            {
                case 1:
                    return RedirectToAction("RoomRateHigh", new { Id = hotel, vista = true });
                    break;
                case 2:
                    return RedirectToAction("RoomRate", new { Id = hotel, vista = true });
                    break;
                case 3:
                    return RedirectToAction("RoomRatePromotion", new { Id = hotel, vista = true });
                    break;
            }
            return View();

        }

        public ActionResult RoomRate(int Id, bool print)
        {
            ViewData.Model = db.Room.Where(u => u.IdHotel.Equals(Id)).ToList();
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
}
