using CardsLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
            NewGame(numberOfPlayers);
        }
        /// <summary>
        /// This returns the cards that a player has denoted by their player number
        /// </summary>
        /// <param name="playerNumber">This is the player number non-zero based so player 1 = 1, player 2 = 2</param>
        /// <returns>A list of cards of specified player</returns>
        public List<Card> GetPlayersCardsByPlayerNumber(int playerNumber)
        {
            if (playersCards.Count >= numberOfPlayers)
            {
                return playersCards.ElementAt(playerNumber - 1).Cards;
            }
            throw new ArgumentException($"Player {playerNumber} not found.");
        }
        /// <summary>
        /// This is a method that clears everything a rebuilds a new game from scratch.
        /// </summary>
        /// <param name="numberOfPlayers">This is the number of players paying the game.</param>
        public void NewGame(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
            drawDeck.ResetDeck();
            discardPile.ClearDeck();
            dealersCards.ClearDeck();
            playersCards.Clear();

            for (int player = 0; player < numberOfPlayers; player++)
            {
                playersCards.Add(new Deck());
            }

            EditFaceCardValues();

            ReshuffleDeck();
        }
        private void EditFaceCardValues()
        {
            for (int cards = 0; cards < drawDeck.Cards.Count; cards++)
            {
                if (drawDeck.Cards.ElementAt(cards).Value.Equals("Jack") || drawDeck.Cards.ElementAt(cards).Value.Equals("Queen") || drawDeck.Cards.ElementAt(cards).Value.Equals("King"))
                {
                    drawDeck.Cards.ElementAt(cards).ValueInt = 10;
                }
            }
        }
        /// <summary>
        /// This method discards all the cards from everyone and deals out new cards.
        /// </summary>
        public void NewRound()
        {
            for (int player = 0; player < numberOfPlayers; player++)
            {
                while (playersCards.ElementAt(player).Cards.Count != 0)
                {
                    discardPile.Draw(playersCards.ElementAt(player));
                }
            }
            while (dealersCards.Cards.Count != 0)
            {
                discardPile.Draw(dealersCards);
            }
            DealerDealFirstCardSet();
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
        /// <returns>Returns whether or not inputed player exceeded 21, or returns null if invalid player number entered.</returns>
        public bool? CheckPlayerForBust(int playerNumber)
        {
            bool? bust = null;
            int cardValues = CheckValueOfHand(playerNumber);
            if (cardValues != 0)
            {
                bust = cardValues > 21;
            }
            return bust;
        }
        public int CheckValueOfHand(int playerNumber)
        {
            int valueOfCards = 0;
            if (playerNumber <= playersCards.Count)
            {
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
            }
            return valueOfCards;
        }
        /// <summary>
        /// This is the first move that the Dealer has to make
        /// </summary>
        public void DealerFlipsFaceDownCard()
        {
            dealersCards.Cards.Where(card => !card.FaceUp).ToList().ForEach(card => card.FaceUp = true);
        }
        /// <summary>
        /// The checks if the dealer needs to make a move
        /// </summary>
        /// <returns>Returns true if the dealer needs to draw a card</returns>
        public bool CheckIfDealerNeedsToHit()
        {
            return CheckValueOfHand(0) < 17;
        }
        /// <summary>
        /// This saves the game as a byte[].
        /// </summary>
        /// <param name="playerNumber">This is the player number of whos turn it is non-zero based so player 1 = 1, player 2 = 2, dealer = 0.</param>
        /// <returns>Returns a byte[] of the games state serialized</returns>
        public byte[] SaveState(int playerNumber)
        {
            MemoryStream saveMemoryStream = new MemoryStream();
            BinaryFormatter saveBinaryFormatter = new BinaryFormatter();
            Dictionary<string, object> saveData = new Dictionary<string, object>();
            byte[] serializedData;

            saveData.Add("DrawDeck", drawDeck);
            saveData.Add("DiscardPile", discardPile);
            saveData.Add("DealersCards", dealersCards);
            saveData.Add("PlayersCards", playersCards);
            saveData.Add("NumberOfPlayers", numberOfPlayers);
            saveData.Add("CurrentPlayerTurn" , playerNumber);

            saveBinaryFormatter.Serialize(saveMemoryStream, saveData);

            serializedData = saveMemoryStream.ToArray();
            saveMemoryStream.Close();

            return serializedData;
        }
        /// <summary>
        /// This method takes in a byte[] of data and deserializes it in to the current BlackjackSetup, returns the currents players turn.
        /// </summary>
        /// <param name="data">This is the byte[] data that is being deserialized in to the game.</param>
        public int LoadState(byte[] data)
        {
            MemoryStream loadMemoryStream = new MemoryStream();
            BinaryFormatter loadBinaryFormatter = new BinaryFormatter();

            loadMemoryStream.Write(data, 0, data.Length);
            loadMemoryStream.Position = 0;
            Dictionary<string, object> loadData = (Dictionary<string, object>)loadBinaryFormatter.Deserialize(loadMemoryStream);
            loadMemoryStream.Close();

            if (loadData.Keys.Contains("NumberOfPlayers") && loadData.Keys.Contains("PlayersCards") && loadData.Keys.Contains("DealersCards") && loadData.Keys.Contains("DrawDeck") && loadData.Keys.Contains("DrawPile") && loadData.Keys.Contains("DiscardPile") && loadData.Keys.Contains("CurrentPlayerTurn"))
            {
                numberOfPlayers = (int)loadData["NumberOfPlayers"];
                playersCards = loadData["PlayersCards"] as List<Deck>;
                dealersCards = loadData["DealersCards"] as Deck;
                drawDeck = loadData["DrawDeck"] as Deck;
                discardPile = loadData["DiscardPile"] as Deck;
                return (int)loadData["CurrentPlayerTurn"];
            }
            else
            {
                throw new ArgumentException("The byte[] data you provided in incapable to be converted to Blackjack game data.");
            }
        }
        /// <summary>
        /// This is the AI logic for the easy difficulty opponents.
        /// </summary>
        /// <param name="playerNumber">This is the player number non-zero based so player 1 = 1, player 2 = 2.</param>
        /// <returns>Returns whether the AI wants to hit or stand it will return null if playerNumber is invalid range</returns>
        public bool? EasyAI(int playerNumber)
        {
            bool? hitStand = null;
            int valueOfHand = CheckValueOfHand(playerNumber);

            if (playerNumber != 0 && valueOfHand != 0)
            {
                return rand.Next(0, 22 - valueOfHand) == 0;
            }

            return hitStand;
        }
        /// <summary>
        /// This is the AI logic for the Medium difficulty opponents.
        /// </summary>
        /// <param name="playerNumber">This is the player number non-zero based so player 1 = 1, player 2 = 2.</param>
        /// <returns>Returns whether the AI wants to hit or stand it will return null if playerNumber is invalid range</returns>
        public bool? MediumAI(int playerNumber)
        {
            int valueOfHand = CheckValueOfHand(playerNumber), dealersUpCardValue = dealersCards.Cards.Where(card => card.FaceUp).First().ValueInt;

            if (playerNumber != 0 && valueOfHand != 0)
            {
                if (playersCards.ElementAt(playerNumber - 1).Cards.Where(card => card.ValueInt == 1).ToList().Count == 0)
                {
                    switch (dealersUpCardValue)
                    {
                        case 1:                        
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            return valueOfHand >= 17;
                        case 4:
                        case 5:
                        case 6:
                            return valueOfHand >= 12;
                        case 2:
                        case 3:
                            return valueOfHand >= 13;
                    }
                }
                else
                {
                    return CheckValueOfHand(playerNumber) >= 18;
                }
            }

            return null;
        }
        /// <summary>
        /// This is the AI logic for the Hard difficulty opponents.
        /// </summary>
        /// <param name="playerNumber">This is the player number non-zero based so player 1 = 1, player 2 = 2.</param>
        /// <returns>Returns whether the AI wants to hit or stand it will return null if playerNumber is invalid range</returns>
        public bool? HardAI(int playerNumber)
        {
            bool? hitStand = null;
            int valueOfHand = CheckValueOfHand(playerNumber);

            if (playerNumber != 0 && valueOfHand != 0)
            {
                return drawDeck.Cards.First().ValueInt + valueOfHand < 21;
            }

            return hitStand;
        }
    }
}
