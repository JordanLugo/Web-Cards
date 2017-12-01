using CardsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class BlackjackSetup
    {
        private Deck drawDeck = new Deck();
        private Deck discardPile = new Deck();
        private Deck dealersCards = new Deck();
        private List<Deck> playersCards = new List<Deck>();
        private int numberOfPlayers;
        private Random rand = new Random();
        /// <summary>
        /// Returns the number of players in the game
        /// </summary>
        public int NumberOfPlayer { get { return numberOfPlayers; } }
        /// <summary>
        /// This is a Property that returns a list of cards that the dealer has.
        /// </summary>
        public List<Card> DealersCards { get { return dealersCards.Cards; } }
        /// <summary>
        /// This returns the number of cards left in the draw pile.
        /// </summary>
        public int DrawDeckCount { get { return drawDeck.Cards.Count; } }
        public BlackjackSetup(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
            ResetNewGame();
        }
        /// <summary>
        /// This returns the cards that a player has denoted by their player number
        /// </summary>
        /// <param name="playerNumber">This is the player number non-zero based so player 1 = 1, player 2 = 2</param>
        /// <returns>A list of cards of specified player</returns>
        public List<Card> GetPlayersCardsByPlayerNumber(int playerNumber)
        {
            return playersCards.ElementAt(playerNumber - 1).Cards;
        }
        /// <summary>
        /// This is a method that clears everything a rebuilds a new game from scratch.
        /// </summary>
        public void ResetNewGame()
        {
            drawDeck.ResetDeck();
            discardPile.ClearDeck();

            drawDeck.Cards.Where(cards => cards.Suit.Equals("Jack", StringComparison.CurrentCultureIgnoreCase)).ToList().ForEach(card => card.ValueInt = 10);
            drawDeck.Cards.Where(cards => cards.Suit.Equals("Queen", StringComparison.CurrentCultureIgnoreCase)).ToList().ForEach(card => card.ValueInt = 10);
            drawDeck.Cards.Where(cards => cards.Suit.Equals("King", StringComparison.CurrentCultureIgnoreCase)).ToList().ForEach(card => card.ValueInt = 10);

            ReshuffleDeck();
        }
        /// <summary>
        /// This method is used to deal out a starting round of cards to the player and itself.
        /// </summary>
        public void DealerDealFirstCardSet()
        {
            for (int dealRound = 0; dealRound < 2; dealRound++)
            {
                for (int player = 0; player < numberOfPlayers; player++)
                {
                    DealerDealsACard(playersCards.ElementAt(player) , true);
                }
                DealerDealsACard(dealersCards, dealRound == 0);
            }
        }
        private void DealerDealsACard(Deck personsCards, bool faceUp)
        {
            if (drawDeck.Cards.Count == 0)
            {
                ReshuffleDeck();
            }

            personsCards.Draw(drawDeck);
            personsCards.Cards.Last().FaceUp = faceUp;
        }
        /// <summary>
        /// This method reshuffles the draw pile and shuffles in the discard pile into the draw pile.
        /// </summary>
        public void ReshuffleDeck()
        {
            while (discardPile.Cards.Count != 0)
            {
                drawDeck.Draw(discardPile);
            }

            drawDeck.Cards.Where(card => card.FaceUp).ToList().ForEach(card => card.FaceUp = false);

            for (int shuffle = 0; shuffle < rand.Next(1, 25); shuffle++)
            {
                drawDeck.Shuffle();
            }
        }
        /// <summary>
        /// Causes the dealer to deal a card to a specified player.
        /// </summary>
        /// <param name="playerNumber">This is the player number non-zero based so player 1 = 1, player 2 = 2, dealer = 0.</param>
        public void HitPlayer(int playerNumber)
        {
            if (playerNumber != 0)
            {
                DealerDealsACard(playersCards.ElementAt(playerNumber - 1), true);
            }
            else
            {
                DealerDealsACard(dealersCards, true);
            }
        }
        /// <summary>
        /// Checks a player for if they bust or not.
        /// </summary>
        /// <param name="playerNumber">This is the player number non-zero based so player 1 = 1, player 2 = 2, dealer = 0.</param>
        /// <returns>Returns whether or not inputed player exceeded 21.</returns>
        public bool CheckPlayerForBust(int playerNumber)
        {
            int valueOfCards = 0;
            Deck player = playerNumber != 0 ? playersCards.ElementAt(playerNumber - 1) : dealersCards;

            foreach (Card card in player.Cards)
            {
                if (card.ValueInt != 1)
                {
                    valueOfCards += card.ValueInt;
                }
            }
            foreach (Card card in player.Cards)
            {
                if (card.ValueInt == 1)
                {
                    if (valueOfCards + 11 > 21)
                    {
                        valueOfCards += 1;
                    }
                    else
                    {
                        valueOfCards += 11;
                    }
                }
            }

            return valueOfCards > 21;
        }
        public void DealerFlipsFaceDownCard()
        {
            dealersCards.Cards.Where(card => !card.FaceUp).ToList().ForEach(card => card.FaceUp = true);
        }
    }
}
