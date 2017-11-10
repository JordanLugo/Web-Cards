using System;
using System.Collections.Generic;
using System.Text;

namespace CardsLib
{
    /// <summary>
    /// Represents a collection of Cards
    /// Could be an actual deck or a hand of cards
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// The collection of Card objects
        /// </summary>
        public List<Card> Cards { get; private set; } = new List<Card>();

        /// <summary>
        /// Creates a new Deck object
        /// </summary>
        /// <param name="isWholeDeck">If true, this will auto fill this Deck instance with the default 52 cards</param>
        public Deck(bool isWholeDeck = false)
        {
            if (isWholeDeck)
            {
                Cards.Add( new Card("Ace of Spades",     "Spades",   "Ace",      1      ));
                Cards.Add( new Card("Two of Spades",     "Spades",   "Two",      2      ));
                Cards.Add( new Card("Three of Spades",   "Spades",   "Three",    3      ));
                Cards.Add( new Card("Four of Spades",    "Spades",   "Four",     4      ));
                Cards.Add( new Card("Five of Spades",    "Spades",   "Five",     5      ));
                Cards.Add( new Card("Six of Spades",     "Spades",   "Six",      6      ));
                Cards.Add( new Card("Seven of Spades",   "Spades",   "Seven",    7      ));
                Cards.Add( new Card("Eight of Spades",   "Spades",   "Eight",    8      ));
                Cards.Add( new Card("Nine of Spades",    "Spades",   "Nine",     9      ));
                Cards.Add( new Card("Ten of Spades",     "Spades",   "Ten",      10     ));
                Cards.Add( new Card("Jack of Spades",    "Spades",   "Jack",     11     ));
                Cards.Add( new Card("Queen of Spades",   "Spades",   "Queen",    12     ));
                Cards.Add( new Card("King of Spades",    "Spades",   "King",     13     ));
                           
                Cards.Add( new Card("Ace of Diamonds",   "Diamonds", "Ace",      1      ));
                Cards.Add( new Card("Two of Diamonds",   "Diamonds", "Two",      2      ));
                Cards.Add( new Card("Three of Diamonds", "Diamonds", "Three",    3      ));
                Cards.Add( new Card("Four of Diamonds",  "Diamonds", "Four",     4      ));
                Cards.Add( new Card("Five of Diamonds",  "Diamonds", "Five",     5      ));
                Cards.Add( new Card("Six of Diamonds",   "Diamonds", "Six",      6      ));
                Cards.Add( new Card("Seven of Diamonds", "Diamonds", "Seven",    7      ));
                Cards.Add( new Card("Eight of Diamonds", "Diamonds", "Eight",    8      ));
                Cards.Add( new Card("Nine of Diamonds",  "Diamonds", "Nine",     9      ));
                Cards.Add( new Card("Ten of Diamonds",   "Diamonds", "Ten",      10     ));
                Cards.Add( new Card("Jack of Diamonds",  "Diamonds", "Jack",     11     ));
                Cards.Add( new Card("Queen of Diamonds", "Diamonds", "Queen",    12     ));
                Cards.Add( new Card("King of Diamonds",  "Diamonds", "King",     13     ));
                                                                                         
                Cards.Add( new Card("Ace of Clubs",      "Clubs",    "Ace",      1      ));
                Cards.Add( new Card("Two of Clubs",      "Clubs",    "Two",      2      ));
                Cards.Add( new Card("Three of Clubs",    "Clubs",    "Three",    3      ));
                Cards.Add( new Card("Four of Clubs",     "Clubs",    "Four",     4      ));
                Cards.Add( new Card("Five of Clubs",     "Clubs",    "Five",     5      ));
                Cards.Add( new Card("Six of Clubs",      "Clubs",    "Six",      6      ));
                Cards.Add( new Card("Seven of Clubs",    "Clubs",    "Seven",    7      ));
                Cards.Add( new Card("Eight of Clubs",    "Clubs",    "Eight",    8      ));
                Cards.Add( new Card("Nine of Clubs",     "Clubs",    "Nine",     9      ));
                Cards.Add( new Card("Ten of Clubs",      "Clubs",    "Ten",      10     ));
                Cards.Add( new Card("Jack of Clubs",     "Clubs",    "Jack",     11     ));
                Cards.Add( new Card("Queen of Clubs",    "Clubs",    "Queen",    12     ));
                Cards.Add( new Card("King of Clubs",     "Clubs",    "King",     13     ));
                           
                Cards.Add( new Card("Ace of Hearts",     "Hearts",   "Ace",      1      ));
                Cards.Add( new Card("Two of Hearts",     "Hearts",   "Two",      2      ));
                Cards.Add( new Card("Three of Hearts",   "Hearts",   "Three",    3      ));
                Cards.Add( new Card("Four of Hearts",    "Hearts",   "Four",     4      ));
                Cards.Add( new Card("Five of Hearts",    "Hearts",   "Five",     5      ));
                Cards.Add( new Card("Six of Hearts",     "Hearts",   "Six",      6      ));
                Cards.Add( new Card("Seven of Hearts",   "Hearts",   "Seven",    7      ));
                Cards.Add( new Card("Eight of Hearts",   "Hearts",   "Eight",    8      ));
                Cards.Add( new Card("Nine of Hearts",    "Hearts",   "Nine",     9      ));
                Cards.Add( new Card("Ten of Hearts",     "Hearts",   "Ten",      10     ));
                Cards.Add( new Card("Jack of Hearts",    "Hearts",   "Jack",     11     ));
                Cards.Add( new Card("Queen of Hearts",   "Hearts",   "Queen",    12     ));
                Cards.Add( new Card("King of Hearts",    "Hearts",   "King",     13     ));
            }
        }

        /// <summary>
        /// Shuffles the Cards list
        /// </summary>
        public void Shuffle()
        {
            Random r = new Random();
            List<Card> shuffled = new List<Card>();
            while (Cards.Count > 0)
            {
                int index = r.Next(0, Cards.Count);
                shuffled.Add(Cards[index]);
                Cards.RemoveAt(index);
            }
            Cards = shuffled;
        }

        /// <summary>
        /// Draws the first card from another deck instance
        /// </summary>
        /// <param name="from">The deck to draw from</param>
        public void Draw(Deck from)
        {
            Cards.Add(from.Cards[0]);
            from.Cards.RemoveAt(0);
        }
    }
}
