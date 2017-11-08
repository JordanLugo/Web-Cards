using CardsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarLib
{
    public class WarSetup
    {
        private Deck Player1StoredCards = new Deck();
        private Deck Player2StoredCards = new Deck();
        private Deck player1Cards;
        private Deck player2Cards;
        /// <summary>
        /// The cards that player 1 has.
        /// </summary>
        public List<Card> Player1Cards { get { return player1Cards.Cards; } }
        /// <summary>
        /// The cards that player 2 has.
        /// </summary>
        public List<Card> Player2Cards { get { return player2Cards.Cards; } }
        /// <summary>
        /// This sets up and starts a game of War
        /// </summary>
        public WarSetup()
        {
            Deck TableCards = new Deck(true);
            TableCards.Cards.Where(x => x.ValueInt == 1).Select(x => x).ToList().ForEach(x => x.ValueInt = 14);
            TableCards.Shuffle();

            player1Cards = new Deck();
            player2Cards = new Deck();

            for (; TableCards.Cards.Count != 0;)
            {
                player1Cards.Draw(TableCards);
                player2Cards.Draw(TableCards);
            }
        }
        /// <summary>
        /// Player 1 plays one card
        /// </summary>
        /// <param name="faceUp">If true player 1 will lay a card face up. If false player 1 will lay a card face down.</param>
        public void Player1LayCard(bool faceUp)
        {
            Player1StoredCards.Draw(player1Cards);
            Player1StoredCards.Cards.Last().FaceUp = faceUp;
        }
        /// <summary>
        /// Player 2 plays one card
        /// </summary>
        /// <param name="faceUp">If true player 2 will lay a card face up. If false player 2 will lay a card face down.</param>
        public void Player2LayCard(bool faceUp)
        {
            Player2StoredCards.Draw(player2Cards);
            Player2StoredCards.Cards.Last().FaceUp = faceUp;
        }
        /// <summary>
        /// Compares Player 1's and Player 2's faced up cards and decides winner, loser, or war. 
        /// </summary>
        /// <returns>Returns true when a war occurs, false if not</returns>
        public bool Battle()
        {
            bool war = false;

            if (Player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt == Player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
            {
                war = true;
            }
            if (!war)
            {
                if (Player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt > Player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
                {
                    CollectWinnings(player1Cards, Player2StoredCards, Player1StoredCards);
                }
                if (Player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt < Player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
                {
                    CollectWinnings(player2Cards, Player1StoredCards, Player2StoredCards);
                }
            }

            return war;
        }
        private void CollectWinnings(Deck winner, Deck loserStored, Deck winnerStored)
        {
            for (; loserStored.Cards.Count != 0;)
            {
                winner.Draw(Player2StoredCards);
            }
            for (; winnerStored.Cards.Count != 0;)
            {
                winner.Draw(Player1StoredCards);
            }
        }
        /// <summary>
        /// Sets up the field for war
        /// </summary>
        public void War()
        {
            Player1StoredCards.Cards.Where(x => x.FaceUp).Select(x => x).ToList().ForEach(x => x.FaceUp = false);
            Player2StoredCards.Cards.Where(x => x.FaceUp).Select(x => x).ToList().ForEach(x => x.FaceUp = false);

            Player1LayCard(false);
            Player1LayCard(true);

            Player2LayCard(false);
            Player2LayCard(true);
        }
    }
}
