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
		WarSetup test = new WarSetup();        // GET: Home
        public ActionResult Index()
        {
            return View(test);
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
            return View(test);
        }

        public ActionResult War()
        {
            return View(test);
        }

        public string WarReset()
        {
            test.ResetNewGame();
            return "War Reset ok";
        }
    }
}