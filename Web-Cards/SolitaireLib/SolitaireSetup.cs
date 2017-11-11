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
        private Deck drawPile = new Deck();
        private Deck viewPile = new Deck();
        private Deck[] piles = new Deck[7];
        private Deck[] foundationPiles = new Deck[4];
        private Random rand = new Random();
        public Deck[] Piles { get; }
        /// <summary>
        /// This sets up and starts a game of War
        /// </summary>
        public SolitaireSetup()
        {
            foundationPiles.ToList().ForEach(deck => deck = new Deck());
            piles.ToList().ForEach(deck => deck = new Deck());

            ResetNewGame();
        }
        /// <summary>
        /// This resets everything used to begin a new game
        /// </summary>
        public void ResetNewGame(bool test = false)
        {
            Deck tableCards = new Deck(true);
            for (int shuffle = 0; shuffle < rand.Next(1, 25); shuffle++)
            {
                tableCards.Shuffle();
            }

            foundationPiles.ToList().ForEach(deck => deck.ClearDeck());
            piles.ToList().ForEach(deck => deck.ClearDeck());
            drawPile.ClearDeck();
            viewPile.ClearDeck();

            for (int finishedPiles = 0; finishedPiles < 7; finishedPiles++)
            {
                for (int cards = 6; cards >= 0 + finishedPiles; cards--)
                {
                    if (cards == finishedPiles)
                    {
                        piles[cards].Draw(tableCards);
                        piles[cards].Cards.Last().FaceUp = true;
                    }
                    else
                    {
                        piles[cards].Draw(tableCards);
                    }
                }
            }

            for (; tableCards.Cards.Count == 0;)
            {
                drawPile.Draw(tableCards);
            }

            tableCards.ClearDeck();
        }
        public void CheckForFlip()
        {
            piles.ToList().Where(deck => deck.Cards.Where(cards => cards.FaceUp).Count() == 0).Select(deck => deck).ToList().ForEach(pile => pile.Cards.Last().FaceUp = true);
        }
        public bool SwitchCardsPile(int cardsToMove, Deck moveFromPile, Deck moveToPile)
        {
            bool validMove = false;

            if (CheckIfMoveValid(cardsToMove, moveFromPile, moveToPile))
            {
                validMove = true;
                for (int cards = 0; cards < cardsToMove; cards++)
                {
                    moveToPile.Cards.Add(moveFromPile.Cards.ElementAt(moveFromPile.Cards.Count - (cardsToMove - cards)));
                    moveFromPile.Cards.RemoveAt(moveFromPile.Cards.Count - (cardsToMove - cards));
                }
            }

            return validMove;
        }
        private bool CheckIfMoveValid(int cardsToMove, Deck moveFromPile, Deck moveToPile)
        {
            bool validMove = cardsToMove <= moveFromPile.Cards.Count();

            if (validMove)
            {
                switch (moveToPile.Cards.Last().Suit)
                case ""
            }

            return validMove;
        }
    }
}
