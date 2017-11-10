using CardsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireLib
{
    public class SolitaireSetup
    {
        /// <summary>
        /// This sets up and starts a game of War
        /// </summary>
        public SolitaireSetup()
        {
            ResetNewGame();
        }
        /// <summary>
        /// This resets everything used to begin a new game
        /// </summary>
        public void ResetNewGame()
        {
            Deck TableCards = new Deck(true);
            TableCards.Shuffle();

            TableCards.ClearDeck();
        }
    }
}
