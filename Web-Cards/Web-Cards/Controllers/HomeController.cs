using WarLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardsLib;

namespace Web_Cards.Controllers
{
    public class HomeController : Controller
    {
        WarSetup war = new WarSetup();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}