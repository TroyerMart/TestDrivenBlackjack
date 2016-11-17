using BlackJack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack.Interfaces
{
    public interface IDeck
    {
        int Size();
        ICard DealCard(int nthCard, bool IsFaceUp = true);
        ICard DealTopCard(bool IsFaceUp = true);
        ICard DealBottomCard(bool IsFaceUp = true);
        void Shuffle();
        bool IsDeckSorted();
        void AddCardToEnd(ICard card);
        void AddCardToTop(ICard card);
        IDeck Clone();
        void EmptyDeck();
        bool HasDuplicateCards();
        List<ICard> ToList();
        void Sort(bool descending = false);
        void InitializeDeck();
        void ResetDeck(List<ICard> newCards);
        ICard PeekCard(int pos);
        ICard PeekTopCard();
        ICard PeekLastCard();

    }
}
