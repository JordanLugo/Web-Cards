using System;

namespace CardsLib
{
    /// <summary>
    /// Represents a standard playing card
    /// </summary> 
    [Serializable]
    public class Card
    {
        public Card(string fullName, string suit, string value, int numValue)
        {
            FullName = fullName;
            Suit = suit;
            Value = value;
            ValueInt = numValue;
        }

        /// <summary>
        /// Full display name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The suit of the card, ie: clubs, spades, hearts, diamonds
        /// </summary>
        public string Suit { get; set; }

        /// <summary>
        /// The value of the card, ie: Two, Five, Ten, King, Ace ...
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// The value of the card in points
        /// </summary>
        public int ValueInt { get; set; }

        /// <summary>
        /// Whether the card is face down (false) or face up (true).
        /// </summary>
        public bool FaceUp { get; set; } = false;
    }
}
