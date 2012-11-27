﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS.Models;
using System.Web.Security;

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
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string nombre)
        {
            return View("ContactOk");
        }

    }
}
