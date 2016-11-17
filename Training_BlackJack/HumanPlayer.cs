using System;
using System.Linq;
using BlackJack;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack
{
    public class HumanPlayer : IPlayer
    {
        internal const string DEFAULT_NAME = "Me";
        internal const int MAXRETRIES = 10;
        private string _name;
        private IHand _hand;
        IConsoleIO _io;

        public HumanPlayer()
        {
            _name = DEFAULT_NAME;
            _hand = new Hand();
            _io = Dependencies.consoleIO.make();
        }
        public HumanPlayer(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = DEFAULT_NAME;
            }
            _name = name;
        }
        public HumanPlayer(IConsoleIO io)
        {
            _name = DEFAULT_NAME;
            _hand = new Hand();
            _io = io;
        }
        public HumanPlayer(string name, IConsoleIO io) :this(io)
        {
            _name = name;
        }

        public HumanPlayer(IHand hand):this()
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

        public PlayerAction NextAction(IHand otherHand)
        {
            // This needs to prompt the user for their choice of action
            PlayerAction nextAction = PlayerAction.Invalid;
            int i = 0;
            do
            {
                string actionEntered = _io.PromptForString("Enter Action H)it, S)tand, B)ust:");
                nextAction = ParseAction(actionEntered);
                if (nextAction == PlayerAction.Invalid)
                {
                    _io.WriteLine("Invalid action entered");
                    i++;
                }
            } while (nextAction == PlayerAction.Invalid && i<MAXRETRIES);            
            return nextAction;
        }

        internal PlayerAction ParseAction(string actionEntered)
        {
            PlayerAction action;
            if (String.IsNullOrWhiteSpace(actionEntered))
            {
                return PlayerAction.Invalid;
            }
            char firstChar = actionEntered.Trim().ToUpper().First();
            switch (firstChar)
            {
                case 'H':
                    action = PlayerAction.Hit;
                    break;
                case 'S':
                    action = PlayerAction.Stand;
                    break;
                case 'B':
                    action = PlayerAction.Busted;
                    break;
                default:
                    action = PlayerAction.Invalid;
                    break;
            }
            return action;
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
