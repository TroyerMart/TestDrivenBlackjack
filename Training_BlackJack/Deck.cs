using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Training_BlackJack;
using Training_BlackJack.Interfaces;

namespace BlackJack
{
    public class Deck: IDeck
    {
        public readonly static int AllCardCombinationsCount = Card.SuitsCount * Card.RanksCount ;

        private List<ICard> _cards = new List<ICard>();

        public Deck()
        {
            InitializeDeck();
        }

        public Deck(List<ICard> newCards)
        {
            ResetDeck(newCards);
        }

        public Deck(bool returnEmptyDeck) : this()
        {
            if (returnEmptyDeck)
                EmptyDeck();
        }

        public void InitializeDeck()
        {
            _cards = new List<ICard>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)) )
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    ICard card = new BlackJack.Card(suit, rank);
                    AddCardToEnd(card);
                }
            }
        }

        public void EmptyDeck()
        {
            _cards = new List<ICard>();
        }

        public void ResetDeck(List<ICard> newCards)
        {
            EmptyDeck();
            if (newCards != null)
                _cards.AddRange(newCards); 
        }

        public void AddCardToEnd(ICard card)
        {
            _cards.Add(card);
        }

        public void AddCardToTop(ICard card)
        {
            InsertCard(card, 0);
        }

        public int Size()
        {
            return _cards.Count;
        }

        public ICard DealCard(int nthCard, bool IsFaceUp = true)
        {
            // return the top card off the deck
            try
            {
                ICard card = PeekCard(nthCard);
                _cards.Remove(card);
                card.Visible = IsFaceUp;
                return card;
            }
            catch
            {
                throw;
            }
        }

        public ICard DealTopCard(bool IsFaceUp = true)
        {
            // return the top card off the deck
            return DealCard(0, IsFaceUp);
        }

        public ICard DealBottomCard(bool IsFaceUp = true)
        {
            // return the bottom card from the deck
            int indexOfLastCard = _cards.Count - 1;
            return DealCard(indexOfLastCard, IsFaceUp);
        }

        public ICard PeekCard(int position)
        {
            // return the card at position from the deck (zero-based)
            if (_cards == null || _cards.Count == 0)
            {
                throw new DeckException("Cannot get card.  Deck is empty");
            }
            if (position >= _cards.Count)
            {
                throw new DeckException("Cannot get card.  Not enough cards in deck");
            }
            ICard card = _cards[position];
            return card;
        }

        public ICard PeekTopCard()
        {
            try
            {
                return PeekCard(0);
            }
            catch {
                throw;
            }
        }
        public ICard PeekLastCard()
        {
            try
            {
                return PeekCard(_cards.Count - 1);
            }
            catch {
                throw;
            }            
        }

        public void Shuffle()
        {
            const int TimesToSwap = 1000;
            //int seed = Training_BlackJack.Dependencies.randomSeeds.make();
            //Random rand = new Random(seed);
            Random rand = Dependencies.randomInstance.make();

            for (int i=0; i<TimesToSwap; i++)
            {
                int pos1 = rand.Next(_cards.Count);
                int pos2 = rand.Next(_cards.Count);
                SwapCards(pos1, pos2);
            }
        }

        public void Sort(bool desc = false)
        {
            List<ICard> cards;
            if (!desc)
            {
                cards = _cards.OrderBy(c => c.suit).ThenBy(c => c.rank).ToList();
            }
            else
            {
                cards = _cards.OrderByDescending(c => c.suit).ThenByDescending(c => c.rank).ToList();
            }
            
            ResetDeck(cards);
        }

        public List<ICard> ToList()
        {
            return _cards;
        }

        public static ICard GenerateRandomCard()
        {
            Random rand = Dependencies.randomInstance.make();
            Suit randomSuit = (Suit)rand.Next(Card.SuitsCount);
            Rank randomRank = (Rank)rand.Next(Card.RanksCount);
            return new Card(randomSuit, randomRank);
        }

        public bool HasDuplicateCards()
        {
            Dictionary<string, ICard> cards = new Dictionary<string, ICard>();
            try
            {
                foreach (ICard card in _cards)
                {
                    cards.Add(card.ToString(), card);
                }
                return false;
            }
            catch
            {
                return true; 
            }
        }

        public IDeck Clone()
        {
            IDeck d = new Deck();
            d.EmptyDeck();
            foreach (ICard card in _cards)
            {
                ICard clonedCard = card.Clone();
                d.AddCardToEnd(clonedCard);
            }
            return d;
        }

        public static IDeck GetSortedDeck(IDeck unsorted)
        {
            Deck d = (Deck)unsorted.Clone();
            List<ICard> cards = d._cards.OrderBy(c => c.suit).ThenBy(c => c.rank).ToList();
            d.ResetDeck(cards);
            return d;
        }

        public bool IsDeckSorted()
        {
            ICard smallerCard = DealTopCard();
            do
            {
                try
                {
                    ICard nextCard = DealTopCard();
                    if (smallerCard.IsGreaterSuit(nextCard))
                    {
                        return false;
                    }
                    else
                    {
                        if (smallerCard.EqualsSuit(nextCard) && smallerCard.IsGreaterRank(nextCard))
                        {
                            return false;
                        }
                    }
                    smallerCard = nextCard;
                } catch (DeckException)
                {
                    // if we made it this far, then it is sorted
                    return true;
                }
            } while (smallerCard != null);
            return true;
        }

        // Internal helper methods
        internal void InsertCard(ICard card, int position)
        {
            _cards.Insert(position, card);
        }

        internal void ReplaceCard(ICard card, int position)
        {
            // replaces the card at this position
            if (_cards == null || position >= _cards.Count)
            {
                throw new DeckException($"Unable to ReplaceCard at position {position}");
            }
            _cards[position] = card;
        }
        internal void SwapCards(int pos1, int pos2)
        {
            ICard card1 = PeekCard(pos1);
            ICard card2 = PeekCard(pos2);
            ReplaceCard(card2, pos1);
            ReplaceCard(card1, pos2);
        }


    }
}
