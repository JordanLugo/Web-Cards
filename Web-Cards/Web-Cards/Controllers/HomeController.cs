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
        static WarSetup w = new WarSetup();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ExampleGame()
        {
            
            return View(w);
        }

        public ActionResult GetData()
        {
            return View(w);
        }
    }
}