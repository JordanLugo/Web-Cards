using WarLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardsLib;
using Blackjack;

namespace Web_Cards.Controllers
{
    public class HomeController : Controller
    {

		static WarSetup WarGame = new WarSetup();
        static BlackjackSetup bjs = new BlackjackSetup(1);
        static bool bjRoundStarted = false;
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
            if (WarGame.Player2CardsCount > 0 && WarGame.Player1CardsCount > 0)
            {
                bool resP1 = WarGame.Player1CardsCount > 0 ? WarGame.Player1LayCard(true) : true;
                bool resP2 = WarGame.Player2CardsCount > 0 ? WarGame.Player2LayCard(true) : true;
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
            else
            {
                ViewBag.GameEnd = true;
                ViewBag.Winner = WarGame.Player1CardsCount > 0 ? "Player 2" : "Player 1";
                return View("WarEnd");
            }
        }

        public string WarDoBattle()
        {
            int winner = -1;
            if (WarGame.Player1CardsCount > 0 && WarGame.Player2CardsCount > 0)
            {
                winner = WarGame.Battle();
            }
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

        public ActionResult BlackJack()
        {
            if (!bjRoundStarted)
            {
                bjs.DealerDealFirstCardSet();
                if (bjs.DealersCards.Sum(x => x.ValueInt) == 21)
                {
                    bjs.DealerFlipsFaceDownCard();
                }
                if (bjs.GetPlayersCardsByPlayerNumber(1).Sum(x => x.ValueInt) == 21)
                {

                }
                bjRoundStarted = true;
            }

            return View(bjs);
        }

        public ActionResult BlackJackGetRound(string type)
        {
            if (type == "hit")
            {
                bjs.HitPlayer(1);
                if (bjs.CheckPlayerForBust(1) ?? false)
                {
                    ViewBag.Notify = "You busted";
                    ViewBag.NotifyClassName = "lose-warning";
                    ViewBag.Reset = true;
                }
            }
            else if(type == "hold")
            {
                bjs.DealerFlipsFaceDownCard();
                while (bjs.CheckIfDealerNeedsToHit())
                {
                    bjs.HitPlayer(0);
                }
                if (bjs.CheckPlayerForBust(0) ?? false)
                {
                    ViewBag.Notify = "The dealer busted, you won!";
                    ViewBag.NotifyClassName = "win-warning";
                    ViewBag.Reset = true;
                }
                else if (bjs.CheckPlayerForBust(1) ?? false)
                {
                    ViewBag.Notify = "You busted";
                    ViewBag.NotifyClassName = "lose-warning";
                    ViewBag.Reset = true;
                }
                else if (21-bjs.CheckValueOfHand(0) < 21-bjs.CheckValueOfHand(1))
                {
                    ViewBag.Notify = "The dealer is wins! You lost";
                    ViewBag.NotifyClassName = "lose-warning";
                    ViewBag.Reset = true;
                }
                else if (21 - bjs.CheckValueOfHand(0) > 21 - bjs.CheckValueOfHand(1))
                {
                    ViewBag.Notify = "You win!";
                    ViewBag.NotifyClassName = "win-warning";
                    ViewBag.Reset = true;
                }
                else if (21 - bjs.CheckValueOfHand(0) == 21 - bjs.CheckValueOfHand(1))
                {
                    ViewBag.Notify = "Tie, everybody lost";
                    ViewBag.NotifyClassName = "lose-warning";
                    ViewBag.Reset = true;
                }
            }
            else if(type == "reset")
            {
                bjs.NewRound();
                if (bjs.DealersCards.Sum(x => x.ValueInt) == 21)
                {
                    bjs.DealerFlipsFaceDownCard();
                    ViewBag.Notify = "The dealer got a natural!";
                    ViewBag.NotifyClassName = "lose-warning";
                    ViewBag.Reset = true;
                }
            }
            return View(bjs);
        }
    }
}