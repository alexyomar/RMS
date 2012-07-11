using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Regional.Controllers
{
    public class HotelController : Controller
    {
        //
        // GET: /Hotel/
        Helpers __helpers = new Helpers();

        public ActionResult Index()
        {
            IQueryable<Models.Hotel> __hoteles = __helpers.BD.Hotels.AsQueryable();
            ViewData.Model = __hoteles;
            return View();
        }

        public ActionResult Editar(int? IdHotel)
        {
            ViewBag.Estados = __helpers.BD.States.OrderBy(u => u.Name).AsQueryable();
            if (IdHotel != null)
            {
                Models.Hotel __hotel = __helpers.BD.Hotels.Where(u => u.Id.Equals(IdHotel.Value)).SingleOrDefault();
                ViewData.Model = __hotel;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Editar(Models.Hotel __hotel)
        {

            try
            {
                if (__hotel.Id.Equals(0))
                    __helpers.BD.Hotels.InsertOnSubmit(__hotel);
                else
                {
                    Models.Hotel __update = __helpers.BD.Hotels.Where(u => u.Id.Equals(__hotel.Id)).SingleOrDefault();

                    __update.Active = __hotel.Active;
                    __update.Address = __hotel.Address;
                    __update.CanSell = __hotel.CanSell;
                    __update.Contact = __hotel.Contact;
                    __update.Description = __hotel.Description;
                    __update.IdState = __hotel.IdState;
                    __update.Map = __hotel.Map;
                    __update.Name = __hotel.Name;
                    __update.TripAdvisor = __hotel.TripAdvisor;
                    __update.TripAdvisorEng = __hotel.TripAdvisorEng;
                }

                __helpers.BD.SubmitChanges();
                return Json(new { Success = "true" }); ;
            }
            catch (Exception ex)
            {
                return Json(new { Success = "false", Message = ex.Message, Stacktrace = ex.StackTrace }); ;

            }
        }

        [HttpPost]
        public ActionResult Eliminar(int IdHotel)
        {

            try
            {
                __helpers.BD.Hotels.DeleteOnSubmit(__helpers.BD.Hotels.Where(u => u.Id.Equals(IdHotel)).SingleOrDefault());
                __helpers.BD.SubmitChanges();
                return Json(new { Success = "true" }); ;
            }
            catch (Exception ex)
            {
                return Json(new { Success = "false", Message = ex.Message, Stacktrace = ex.StackTrace }); ;

            }
        }

    }
}

