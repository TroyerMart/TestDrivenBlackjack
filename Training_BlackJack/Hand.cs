using BlackJack;
using System.Collections.Generic;
using System.Linq;
using Training_BlackJack.Exceptions;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack
{
    public class Hand: IHand
    {
        public const string FACEDOWN = "HIDDEN";

        List<ICard> _hand = new List<ICard>();

        public Hand()
        { }

        public void AddCard(ICard card)
        {
            if (card == null)
            {
                throw new HandException("Cannot add a blank card");
            }
            _hand.Add(card);
        }

        public void AddCard(ICard card, bool isVisible)
        {
            if (card == null)
            {
                throw new HandException("Cannot add a blank card");
            }
            card.Visible = isVisible;
            _hand.Add(card);
        }

        public int Score(bool includeHiddenCards)
        {
            return ScoreHand(this, includeHiddenCards);
        }

        public int Size()
        {
            return _hand.Count();
        }

        public string DisplayHand(bool showHiddenCards)
        {
            string display = "";
            List<ICard> cardsInHand = GetCards(false);
            foreach (ICard card in cardsInHand)
            {
                if (display != "")
                {
                    display += ",";
                }
                if (showHiddenCards || card.Visible)
                {
                    display += card.ToString();
                }
                else
                {
                    display += FACEDOWN;
                }
            }
            return display;
        }

        public List<ICard> GetCards(bool visibleOnly = false)
        {
            if (visibleOnly)
            {
                return _hand.Where<ICard>(c => c.Visible).ToList<ICard>();
            }
            else
            {
                return _hand;
            }
            
        }

        private int ScoreHand(IHand hand, bool includeHiddenCards)
        {
            int totalScore = 0;
            var cardsToScore = hand.GetCards(! includeHiddenCards);
            foreach (ICard card in cardsToScore)
            {
                totalScore += BlackjackGame.GetCardValue(card);
            }
            
            if (hand.AceCount() > 0)
            {
                // assume they will want to Stand on 18+
                if (totalScore > 7 && totalScore < 12)
                {
                    if (includeHiddenCards == true)   // when cards are hidden, just take lowest total
                        { totalScore += 10; }
                }
            }
            return totalScore;
        }

        public int AceCount()
        {
            int count = _hand.Where(c => c.rank == Rank.Ace).Count();
            return count;
        }

        public bool IsBlackjack()
        {
            if (AceCount() != 1)
            {
                return false;
            }
            if (Score(true) == 21 && Size() == 2)
            {
                return true;
            }
            return false;
        }

        public int HiddenCardsCount()
        {
            int count = _hand.Where(c => !c.Visible).Count();
            return count;
        }
    }
}
