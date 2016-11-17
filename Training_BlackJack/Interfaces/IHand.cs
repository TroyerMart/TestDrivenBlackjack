using BlackJack;
using System.Collections.Generic;

namespace Training_BlackJack.Interfaces
{
    public interface IHand
    {
        void AddCard(ICard card);
        void AddCard(ICard card, bool isVisible);

        int Score(bool includeHiddenCards);

        int Size();

        string DisplayHand(bool showHiddenCards);

        List<ICard> GetCards(bool visibleOnly = false);

        int AceCount();

        bool IsBlackjack();

        int HiddenCardsCount();

    }
}