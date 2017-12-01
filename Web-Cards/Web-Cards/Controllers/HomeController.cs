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
            bool resP1 = test.Player1LayCard(true);
            bool resP2 = test.Player2LayCard(true);
            if (resP1 || resP2)
            {
                ViewBag.GameEnd = true;
                ViewBag.Winner = resP1 ? "Player 1" : "Player 2";
                return View("WarEnd");
            }
            else {
                return View(test);
            }
        }

        public string WarDoBattle()
        {
            //TODO, UPDATE VIEW WITH WINNER INFO
            int winner = test.Battle();
            if (winner == 0)
            {
                test.War();
                return "War is now initiated";
            }
            else if (winner == -1)
            {
                return "Game over. Press Reset to play again";
            }
            return $"Player {winner} won this round";
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