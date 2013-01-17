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
    public class RoomRateController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        //
        // GET: /RoomRate/
        [Authorize]
        public ViewResult Index(int id)
        {
            var roomocupation = db.RoomOcupation.Include("Room").Where(u => u.IdRoom.Equals(id));
            ViewBag.Room = db.Room.SingleOrDefault(u => u.Id.Equals(id));
            return View(roomocupation.ToList());
        }
        [Authorize]
        public ViewResult Index2(int id)
        {
            var roomocupation = db.RoomOcupation.Include("Room").Where(u => u.IdRoom.Equals(id));
            ViewBag.Room = db.Room.SingleOrDefault(u => u.Id.Equals(id));
            return View(roomocupation.ToList());
        }

        //
        // GET: /RoomRate/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            return View(roomocupation);
        }

        //
        // GET: /RoomRate/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name");
            ViewBag.Room = db.Room.SingleOrDefault(u => u.Id.Equals(id));
            return View();
        }

        //
        // POST: /RoomRate/Create

        [HttpPost]
        public ActionResult Create(RoomOcupation roomocupation)
        {

            ViewBag.Room = db.Room.SingleOrDefault(u => u.Id.Equals(roomocupation.IdRoom));
            try
            {
                Session.Add("DateStart", roomocupation.DateStart.ToShortDateString());
                Session.Add("DateEnd", roomocupation.DateEnd.ToShortDateString());

                int __capacity = new int();

                switch (roomocupation.Name)
                {
                    case "Sencilla":
                        __capacity = 1;
                        break;
                    case "Doble":
                        __capacity = 2;
                        break;
                    case "Triple":
                        __capacity = 3;
                        break;
                    case "Cuádruple":
                        __capacity = 4;
                        break;
                    case "Quíntuple":
                        __capacity = 5;
                        break;
                    case "Séxtuple":
                        __capacity = 6;
                        break;
                    default:
                        __capacity = 1;
                        break;
                }

                roomocupation.Capacity = __capacity;
                roomocupation.Active = true;

                db.RoomOcupation.AddObject(roomocupation);
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = roomocupation.IdRoom });

            }
            catch (Exception)
            {

                throw;
            }


            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name", roomocupation.IdRoom);
            return View(roomocupation);
        }

        //
        // GET: /RoomRate/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name", roomocupation.IdRoom);
            return View(roomocupation);
        }

        //
        // POST: /RoomRate/Edit/5

        [HttpPost]
        public ActionResult Edit(RoomOcupation roomocupation)
        {
            if (ModelState.IsValid)
            {
                db.RoomOcupation.Attach(roomocupation);
                db.ObjectStateManager.ChangeObjectState(roomocupation, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = roomocupation.IdRoom });
            }
            ViewBag.IdRoom = new SelectList(db.Room, "Id", "Name", roomocupation.IdRoom);
            return View(roomocupation);
        }

        //
        // GET: /RoomRate/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            return View(roomocupation);
        }

        //
        // POST: /RoomRate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomOcupation roomocupation = db.RoomOcupation.Single(r => r.Id == id);
            db.RoomOcupation.DeleteObject(roomocupation);
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = roomocupation.IdRoom });
        }
        [Authorize]
        public ActionResult Batch(int Id)
        {
            ViewBag.IdRoom = Id;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Batch(BatchParams input)
        {
            var __roomrates = db.RoomOcupation.Where(u => u.IdRoom.Equals(input.IdRoom));

            foreach (var item in __roomrates)
            {
                if (input.descuesto1.HasValue)
                    item.Discount1 = input.descuesto1.Value;
                if (input.descuesto1.HasValue)
                    item.Discount2 = input.descuesto2.Value;
                if (input.descuesto1.HasValue)
                    item.Discount3 = input.descuesto3.Value;
                if (input.percentadmon.HasValue)
                    item.PercentAdmin = input.percentadmon.Value;
                if (input.percentagent.HasValue)
                    item.PercentAgent = input.percentagent.Value;
            }

            db.SaveChanges();

            return JavaScript("Tarifas actualizadas correctamente.");
        }

        [Authorize]
        public ActionResult GetDataFare(string sidx, string sord, int page, int rows, int id)
        {
            var roomocupation = db.RoomOcupation.Where(u => u.IdRoom.Equals(id)).Select(x => new
            {
                x.Active,
                x.Capacity,
                x.DateEnd,
                x.DateStart,
                x.Discount1,
                x.Discount2,
                x.Discount3,
                x.PercentAdmin,
                x.PercentAgent,
                x.Name,
                x.PriceRack,
                x.Price,
                x.Id
            });

            var result = new
            {
                total = 1,
                page = page,
                records = roomocupation.Count(),
                rows = roomocupation.ToList() // .AsEnumerable() whatever
                    .Select(x => new
                    {
                        id = x.Id,
                        cell = new object[] {
                            x.Name,                            
                            x.Capacity,
                            x.PriceRack,
                            x.Price,
                            x.DateStart.ToShortDateString(),
                            x.DateEnd.ToShortDateString(),                          
                            x.Discount1,
                            x.Discount2,
                            x.Discount3,                            
                            x.PercentAgent,
                            x.PercentAdmin,
                            x.Active,
                            x.DateStart.ToShortDateString() + " - " + x.DateEnd.ToShortDateString()
                        }
                    })
                    .ToArray(),
            };

            return new JsonResult()
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8
            };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    public class BatchParams
    {

        public int IdRoom { get; set; }
        public int? descuesto1 { get; set; }
        public int? descuesto2 { get; set; }
        public int? descuesto3 { get; set; }
        public int? percentadmon { get; set; }
        public int? percentagent { get; set; }


    }
}