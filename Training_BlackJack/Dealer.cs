using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack
{
    public class Dealer : IPlayer
    {
        internal const string DEFAULT_NAME = "Dealer";
        private string _name;
        private IHand _hand;
        private IConsoleIO _io;

        public Dealer()
        {
            _name = DEFAULT_NAME;
            _hand = new Hand();
            _io = Dependencies.consoleIO.make();
        }
        public Dealer(string name):this()
        {
            if (name == null)
            {
                name = DEFAULT_NAME;
            }
            _name = name;
        }
        public Dealer(IConsoleIO io)
        {
            _name = DEFAULT_NAME;
            _hand = new Hand();
            _io = io;
        }
        public Dealer(string name, IConsoleIO io) :this(io)
        {
            _name = name;
        }

        public Dealer(IHand hand):this()
        {
            _hand = hand;
        }

        public IHand GetHand()
        {
            return _hand;
        }

        public string GetName()
        {
            return _name;
        }

        public Interfaces.PlayerAction NextAction(IHand otherHand)
        {
            // we can ignore the value of the other player's hand for the dealer
            int thisTotal = _hand.Score(true);
            if (thisTotal == 21) { return PlayerAction.Stand; }
            if (thisTotal > 21)
            {
                return PlayerAction.Busted;
            }
            if (thisTotal > 17)
            {
                return PlayerAction.Stand;
            }
            else
            {
                if (thisTotal == 17)
                {   // hit on a soft 17
                    if (_hand.AceCount() > 0) { return PlayerAction.Hit; }
                    // stand on a hard 17
                    else { return PlayerAction.Stand; }                        
                } 
                else // less than 17
                {
                    return PlayerAction.Hit;
                }
            }
        }

        public void AddCardToHand(ICard card)
        {
            _hand.AddCard(card);
        }

        public void ClearHand()
        {
            _hand = new Hand();
        }

        public string GetPlayerType()
        {
            return this.GetType().Name;
        }
    }
}
