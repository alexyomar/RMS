using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Text;

namespace RMS.Controllers
{
    public class HomeController : Controller
    {
        private RegionalEntities db = new RegionalEntities();

        public ActionResult Index()
        {
            return View();
        }



        //public ActionResult Create()
        //{
        //    Membership.CreateUser("alexyomar", "a17302339", "alexyomar@gmail.com");
        //    return View();
        //}

        //public ActionResult Database()
        //{
        //    db.CreateDatabase();
        //    return View();
        //}

        public ActionResult Start()
        {
            return View();
        }

        public ActionResult Trips()
        {
            return View();
        }

        public ActionResult About()
        {
            return View("AboutUs");
        }
        public ActionResult Contact()
        {
            ViewData.Model = db.Hotel.OrderBy(u => u.Name).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string nombre,
                                    string email,
                                    string empresa,
                                    string celular,
                                    string arrive,
                                    string departure,
                                    string destino,
                                    string adultos,
                                    string ninos,
                                    string hotel,
                                    string pasajes,
                                    string[] servicios,
                                    string mensaje)
        {

            MailMessage mailmessage;

            mailmessage = new MailMessage(new MailAddress("promocionesrm@regionalmarketing.com.ve"), new MailAddress(System.Configuration.ConfigurationManager.AppSettings["email"]));
            mailmessage.BodyEncoding = Encoding.Default;
            mailmessage.Subject = "Solicitud de presupuesto Web";
            mailmessage.Body = "<h1>Planilla de solicitud de presupuesto por Web</h1>";
            mailmessage.Body += "<p>Datos de la planilla:</p><br /><br />";
            mailmessage.Body += "<ul>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Nombre del contacto: " + nombre;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Email: " + email;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Empresa: " + empresa;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Celular: " + celular;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Fecha de llegada: " + arrive;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Fecha de salida: " + departure;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Adultos:" + adultos;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Niños:" + ninos;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Hotel: " + hotel;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Incluir Pasajes: " + pasajes;
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Servicios a incluir: ";
            foreach (var item in servicios)
            {
                mailmessage.Body += item + " ";
            }
            mailmessage.Body += "</li>";
            mailmessage.Body += "<li>";
            mailmessage.Body += "Observaciones: " + mensaje;
            mailmessage.Body += "</li>";
            mailmessage.Body += "</ul><br /><br />";
            mailmessage.Body += "Planilla generada el " + DateTime.Now.ToString();
            mailmessage.Priority = MailPriority.High;
            mailmessage.IsBodyHtml = true;

            SmtpClient smtpMail = new SmtpClient();

            smtpMail.Send(mailmessage);

            return View("ContactOk");
        }

    }
}
