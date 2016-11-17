using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack.Interfaces
{
    public enum Suit
    {
        // The order is lowest to highest
        Clubs,
        Diamonds,
        Hearts,
        Spades,
    }

    public enum Rank
    {
        // The order is lowest to highest
        Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
    }

    public interface ICard
    {
        Suit suit { get; set; }
        Rank rank { get; set; }
        bool Visible { get; set; }

        bool Equals(ICard card);

        bool EqualsSuit(ICard card);

        bool EqualsRank(ICard card);

        bool IsGreaterSuit(ICard card);

        bool IsGreaterRank(ICard card);

        ICard Clone();

        string ToString();

    }
}
