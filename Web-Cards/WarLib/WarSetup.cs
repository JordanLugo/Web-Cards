using CardsLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WarLib
{
    public class WarSetup
    {
        private Deck player1StoredCards = new Deck();
        private Deck player2StoredCards = new Deck();
        private Deck player1Cards = new Deck();
        private Deck player2Cards = new Deck();
        private Random rand = new Random();
        /// <summary>
        /// The cards that player 1 has.
        /// </summary>
        public List<Card> Player1Cards { get { return player1Cards.Cards; } }
        /// <summary>
        /// The cards that player 2 has.
        /// </summary>
        public List<Card> Player2Cards { get { return player2Cards.Cards; } }
        /// <summary>
        /// The cards player 1 has active.
        /// </summary>
        public List<Card> Player1StoredCards { get { return player1StoredCards.Cards; } }
        /// <summary>
        /// The cards player 2 has active
        /// </summary>
        public List<Card> Player2StoredCards { get { return player2StoredCards.Cards; } }
        /// <summary>
        /// This sets up and starts a game of War
        /// </summary>
        public WarSetup()
        {
            ResetNewGame();
        }
        /// <summary>
        /// This resets everything used to begin a new game
        /// </summary>
        public void ResetNewGame()
        {
            Deck tableCards = new Deck(true);
            tableCards.Cards.Where(x => x.ValueInt == 1).Select(x => x).ToList().ForEach(x => x.ValueInt = 14);
            for (int shuffle = 0; shuffle < rand.Next(1, 25); shuffle++)
            {
                tableCards.Shuffle();
            }

            player1Cards.ClearDeck();
            player2Cards.ClearDeck();

            for (; tableCards.Cards.Count != 0;)
            {
                player1Cards.Draw(tableCards);
                player2Cards.Draw(tableCards);
            }

            tableCards.ClearDeck();
        }
        /// <summary>
        /// Player 1 plays one card
        /// </summary>
        /// <param name="faceUp">If true player 1 will lay a card face up. If false player 1 will lay a card face down.</param>
        public void Player1LayCard(bool faceUp)
        {
            player1StoredCards.Draw(player1Cards);
            player1StoredCards.Cards.Last().FaceUp = faceUp;
        }
        /// <summary>
        /// Player 2 plays one card
        /// </summary>
        /// <param name="faceUp">If true player 2 will lay a card face up. If false player 2 will lay a card face down.</param>
        public void Player2LayCard(bool faceUp)
        {
            player2StoredCards.Draw(player2Cards);
            player2StoredCards.Cards.Last().FaceUp = faceUp;
        }
        /// <summary>
        /// Compares Player 1's and Player 2's faced up cards and decides winner or loser of that turn, or whether a war state has been achieved. 
        /// </summary>
        /// <returns>Returns true when war occurs, and false if not</returns>
        public bool Battle()
        {
            bool war = false;

            if (player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt == player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
            {
                war = true;
            }
            if (!war)
            {
                if (player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt > player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
                {
                    CollectWinnings(player1Cards, player2StoredCards, player1StoredCards);
                }
                if (player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt < player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
                {
                    CollectWinnings(player2Cards, player1StoredCards, player2StoredCards);
                }
            }

            return war;
        }
        private void CollectWinnings(Deck winner, Deck loserStored, Deck winnerStored)
        {
            for (; loserStored.Cards.Count != 0;)
            {
                winner.Draw(player2StoredCards);
            }
            for (; winnerStored.Cards.Count != 0;)
            {
                winner.Draw(player1StoredCards);
            }
        }
        /// <summary>
        /// Sets up the field for war
        /// </summary>
        public void War()
        {
            player1StoredCards.Cards.Where(x => x.FaceUp).Select(x => x).ToList().ForEach(x => x.FaceUp = false);
            player2StoredCards.Cards.Where(x => x.FaceUp).Select(x => x).ToList().ForEach(x => x.FaceUp = false);

            Player1LayCard(false);
            Player1LayCard(true);

            Player2LayCard(false);
            Player2LayCard(true);
        }
        /// <summary>
        /// This Method returns a formated dictionary to be used in a save state of a game
        /// </summary>
        /// <returns>A Dictionary(string List<Card>) that can easily saved and loaded back in.></returns>
        public byte[] SaveState()
        {
            Dictionary<string, List<Card>> saveData = new Dictionary<string, List<Card>>();

            saveData.Add("Player1HeldCards", Player1Cards.ToList());
            saveData.Add("Player2HeldCards", Player2Cards.ToList());
            saveData.Add("Player1StoredCards", player1StoredCards.Cards.ToList());
            saveData.Add("Player2StoredCards", player2StoredCards.Cards.ToList());

            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(stream, saveData);
            
            return stream.ToArray();
        }

        public bool LoadState(byte[] data)
        {
            bool validInput = false;

            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            stream.Write(data, 0, data.Length);
            Dictionary<string, List<Card>> test = (Dictionary<string, List<Card>>)bf.Deserialize(stream);

            if (test.Keys.Contains("Player1HeldCards") && test.Keys.Contains("Player2HeldCards") && test.Keys.Contains("Player1StoredCards") && test.Keys.Contains("Player2StoredCards"))
            {
                player1Cards.ClearDeck();
                player2Cards.ClearDeck();
                player1StoredCards.ClearDeck();
                player2StoredCards.ClearDeck();

                player1Cards.Cards.AddRange(test.Values.ElementAt(test.Keys.ToList().IndexOf("Player1HeldCards")));
                player2Cards.Cards.AddRange(test.Values.ElementAt(test.Keys.ToList().IndexOf("Player2HeldCards")));
                player1StoredCards.Cards.AddRange(test.Values.ElementAt(test.Keys.ToList().IndexOf("Player1StoredCards")));
                player2StoredCards.Cards.AddRange(test.Values.ElementAt(test.Keys.ToList().IndexOf("Player2StoredCards")));

                validInput = true;
            }

            return validInput;
        }
    }
}
