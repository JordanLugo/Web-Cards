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

        public ActionResult WarGetRound()
        {
            return View(w);
        }

        public ActionResult War()
        {
            return View(w);
        }

        public string WarReset()
        {
            w.ResetNewGame();
            return "War Reset ok";
        }
    }
}