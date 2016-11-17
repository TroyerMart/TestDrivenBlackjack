using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackJack;
using Training_BlackJack;
using Training_BlackJack.Interfaces;
using Moq;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class Deck_Test
    {
        public Deck_Test()
        {   // initialize test
        }

        [TestMethod]
        public void suits_count_equals_total_number_of_suits()
        {
            int expectedSize = Enum.GetValues(typeof(Suit)).Length;
            Assert.AreEqual(expectedSize, Card.SuitsCount);
        }

        [TestMethod]
        public void ranks_count_equals_total_number_of_ranks()
        {
            int expectedSize = Enum.GetValues(typeof(Rank)).Length;
            Assert.AreEqual(expectedSize, Card.RanksCount);
        }

        [TestMethod]
        public void allcard_combinations_count_equals_suits_time_ranks()
        {
            int expectedSize = Card.SuitsCount * Card.RanksCount;
            Assert.AreEqual(expectedSize, Deck.AllCardCombinationsCount);
        }

        [TestMethod]
        public void deck_size_should_start_at_suits_times_ranks()
        {
            int expectedSize = Card.SuitsCount * Card.RanksCount;
            IDeck d = new Deck();
            int cardCount = d.Size();
            Assert.AreEqual(expectedSize, cardCount);
        }

        [TestMethod]
        public void deck_size_should_start_at_AllCardCombinationsCount()
        {
            int expectedSize = Deck.AllCardCombinationsCount;
            IDeck d = new Deck();
            int cardCount = d.Size();
            Assert.AreEqual(expectedSize, cardCount);
        }

        [TestMethod]
        public void initialize_deck_with_existing_list_of_cards_has_same_number_of_cards()
        {
            List<ICard> cards = GetListOfMockCards(3);
            IDeck d = new Deck(cards);
            Assert.AreEqual(cards.Count, d.Size() );
        }

        [TestMethod]
        public void initialize_deck_with_existing_list_of_cards_has_only_same_cards()
        {
            int N = 10;
            List<ICard> cards = GetListOfMockCards(N);
            IDeck d = new Deck(cards);

            for (int i=0; i<N; i++)
            {
                Assert.AreEqual(cards[i].GetHashCode(), d.PeekCard(i).GetHashCode());
            }
        }

        [TestMethod]
        public void initialize_deck_with_empty_list_of_cards_gives_empty_deck()
        {
            List<ICard> cards = new List<ICard>();
            IDeck d = new Deck(cards);
            Assert.AreEqual(0, d.Size());
        }

        [TestMethod]
        public void initialize_deck_with_null_list_of_cards_success()
        {
            List<ICard> cards = null;
            IDeck d = new Deck(cards);
            Assert.AreEqual(0, d.Size());
        }

        [TestMethod]
        public void initialize_deck_with_no_cards()
        {
            IDeck d = new Deck(true);
            Assert.AreEqual(0, d.Size());
        }

        [TestMethod]
        public void deck_size_should_decrement_by_one_when_top_card_is_dealt()
        {
            IDeck d = new Deck();
            int startSize = d.Size();
            ICard card = d.DealTopCard();
            int endSize = d.Size();
            Assert.AreEqual(1, startSize - endSize);
        }

        [TestMethod]
        public void deck_size_should_decrement_by_one_when_last_card_is_dealt()
        {
            IDeck d = new Deck();
            int startSize = d.Size();
            ICard card = d.DealBottomCard();
            int endSize = d.Size();
            Assert.AreEqual(1, startSize - endSize);
        }

        [TestMethod]
        public void deck_size_should_decrement_by_one_when_card_from_middle_of_deck_is_dealt()
        {
            IDeck d = new Deck();
            int middle = (int)(d.Size() / 2);
            int startSize = d.Size();
            ICard card = d.DealCard(middle);
            int endSize = d.Size();
            Assert.AreEqual(1, startSize - endSize);
        }

        [TestMethod, ExpectedException(typeof(DeckException))]
        public void dealing_from_empty_deck_throws_exception()
        {
            IDeck d = new Deck(true);
            ICard card = d.DealTopCard();
            Assert.IsFalse(true, "This should have thrown an exception");
        }

        [TestMethod, ExpectedException(typeof(DeckException))]
        public void dealing_card_by_index_greater_than_number_of_cards_throws_exception()
        {
            int N = 52;
            IDeck d = GetDeckOfMockCards(N);
            ICard card = d.DealCard(N + 1);
            Assert.IsFalse(true, "This should have thrown an exception");
        }

        [TestMethod]
        public void deal_until_deck_is_empty_returns_all_cards_without_duplication()
        {
            IDeck freshDeck = new Deck();
            IDeck testDeck = new Deck(true);
            ICard card;
            do
            {
                try
                {
                    card = freshDeck.DealTopCard();
                    testDeck.AddCardToEnd(card);
                }
                catch (DeckException ex)
                {
                    card = null;
                }
            } while (card != null);
            Assert.IsFalse(freshDeck.HasDuplicateCards(), "The initial deck must have no duplicates");
            Assert.IsFalse(testDeck.HasDuplicateCards(), "Duplicate cards found");
        }

        [TestMethod]
        public void deal_until_deck_is_empty_returns_same_number_of_cards()
        {
            int initialCardCount = 30;
            IDeck freshDeck = GetDeckOfMockCards(initialCardCount);
            IDeck testDeck = GetDeckOfMockCards(0);
            ICard card;
            do
            {
                try
                {
                    card = freshDeck.DealTopCard();
                    testDeck.AddCardToEnd(card);
                } catch (DeckException ex)
                {
                    card = null;
                }
            } while (card != null);
            Assert.AreEqual(initialCardCount, testDeck.Size());
        }

        [TestMethod]
        public void deck_starts_out_sorted_by_suit_then_rank()
        {
            IDeck d = new Deck();
            Assert.IsTrue(d.IsDeckSorted());
        }

        [TestMethod]
        public void deck_is_unsorted_after_shuffle()
        {
            IDeck d = new Deck();
            d.Shuffle();
            Assert.IsFalse(d.IsDeckSorted());
        }

        [TestMethod]
        public void shuffled_deck_that_is_sorted_returns_sorted_deck()
        {
            IDeck d = new Deck();
            d.Shuffle();
            IDeck sorted = Deck.GetSortedDeck(d);
            Assert.IsTrue(sorted.IsDeckSorted());
        }
        [TestMethod]
        public void shuffled_then_sorted_deck_gets_sorted()
        {
            IDeck d = new Deck();
            d.Shuffle();
            d.Sort();
            Assert.IsTrue(d.IsDeckSorted());
        }

        [TestMethod]
        public void shuffle_maintains_same_card_count()
        {
            Training_BlackJack.Dependencies.randomSeeds.ConstantValue = 14140;

            IDeck d = new Deck();
            int initalCards = d.Size();
            d.Shuffle();
            int finalCards = d.Size();
            Assert.AreEqual<int>(initalCards, finalCards);
        }

        [TestMethod]
        public void shuffle_randomizes_deck()
        {
            //Dependencies.randomSeeds.ConstantValue = 1414;
            Dependencies.randomInstance.ConstantValue = new Random(1414);

            IDeck d = new Deck();
            d.Shuffle();
            Assert.IsTrue(d.DealTopCard().Equals(new Card(Suit.Clubs, Rank.Two)));
            Assert.IsTrue(d.DealTopCard().Equals(new Card(Suit.Clubs, Rank.Three)));
            Assert.IsTrue(d.DealTopCard().Equals(new Card(Suit.Hearts, Rank.Seven)));
            Assert.IsTrue(d.DealTopCard().Equals(new Card(Suit.Clubs, Rank.Jack)));
            Assert.IsTrue(d.DealBottomCard().Equals(new Card(Suit.Hearts, Rank.Ace)));

            Dependencies.randomInstance.reset();
        }

        [TestMethod]
        public void swap_maintains_same_card_count()
        {
            Deck d = new Deck();
            int pos1 = 10;
            int pos2 = 22;
            int initalCards = d.Size();
            d.SwapCards(pos1, pos2);
            int finalCards = d.Size();
            Assert.AreEqual(initalCards, finalCards);
        }

        [TestMethod]
        public void swap_switches_cards()
        {
            Deck d = new Deck();
            for (int pos1 = 0; pos1 < d.Size(); pos1++)
            {
                for (int pos2 = 0; pos2 < d.Size(); pos2++)
                {
                    if (pos1 != pos2)
                    {
                        ICard unswapped1 = d.PeekCard(pos1).Clone();
                        ICard unswapped2 = d.PeekCard(pos2).Clone();
                        d.SwapCards(pos1, pos2);
                        ICard swapped1 = d.PeekCard(pos1).Clone();
                        ICard swapped2 = d.PeekCard(pos2).Clone();
                        Assert.IsTrue(unswapped1.Equals(swapped2));
                        Assert.IsTrue(unswapped2.Equals(swapped1));
                    }
                }
            }            
        }

        [TestMethod]
        public void swap_internal_cards_does_not_affect_top_or_bottom_cards()
        {
            Deck d = new Deck();
            for (int pos1 = 1; pos1 < d.Size()-1; pos1++)
            {
                for (int pos2 = 1; pos2 < d.Size()-1; pos2++)
                {
                    if (pos1 != pos2)
                    {
                        ICard untouchedFirst = d.PeekTopCard().Clone();
                        ICard untouchedLast = d.PeekLastCard().Clone();
                        d.SwapCards(pos1, pos2);
                        ICard swapped1 = d.PeekCard(pos1).Clone();
                        ICard swapped2 = d.PeekCard(pos2).Clone();
                        Assert.IsTrue(untouchedFirst.Equals(d.PeekTopCard()));
                        Assert.IsTrue(untouchedLast.Equals(d.PeekLastCard()));
                    }
                }
            }
        }

        [TestMethod]
        public void sort_by_rank_same_suit()
        {
            IDeck d = new Deck(true);
            Mock<ICard> card1 = new Mock<ICard>();
            Mock<ICard> card2 = new Mock<ICard>();
            Mock<ICard> card3 = new Mock<ICard>();
            card1.Setup(c => c.rank).Returns(Rank.Ten);
            card2.Setup(c => c.rank).Returns(Rank.Ace);
            card3.Setup(c => c.rank).Returns(Rank.King);
            card1.Setup(c => c.suit).Returns(Suit.Hearts);
            card2.Setup(c => c.suit).Returns(Suit.Hearts);
            card3.Setup(c => c.suit).Returns(Suit.Hearts);

            d.AddCardToTop(card1.Object);
            d.AddCardToTop(card2.Object);
            d.AddCardToTop(card3.Object);

            d.Sort();

            ICard sorted1 = d.PeekCard(0);
            ICard sorted2 = d.PeekCard(1);
            ICard sorted3 = d.PeekCard(2);

            Assert.AreEqual(card2.Object.GetHashCode(), sorted1.GetHashCode());
            Assert.AreEqual(card1.Object.GetHashCode(), sorted2.GetHashCode());
            Assert.AreEqual(card3.Object.GetHashCode(), sorted3.GetHashCode());
        }
        
        [TestMethod]
        public void sort_by_rank_same_suit_descending()
        {
            IDeck d = new Deck(true);
            Mock<ICard> card1 = new Mock<ICard>();
            Mock<ICard> card2 = new Mock<ICard>();
            Mock<ICard> card3 = new Mock<ICard>();
            card1.Setup(c => c.rank).Returns(Rank.Ten);
            card2.Setup(c => c.rank).Returns(Rank.Ace);
            card3.Setup(c => c.rank).Returns(Rank.King);
            card1.Setup(c => c.suit).Returns(Suit.Hearts);
            card2.Setup(c => c.suit).Returns(Suit.Hearts);
            card3.Setup(c => c.suit).Returns(Suit.Hearts);

            d.AddCardToTop(card1.Object);
            d.AddCardToTop(card2.Object);
            d.AddCardToTop(card3.Object);

            d.Sort(true);

            ICard sorted1 = d.PeekCard(0);
            ICard sorted2 = d.PeekCard(1);
            ICard sorted3 = d.PeekCard(2);

            Assert.AreEqual(card3.Object.GetHashCode(), sorted1.GetHashCode());
            Assert.AreEqual(card1.Object.GetHashCode(), sorted2.GetHashCode());
            Assert.AreEqual(card2.Object.GetHashCode(), sorted3.GetHashCode());
        }

        [TestMethod]
        public void sort_by_suit_same_rank()
        {
            IDeck d = new Deck(true);

            Mock<ICard> card1 = new Mock<ICard>();
            Mock<ICard> card2 = new Mock<ICard>();
            Mock<ICard> card3 = new Mock<ICard>();
            card1.Setup(c => c.rank).Returns(Rank.Seven);
            card2.Setup(c => c.rank).Returns(Rank.Seven);
            card3.Setup(c => c.rank).Returns(Rank.Seven);
            card1.Setup(c => c.suit).Returns(Suit.Hearts);
            card2.Setup(c => c.suit).Returns(Suit.Spades);
            card3.Setup(c => c.suit).Returns(Suit.Clubs);

            d.AddCardToTop(card1.Object);
            d.AddCardToTop(card2.Object);
            d.AddCardToTop(card3.Object);

            d.Sort();

            ICard sorted1 = d.PeekCard(0);
            ICard sorted2 = d.PeekCard(1);
            ICard sorted3 = d.PeekCard(2);

            Assert.AreEqual(card3.Object.GetHashCode(), sorted1.GetHashCode());
            Assert.AreEqual(card1.Object.GetHashCode(), sorted2.GetHashCode());
            Assert.AreEqual(card2.Object.GetHashCode(), sorted3.GetHashCode());
        }

        [TestMethod]
        public void sort_by_suit_same_rank_descending()
        {
            IDeck d = new Deck(true);
            Mock<ICard> card1 = new Mock<ICard>();
            Mock<ICard> card2 = new Mock<ICard>();
            Mock<ICard> card3 = new Mock<ICard>();
            card1.Setup(c => c.rank).Returns(Rank.Seven);
            card2.Setup(c => c.rank).Returns(Rank.Seven);
            card3.Setup(c => c.rank).Returns(Rank.Seven);
            card1.Setup(c => c.suit).Returns(Suit.Hearts);
            card2.Setup(c => c.suit).Returns(Suit.Spades);
            card3.Setup(c => c.suit).Returns(Suit.Clubs);

            d.AddCardToTop(card1.Object);
            d.AddCardToTop(card2.Object);
            d.AddCardToTop(card3.Object);

            d.Sort(true);

            ICard sorted1 = d.PeekCard(0);
            ICard sorted2 = d.PeekCard(1);
            ICard sorted3 = d.PeekCard(2);

            Assert.AreEqual(card2.Object.GetHashCode(), sorted1.GetHashCode());
            Assert.AreEqual(card1.Object.GetHashCode(), sorted2.GetHashCode());
            Assert.AreEqual(card3.Object.GetHashCode(), sorted3.GetHashCode());
        }

        [TestMethod]
        public void reset_deck_replaces_all_cards_in_empty_deck()
        {
            int N = 20;
            IDeck d = new Deck(true);
            List<ICard> cards = GetListOfMockCards(N);
            d.ResetDeck(cards);
            Assert.AreEqual(cards.Count, d.Size());
        }
        [TestMethod]
        public void reset_deck_replaces_all_cards_in_existing_deck()
        {
            int initialCount = 5;
            int finalCount = 10;

            IDeck d = GetDeckOfMockCards(initialCount);
            List<ICard> cards = GetListOfMockCards(finalCount);
            d.ResetDeck(cards);
            Assert.AreEqual(cards.Count, d.Size());
        }

        [TestMethod]
        public void peekcard_returns_right_card_from_deck()
        {
            int N = 15;
            List<ICard> cards = GetListOfMockCards(N);
            IDeck d = new Deck(cards);

            for (int i= 0; i< N; i++)
            {
                ICard card = cards[i];
                ICard peekCard = d.PeekCard(i);
                Assert.AreEqual(card.GetHashCode(), peekCard.GetHashCode());

            }
        }

        [TestMethod]
        public void peekcard_returns_right_card_from_sorted_deck()
        {
            // sorted by Suit, then Rank
            IDeck d = new Deck();
            d.Sort();
            int index = 0;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    index = (int)suit * 13 + (int)rank;
                    ICard card = d.PeekCard(index);
                    Assert.IsTrue(card.Equals(new Card(suit, rank)));
                }
            }
        }

        [TestMethod]
        public void peek_top_card_matches_dealt_first_card()
        {
            int N = 5;
            IDeck d = GetDeckOfMockCards(N);
            ICard topCard = d.PeekTopCard();
            Assert.AreEqual(d.DealTopCard().GetHashCode(), topCard.GetHashCode());
        }

        [TestMethod]
        public void peek_last_card_matches_dealt_last_card()
        {
            int N = 7;
            IDeck d = GetDeckOfMockCards(N);
            ICard bottomCard = d.PeekLastCard();
            Assert.AreEqual(d.DealBottomCard().GetHashCode(), bottomCard.GetHashCode());
        }

        [TestMethod, ExpectedException(typeof(DeckException))]
        public void peek_card_no_cards_exception()
        {
            IDeck d = new Deck(true);
            d.PeekCard(0);
            Assert.IsFalse(true, "This should have thrown an exception");
        }

        [TestMethod, ExpectedException(typeof(DeckException))]
        public void getcard_past_end_of_deck_exception()
        {
            int N = 8;
            IDeck d = GetDeckOfMockCards(N);
            d.PeekCard(d.Size() + 10);
            Assert.IsFalse(true, "This should have thrown an exception");
        }

        [TestMethod]
        public void emptydeck_removes_all_cards_from_deck()
        {
            int N = 52;
            IDeck d = GetDeckOfMockCards(N);
            d.EmptyDeck();
            Assert.AreEqual(0, d.Size());
        }

        [TestMethod]
        public void calling_emptydeck_when_empty_does_not_fail()
        {
            int N = 40;
            IDeck d = GetDeckOfMockCards(N);
            d.EmptyDeck();
            Assert.AreEqual(0, d.Size());
        }

        [TestMethod]
        public void replacecard_puts_card_in_correct_position_in_deck()
        {
            int N = 52;
            for (int positionToReplace=0; positionToReplace<N; positionToReplace++)
            {
                Deck d = (Deck)GetDeckOfMockCards(N);
                Mock<ICard> replacementCard = new Mock<ICard>();
                ICard originalCardToReplace = d.PeekCard(positionToReplace);
                d.ReplaceCard(replacementCard.Object, positionToReplace);
                ICard cardNowAtOriginalLocation = d.PeekCard(positionToReplace);
                Assert.AreEqual(replacementCard.Object.GetHashCode(), cardNowAtOriginalLocation.GetHashCode());
                Assert.AreNotEqual(originalCardToReplace.GetHashCode(), cardNowAtOriginalLocation.GetHashCode());
            }
        }

        [TestMethod]
        public void replacecard_first_position_in_deck()
        {
            int N = 20;
            int positionToReplace = 0;
            Deck d = (Deck)GetDeckOfMockCards(N);
            Mock<ICard> replacementCard = new Mock<ICard>();
            ICard originalCardToReplace = d.PeekCard(positionToReplace);
            d.ReplaceCard(replacementCard.Object, positionToReplace);
            ICard cardNowAtOriginalLocation = d.PeekCard(positionToReplace);
            Assert.AreEqual(replacementCard.Object.GetHashCode(), cardNowAtOriginalLocation.GetHashCode());
            Assert.AreNotEqual(originalCardToReplace.GetHashCode(), cardNowAtOriginalLocation.GetHashCode());
        }

        [TestMethod]
        public void replacecard_last_position_in_deck()
        {
            int N = 21;
            Deck d = (Deck)GetDeckOfMockCards(N);
            int positionToReplace = d.Size() - 1;
            Mock<ICard> replacementCard = new Mock<ICard>();
            ICard originalCardToReplace = d.PeekCard(positionToReplace);
            d.ReplaceCard(replacementCard.Object, positionToReplace);
            ICard cardNowAtOriginalLocation = d.PeekCard(positionToReplace);
            Assert.AreEqual(replacementCard.Object.GetHashCode(), cardNowAtOriginalLocation.GetHashCode());
            Assert.AreNotEqual(originalCardToReplace.GetHashCode(), cardNowAtOriginalLocation.GetHashCode());
        }

        [TestMethod, ExpectedException(typeof(DeckException))]
        public void replacecard_nocards_exception()
        {
            Deck d = (Deck)GetDeckOfMockCards(0);
            int positionToReplace = 10;
            Mock<ICard> replacementCard = new Mock<ICard>();
            d.ReplaceCard(replacementCard.Object, positionToReplace);
            Assert.IsFalse(true, "This should have thrown an exception");
        }

        [TestMethod, ExpectedException(typeof(DeckException))]
        public void replacecard_past_end_of_deck_exception()
        {
            int N = 52;
            Deck d = (Deck)GetDeckOfMockCards(N);
            int positionToReplace = d.Size() + 1;
            Mock<ICard> replacementCard = new Mock<ICard>();
            d.ReplaceCard(replacementCard.Object, positionToReplace);
            Assert.IsFalse(true, "This should have thrown an exception");
        }

        [TestMethod]
        public void initial_deck_has_all_unique_card_combinations()
        {
            IDeck d = new Deck();
            Assert.IsFalse(d.HasDuplicateCards());
        }

        [TestMethod]
        public void deck_has_duplicate_cards()
        {
            IDeck d = new Deck();
            bool hasDuplicateBefore = d.HasDuplicateCards();
            ICard duplicate = d.PeekCard(42).Clone();
            d.AddCardToEnd(duplicate);
            bool hasDuplicateAfter = d.HasDuplicateCards();
            Assert.IsFalse(hasDuplicateBefore);
            Assert.IsTrue(hasDuplicateAfter);
        }

        [TestMethod]
        public void add_card_to_end_of_deck()
        {
            int N = 48;
            IDeck d = GetDeckOfMockCards(N);
            Mock<ICard> newCard = new Mock<ICard>();
            d.AddCardToEnd(newCard.Object);
            ICard lastCard = d.DealBottomCard();
            Assert.AreEqual(newCard.GetHashCode(), lastCard.GetHashCode());
        }

        [TestMethod]
        public void add_card_to_top_of_deck()
        {
            int N = 48;
            IDeck d = GetDeckOfMockCards(N);
            Mock<ICard> newCard = new Mock<ICard>();
            d.AddCardToTop(newCard.Object);
            ICard topCard = d.DealTopCard();
            Assert.AreEqual(newCard.GetHashCode(), topCard.GetHashCode());
        }

        [TestMethod]
        public void insert_card_at_a_specific_position()
        {
            int N = 52;
            int positionToInsert = 5;
            Deck d = (Deck)GetDeckOfMockCards(N);
            Mock<ICard> newCard = new Mock<ICard>();
            d.InsertCard(newCard.Object, positionToInsert);
            ICard insertedCard = d.DealCard(positionToInsert);
            Assert.AreEqual(newCard.GetHashCode(), insertedCard.GetHashCode());
        }

        [TestMethod]
        public void deal_card_face_down()
        {
            IDeck d = new Deck();
            ICard card = d.DealTopCard(false);
            Assert.IsFalse(card.Visible);
        }

        [TestMethod]
        public void deal_card_face_up_by_default()
        {
            IDeck d = new Deck();
            ICard card1 = d.DealTopCard();
            Assert.IsTrue(card1.Visible);
        }

        [TestMethod]
        public void deal_card_face_up_explicitly()
        {
            IDeck d = new Deck();
            ICard card2 = d.DealTopCard(true);
            Assert.IsTrue(card2.Visible);
        }

        [TestMethod]
        public void create_and_returns_new_randomcard()
        {
            Dependencies.randomInstance.ConstantValue = new Random(13);

            ICard randomCard1 = Deck.GenerateRandomCard();
            ICard randomCard2 = Deck.GenerateRandomCard();
            ICard randomCard3 = Deck.GenerateRandomCard();
            ICard randomCard4 = Deck.GenerateRandomCard();

            Assert.IsTrue(randomCard1.Equals(new Card(Suit.Hearts, Rank.Nine)));
            Assert.IsTrue(randomCard2.Equals(new Card(Suit.Spades, Rank.Five)));
            Assert.IsTrue(randomCard3.Equals(new Card(Suit.Clubs, Rank.Queen)));
            Assert.IsTrue(randomCard4.Equals(new Card(Suit.Hearts, Rank.King)));
            Dependencies.randomInstance.reset();
        }

        /***************** helper methods  ************************/
        private List<ICard> GetListOfMockCards(int numberOfCards)
        {
            List<ICard> cards = new List<ICard>();
            for (int i = 0; i < numberOfCards; i++)
            {
                Mock<ICard> card = new Mock<ICard>();
                cards.Add(card.Object);
            }
            return cards;
        }

        private IDeck GetDeckOfMockCards(int numberOfCards)
        {
            List<ICard> cards = GetListOfMockCards(numberOfCards);
            IDeck mockDeck = new Deck(cards);
            return mockDeck;
        }

        [TestCleanup]
        public void Cleanup()
        {   // reset Dependencies after test
            typeof(Training_BlackJack.Dependencies).TypeInitializer.Invoke(null, null);
        }

    }
}
