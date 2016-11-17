using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackJack;
using Training_BlackJack.Exceptions;
using Training_BlackJack;
using Training_BlackJack.Interfaces;
using Moq;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class Card_Tests
    {
        [TestMethod]
        public void suitscount_correct()
        {
            int expectedSize = Enum.GetValues(typeof(Suit)).Length; // 4
            Assert.AreEqual(expectedSize, Card.SuitsCount);
        }

        [TestMethod]
        public void rankscount_correct()
        {
            int expectedSize = Enum.GetValues(typeof(Rank)).Length;  // 13
            Assert.AreEqual(expectedSize, Card.RanksCount);
        }

        [TestMethod]
        public void initialize_card_as_explicitly_hidden()
        {
            Card card1 = new Card(Suit.Clubs, Rank.Ace, false);
            Assert.IsFalse(card1.Visible);
        }
        [TestMethod]
        public void initialize_card_as_explicitly_visible()
        {
            Card card1 = new Card(Suit.Clubs, Rank.Eight, true);
            Assert.IsTrue(card1.Visible);
        }
        [TestMethod]
        public void initialize_card_as_visible_by_default()
        {
            Card card1 = new Card(Suit.Diamonds, Rank.Eight);
            Assert.IsTrue(card1.Visible);
        }

        [TestMethod]
        public void cards_with_same_suit_are_equal_suits()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
                {
                    Card card1 = new Card(suit, rank1);
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        Card card2 = new Card(suit, rank);
                        Assert.IsTrue(card1.EqualsSuit(card2));
                        Assert.IsTrue(card2.EqualsSuit(card1));
                    }
                }
            }
        }

        [TestMethod]
        public void cards_with_different_suits_and_same_rank_are_not_equal_suits()
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank))) 
            {
                foreach (Suit suit1 in Enum.GetValues(typeof(Suit)))
                {
                    Card card1 = new Card(suit1, rank);
                    foreach (Suit suit2 in Enum.GetValues(typeof(Suit)))
                    {
                        Card card2 = new Card(suit2, rank);
                        if (card1.suit != card2.suit)
                        {
                            Assert.IsFalse(card1.EqualsSuit(card2));
                            Assert.IsFalse(card2.EqualsSuit(card1));
                        }
                        
                    }
                }
            }
        }

        [TestMethod]
        public void cards_with_same_rank_are_equal_ranks()
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suit1 in Enum.GetValues(typeof(Suit)))
                {
                    Card card1 = new Card(suit1, rank);
                    foreach (Suit suit2 in Enum.GetValues(typeof(Suit)))
                    {
                        Card card2 = new Card(suit2, rank);
                        Assert.IsTrue(card1.EqualsRank(card2));
                        Assert.IsTrue(card2.EqualsRank(card1));
                    }
                }
            }
        }
        [TestMethod]
        public void cards_with_different_ranks_and_same_suit_are_not_equal_ranks()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
                {
                    Card card1 = new Card(suit, rank1);
                    foreach (Rank rank2 in Enum.GetValues(typeof(Rank)))
                    {
                        Card card2 = new Card(suit, rank2);
                        if (card1.rank != card2.rank)
                        {
                            Assert.IsFalse(card1.EqualsRank(card2));
                            Assert.IsFalse(card2.EqualsRank(card1));
                        }

                    }
                }
            }
        }
               
        [TestMethod]
        public void are_equal_when_suit_and_rank_are_equal()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    Card card1 = new Card(suit, rank);
                    Card card2 = new Card(suit, rank);
                    Assert.IsTrue(card1.Equals(card2));
                    Assert.IsTrue(card2.Equals(card1));
                }
            }
        }

        [TestMethod]
        public void are_not_equal_when_suit_is_different()
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suit1 in Enum.GetValues(typeof(Suit)))
                {
                    Card card1 = new Card(suit1, rank);
                    foreach (Suit suit2 in Enum.GetValues(typeof(Suit)))
                    {
                        Card card2 = new Card(suit2, rank);
                        if (card1.suit != card2.suit)
                        {
                            Assert.IsFalse(card1.Equals(card2));
                            Assert.IsFalse(card2.Equals(card1));
                        }

                    }
                }
            }
        }

        [TestMethod]
        public void are_not_equal_when_ranks_are_different()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
                {
                    Card card1 = new Card(suit, rank1);
                    foreach (Rank rank2 in Enum.GetValues(typeof(Rank)))
                    {
                        Card card2 = new Card(suit, rank2);
                        if (card1.rank != card2.rank)
                        {
                            Assert.IsFalse(card1.Equals(card2));
                            Assert.IsFalse(card2.Equals(card1));
                        }

                    }
                }
            }
        }
                
        [TestMethod, ExpectedException(typeof(CardException))]
        public void comparison_throws_exception_when_second_card_is_null()
        {
            Card card1 = new Card(Suit.Spades, Rank.Ace);
            Card card2 = null;
            Assert.IsFalse(card1.Equals(card2), "Should have thrown exception instead");
            // if card1 is null, then would get object error
        }

        [TestMethod]
        public void card_must_be_initialized_with_both_suit_and_rank()
        {
            Suit cardSuit = Suit.Spades;
            Rank cardRank = Rank.Ace;
            // since Suit and Rank are both enums, they are not nullable
            // There is not a no-parameter constructor
            // Card card1 = new Card();
            Card card1 = new Card(cardSuit, cardRank);
            Assert.AreEqual(cardSuit, card1.suit);
            Assert.AreEqual(cardRank, card1.rank);
        }

        [TestMethod]
        public void card_suit_is_greater_than_comparison_card()
        {
            Rank rank = Rank.Two;
            foreach (Suit suit1 in Enum.GetValues(typeof(Suit)))
            {
                foreach (Suit suit2 in Enum.GetValues(typeof(Suit)))
                {
                    Card card1 = new Card(suit1, rank);
                    Card card2 = new Card(suit2, rank);
                    bool isGreater = (card1.suit > card2.suit);
                    // based on existing order in the enum
                    Assert.AreEqual(isGreater, card1.IsGreaterSuit(card2));
                }
            }
        }

        [TestMethod]
        public void card_rank_is_greater_than_comparison_card()
        {
            Suit suit = Suit.Spades;
            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                foreach (Rank rank2 in Enum.GetValues(typeof(Rank)))
                {
                    Card card1 = new Card(suit, rank1);
                    Card card2 = new Card(suit, rank2);
                    bool isGreater = (card1.rank > card2.rank);
                    // based on existing order in the enum
                    Assert.AreEqual(isGreater, card1.IsGreaterRank(card2));
                }
            }
        }

        [TestMethod]
        public void cloned_card_has_same_values()
        {
            ICard original = new Card(Suit.Diamonds, Rank.Seven);
            ICard cloned = original.Clone();
            Assert.IsTrue(original.Equals(cloned));
        }
        [TestMethod]
        public void cloned_card_is_a_distinct_instance()
        {
            ICard original = new Card(Suit.Diamonds, Rank.Seven);
            ICard cloned = original.Clone();
            Assert.AreNotEqual(original.GetHashCode(), cloned.GetHashCode());
        }

        // get card name
        [TestMethod]
        public void tostring_returns_card_name()
        {
            ICard card7D = new Card(Suit.Diamonds, Rank.Seven);
            ICard cardAS = new Card(Suit.Spades, Rank.Ace);
            ICard cardJC = new Card(Suit.Clubs, Rank.Jack);
            ICard card2H = new Card(Suit.Hearts, Rank.Two);
            Assert.AreEqual<string>("Seven of Diamonds", card7D.ToString());
            Assert.AreEqual<string>("Ace of Spades", cardAS.ToString());
            Assert.AreEqual<string>("Jack of Clubs", cardJC.ToString());
            Assert.AreEqual<string>("Two of Hearts", card2H.ToString());
        }


        [TestCleanup]
        public void Cleanup()
        {   // reset classes
            //typeof(Training_BlackJack.Dependencies).TypeInitializer.Invoke(null, null);
            //typeof(Card).TypeInitializer.Invoke(null, null);
        }

    }
}
