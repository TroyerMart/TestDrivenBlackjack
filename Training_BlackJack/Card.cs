using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training_BlackJack;
using Training_BlackJack.Exceptions;
using Training_BlackJack.Interfaces;

namespace BlackJack
{
    public class Card: ICard
    {
        public readonly static int SuitsCount = Enum.GetValues(typeof(Suit)).Length;
        public readonly static int RanksCount = Enum.GetValues(typeof(Rank)).Length;

        public Suit suit { get; set; }
        public Rank rank { get; set; }
        public bool Visible { get; set; } = true;

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        public Card(Suit suit, Rank rank, bool isVisible):this(suit,rank)
        {
            Visible = isVisible;
        }

        public bool Equals(ICard card)
        {
            if (card == null)
            {
                throw new CardException("Card was null");
            }
            else
            {
                return this.EqualsSuit(card) && this.EqualsRank(card);
            }
        }

        public bool EqualsSuit(ICard card)
        {
            if (card == null)
            {
                return false;
            } else
            {
                return (card.suit == this.suit);
            }            
        }

        public bool EqualsRank(ICard card)
        {
            if (card == null)
            {
                return false;
            }
            else
            {
                return (card.rank == this.rank);
            }
        }

        public bool IsGreaterSuit(ICard card)
        {
            if (card == null)
            {
                return false;
            }
            else
            {
                return (this.suit > card.suit);
            }
        }

        public bool IsGreaterRank(ICard card)
        {
            if (card == null)
            {
                return false;
            }
            else
            {
                return (this.rank > card.rank);
            }
        }

        public ICard Clone()
        {
            return new Card(this.suit, this.rank);
        }

        public override string ToString()
        {
            return rank.ToString() + " of " + suit.ToString();
        }

    }

}
