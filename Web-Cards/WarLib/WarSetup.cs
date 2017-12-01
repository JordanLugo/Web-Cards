using CardsLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Web_CardsDAL;

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
        /// The number of cards that player 1 has left in their possession.
        /// </summary>
        public int Player1CardsCount { get { return player1Cards.Cards.Count; } }
        /// <summary>
        /// The number of cards that player 2 has left in their possession.
        /// </summary>
        public int Player2CardsCount { get { return player2Cards.Cards.Count; } }
        /// <summary>
        /// The cards that player 1 has layed.
        /// </summary>
        public List<Card> Player1StoredCards { get { return player1StoredCards.Cards; } }
        /// <summary>
        /// The cards that player 2 has layed.
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
        /// <returns>Returns true when player is unable to lay a card.</returns>
        public bool Player1LayCard(bool faceUp)
        {
            bool unableToLay = false;
            unableToLay = player1Cards.Cards.Count == 0;
            if (!unableToLay)
            {
                player1StoredCards.Draw(player1Cards);
                player1StoredCards.Cards.Last().FaceUp = faceUp;
            }
            return unableToLay;
        }
        /// <summary>
        /// Player 2 plays one card
        /// </summary>
        /// <param name="faceUp">If true player 2 will lay a card face up. If false player 2 will lay a card face down.</param>
        /// <returns>Returns true when player is unable to lay a card.</returns>
        public bool Player2LayCard(bool faceUp)
        {
            bool unableToLay = false;
            unableToLay = player2Cards.Cards.Count == 0;
            if (!unableToLay)
            {
                player2StoredCards.Draw(player2Cards);
                player2StoredCards.Cards.Last().FaceUp = faceUp;
            }
            return unableToLay;
        }
        /// <summary>
        /// Compares Player 1's and Player 2's faced up cards and decides winner or loser of that turn, or whether a war state has been achieved. 
        /// </summary>
        /// <returns>Battle returns the player number of the winner, though it returns zero if war is triggered. If their is an error it passed a -1 back.</returns>
        public int Battle()
        {
            int playerWhoWon = -1;

            if (player1StoredCards.Cards.Count != 0 && player2StoredCards.Cards.Count != 0)
            { 
                if (player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt == player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
                {
                    playerWhoWon = 0;
                }
                else
                {
                    if (player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt > player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
                    {
                        CollectWinnings(player1Cards, player2StoredCards, player1StoredCards);
                        playerWhoWon = 1;
                    }
                    else if (player1StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt < player2StoredCards.Cards.Where(card => card.FaceUp).Select(card => card).First().ValueInt)
                    {
                        CollectWinnings(player2Cards, player1StoredCards, player2StoredCards);
                        playerWhoWon = 2;
                    }
                }
            }

            return playerWhoWon;
        }
        private void CollectWinnings(Deck winner, Deck loserStored, Deck winnerStored)
        {
			while (loserStored.Cards.Count != 0)
            {
                winner.Draw(loserStored);
            };
			while(winnerStored.Cards.Count != 0)
            {
                winner.Draw(winnerStored);
            };
            player1Cards.Cards.Where(x => x.FaceUp).ToList().ForEach(x => x.FaceUp = false);
            player2Cards.Cards.Where(x => x.FaceUp).ToList().ForEach(x => x.FaceUp = false);
        }
        /// <summary>
        /// Sets up the field for war
        /// </summary>
        /// <returns>If a player is unable to provide all the cards for to go to war their player number will be returned. If all players are able to provide all the cards to go to war then it will return a -1</returns>
        public int War()
        {
            int playerWhoWon = -1;

            player1StoredCards.Cards.Where(x => x.FaceUp).ToList().ForEach(x => x.FaceUp = false);
            player2StoredCards.Cards.Where(x => x.FaceUp).ToList().ForEach(x => x.FaceUp = false);

            if (Player1LayCard(false))
            {
                if (Player1LayCard(true))
                {
                    playerWhoWon = 1;
                }
            }


            if (Player2LayCard(false))
            {
                if (Player2LayCard(true))
                {
                    playerWhoWon = 2;
                }
            }

            return playerWhoWon;
        }
        /// <summary>
        /// This Method returns a formated dictionary to be used in a save state of a game
        /// </summary>
        /// <returns>A Dictionary(string List<Card>) that can easily saved and loaded back in.></returns>
        public byte[] SaveState()
        {
            Dictionary<string, List<Card>> saveData = new Dictionary<string, List<Card>>();


            saveData.Add("Player1HeldCards", player1Cards.Cards.ToList());
            saveData.Add("Player2HeldCards", player2Cards.Cards.ToList());
            saveData.Add("Player1StoredCards", player1StoredCards.Cards.ToList());
            saveData.Add("Player2StoredCards", player2StoredCards.Cards.ToList());
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(stream, saveData);

            byte[] serializedData = stream.ToArray();
            stream.Close();

            return serializedData;
        }

        public bool LoadState(byte[] data)
        {
            bool validInput = false;

            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            stream.Write(data, 0, data.Length);
            stream.Position = 0;
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
            stream.Close();
            return validInput;
        }

      
    }
}
