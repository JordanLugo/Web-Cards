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

		static WarSetup WarGame = new WarSetup();
        public ActionResult Index()
        {
            return View(WarGame);
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
            bool resP1 = WarGame.Player1LayCard(true);
            bool resP2 = WarGame.Player2LayCard(true);
            if (resP1 || resP2)
            {
                ViewBag.GameEnd = true;
                ViewBag.Winner = resP1 ? "Player 2" : "Player 1";
                return View("WarEnd");
            }
            else {
                return View(WarGame);
            }
        }

        public string WarDoBattle()
        {
            int winner = WarGame.Battle();
            if (winner == 0)
            {
                WarGame.War();
                return "War is now initiated";
            }
            else if (winner == -1)
            {
                return "Game over. Press Reset to play again";
            }
            return $"{(winner == 1 ? "You" : "The Computer")} won this round";
        }

        public ActionResult War()
        {
            return View(WarGame);
        }

        public string WarReset()
        {
            WarGame.ResetNewGame();
            return "War Reset ok";
        }
    }
}