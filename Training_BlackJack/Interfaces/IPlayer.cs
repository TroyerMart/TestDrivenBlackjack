using BlackJack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack.Interfaces
{
    public enum PlayerAction
    {
        Hit,
        Stand,
        Busted,
        Invalid
    }

    public interface IPlayer
    {
        string GetName();

        IHand GetHand();

        void AddCardToHand(ICard card);

        void ClearHand();

        PlayerAction NextAction(IHand otherHand);

        string GetPlayerType();
    }
}
