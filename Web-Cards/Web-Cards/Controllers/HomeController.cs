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

		static WarSetup test = new WarSetup();
        public ActionResult Index()
        {
            return View(test);        }

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
            test.Player1LayCard(true);
            test.Player2LayCard(true);
            return View(test);
        }

        public string WarDoBattle()
        {
            //TODO, UPDATE VIEW WITH WINNER INFO
            int winner = test.Battle();
            if (winner == 0)
            {
                test.War();
                return "War is go";
            }
            return $"Player {winner} is winner";
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