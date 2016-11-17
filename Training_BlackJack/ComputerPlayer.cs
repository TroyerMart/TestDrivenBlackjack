using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack
{
    public class ComputerPlayer : IPlayer
    {
        internal const string DEFAULT_NAME = "Computer";
        private string _name;
        private Hand _hand;
        IConsoleIO _io;

        public ComputerPlayer()
        {
            _name = DEFAULT_NAME;
            _hand = new Hand();
            _io = Dependencies.consoleIO.make();
        }
        public ComputerPlayer(string name):this()
        {
            if (name == null)
            {
                name = DEFAULT_NAME;
            }
            _name = name;
        }
        public ComputerPlayer(IConsoleIO io)
        {
            _name = DEFAULT_NAME;
            _hand = new Hand();
            _io = io;
        }
        public ComputerPlayer(string name, IConsoleIO io) :this(io)
        {
            _name = name;
        }

        public IHand GetHand()
        {
            return _hand;
        }

        public string GetName()
        {
            return _name;
        }

        public PlayerAction NextAction(IHand otherPlayersHand)
        {            
            int otherTotal = otherPlayersHand.Score(false);
            if (otherPlayersHand.HiddenCardsCount()==1 && otherTotal<12) {
                // assume a hidden card is 10
                otherTotal += 10;
            }
            else if (otherPlayersHand.HiddenCardsCount() > 1 && otherTotal< 12 )
            {   // assume hidden cards total at least 10, probably never more than 1 hidden card
                otherTotal += 10;
            }

            int thisTotal = _hand.Score(true);
            if (thisTotal > 21)
            {
                return PlayerAction.Busted;
            }
            if (thisTotal >= otherTotal)
            {
                if (thisTotal > 10)
                {
                    return PlayerAction.Stand;
                }
                else
                {
                    return PlayerAction.Hit;
                }                
            } 
            else
            {
                return PlayerAction.Hit;
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
