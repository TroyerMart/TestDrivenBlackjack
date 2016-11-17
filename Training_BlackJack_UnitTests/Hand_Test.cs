using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack.Exceptions;
using Training_BlackJack;
using BlackJack;
using Training_BlackJack.Interfaces;
using Moq;
using System.Collections.Generic;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class Hand_Test
    {
        private Mock<ICard> mockCard1;
        private Mock<ICard> mockCard2;
        private Mock<ICard> mockCard3;

        [TestInitialize]
        public void Initialize()
        {
            mockCard1 = new Mock<ICard>();
            mockCard2 = new Mock<ICard>();
            mockCard3 = new Mock<ICard>();
        }

        [TestMethod]
        public void new_hand_contains_no_cards()
        {
            IHand hand = new Hand();
            Assert.AreEqual(0, hand.Size());
        }

        [TestMethod]
        public void add_card_to_hand()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            hand.AddCard(card1);
            Assert.AreEqual(1, hand.Size());
        }

        [TestMethod, ExpectedException(typeof(HandException))]
        public void add_null_card_to_hand_expect_exception()
        {
            IHand hand = new Hand();
            ICard card1 = null;
            hand.AddCard(card1);
            Assert.IsTrue(false, "Should have thrown exception");
        }

        [TestMethod]
        public void return_number_of_cards_in_hand_first_card_added()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            hand.AddCard(card1);
            Assert.AreEqual(1, hand.Size());
        }
        [TestMethod]
        public void return_number_of_cards_in_hand_two_cards_added()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            ICard card2 = mockCard2.Object;
            hand.AddCard(card1);
            hand.AddCard(card2);
            Assert.AreEqual(2, hand.Size());
        }

        [TestMethod]
        public void return_same_card_added_to_hand()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            hand.AddCard(card1);
            ICard card2 = hand.GetCards()[0];
            Assert.AreEqual(card1.GetHashCode(), card2.GetHashCode());
        }

        [TestMethod]
        public void return_same_cards_when_multiple_cards_added_to_hand()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            ICard card2 = mockCard2.Object;
            hand.AddCard(card1);
            hand.AddCard(card2);
            ICard card3 = hand.GetCards()[0];
            ICard card4 = hand.GetCards()[1];
            Assert.AreEqual(card1.GetHashCode(), card3.GetHashCode());
            Assert.AreEqual(card2.GetHashCode(), card4.GetHashCode());
        }

        [TestMethod]
        public void add_card_to_hand_doesnt_change_visibility()
        {
            //Mock<ICard> mockCard1 = new Mock<ICard>();
            IHand hand = new Hand();
            //mockCard1.Setup(c => c.Visible).Returns(true);
            mockCard1.SetupSet(c => c.Visible = It.IsAny<bool>()).Verifiable();
            ICard card1 = mockCard1.Object;
            hand.AddCard(card1);
            mockCard1.VerifySet(c => c.Visible = It.IsAny<bool>(), Times.Never());
        }

        [TestMethod]
        public void add_card_to_hand_as_hidden_sets_visibility_to_false()
        {
            Mock<ICard> mockCard1 = new Mock<ICard>();
            IHand hand = new Hand();
            //mockCard1.SetupSet(c => c.Visible=It.IsAny<bool>()).Verifiable();
            ICard card1 = mockCard1.Object;
            hand.AddCard(card1, false);

            mockCard1.VerifySet(c => c.Visible=It.IsAny<bool>(), Times.Once());
            mockCard1.VerifySet(c => c.Visible = false, Times.Once());
        }

        [TestMethod]
        public void add_card_to_hand_as_visible_sets_visibility_to_visible()
        {
            Mock<ICard> mockCard1 = new Mock<ICard>();
            IHand hand = new Hand();
            //mockCard1.SetupSet(c => c.Visible=It.IsAny<bool>()).Verifiable();
            ICard card1 = mockCard1.Object;
            hand.AddCard(card1, true);

            mockCard1.VerifySet(c => c.Visible = It.IsAny<bool>(), Times.Once());
            mockCard1.VerifySet(c => c.Visible = true, Times.Once());
        }

        [TestMethod]
        public void return_only_visible_cards_from_hand()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            ICard card2 = mockCard2.Object;
            mockCard1.Setup(c => c.Visible).Returns(false);
            mockCard2.Setup(c => c.Visible).Returns(true);
            hand.AddCard(card1);
            hand.AddCard(card2);
            var visibleCards = hand.GetCards(true);
            Assert.AreEqual(1, visibleCards.Count);
            Assert.AreEqual(card2.GetHashCode(), visibleCards[0].GetHashCode());
            Assert.AreNotEqual(card1.GetHashCode(), visibleCards[0].GetHashCode());
        }

        [TestMethod]
        public void display_all_cards_when_all_cards_are_visible()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            ICard card2 = mockCard2.Object;
            ICard card3 = mockCard3.Object;
            mockCard1.Setup(c => c.Visible).Returns(true);
            mockCard2.Setup(c => c.Visible).Returns(true);
            mockCard3.Setup(c => c.Visible).Returns(true);
            mockCard1.Setup(c => c.ToString()).Returns("Rank1 of Suit1");
            mockCard2.Setup(c => c.ToString()).Returns("Rank2 of Suit2");
            mockCard3.Setup(c => c.ToString()).Returns("Rank3 of Suit3");
            hand.AddCard(card1);
            hand.AddCard(card2);
            hand.AddCard(card3);
            string expectedDisplay = $"{card1.ToString()},{card2.ToString()},{card3.ToString()}";
            string actualDisplay = hand.DisplayHand(true);
            Assert.AreEqual(expectedDisplay, actualDisplay);
        }

        [TestMethod]
        public void display_all_cards_when_not_all_cards_are_visible()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            ICard card2 = mockCard2.Object;
            ICard card3 = mockCard3.Object;
            mockCard1.Setup(c => c.Visible).Returns(true);
            mockCard2.Setup(c => c.Visible).Returns(false);
            mockCard3.Setup(c => c.Visible).Returns(true);
            mockCard1.Setup(c => c.ToString()).Returns("Rank1 of Suit1");
            mockCard2.Setup(c => c.ToString()).Returns("Rank2 of Suit2");
            mockCard3.Setup(c => c.ToString()).Returns("Rank3 of Suit3");
            hand.AddCard(card1);
            hand.AddCard(card2);
            hand.AddCard(card3);
            string expectedDisplay = $"{card1.ToString()},{card2.ToString()},{card3.ToString()}";
            string actualDisplay = hand.DisplayHand(true);
            Assert.AreEqual(expectedDisplay, actualDisplay);
        }

        [TestMethod]
        public void display_only_visible_cards_when_not_all_cards_are_visible()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            ICard card2 = mockCard2.Object;
            ICard card3 = mockCard3.Object;
            mockCard1.Setup(c => c.Visible).Returns(true);
            mockCard2.Setup(c => c.Visible).Returns(false);
            mockCard3.Setup(c => c.Visible).Returns(true);
            mockCard1.Setup(c => c.ToString()).Returns("Rank1 of Suit1");
            mockCard2.Setup(c => c.ToString()).Returns($"{Hand.FACEDOWN}");
            mockCard3.Setup(c => c.ToString()).Returns("Rank3 of Suit3");
            hand.AddCard(card1);
            hand.AddCard(card2);
            hand.AddCard(card3);
            string expectedDisplay = $"{card1.ToString()},{card2.ToString()},{card3.ToString()}";
            string actualDisplay = hand.DisplayHand(false);
            Assert.AreEqual(expectedDisplay, actualDisplay);
        }
        
        [TestMethod]
        public void return_all_cards_in_hand_regardless_of_visibility()
        {
            IHand hand = new Hand();
            ICard card1 = mockCard1.Object;
            ICard card2 = mockCard2.Object;
            ICard card3 = mockCard3.Object;
            hand.AddCard(card1, false);
            hand.AddCard(card2);
            hand.AddCard(card3);
            var cards = hand.GetCards();
            Assert.AreEqual(3, cards.Count);
        }

        [TestMethod]
        public void return_score_of_3_card_hand_with_just_one_ace()
        {
            // start with an Ace
            ICard card3 = mockCard3.Object;
            mockCard3.Setup(c => c.rank).Returns(Rank.Ace);

            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                if (rank1 != Rank.Ace)
                {
                    mockCard1.Setup(c => c.rank).Returns(rank1);
                    foreach (Rank rank2 in Enum.GetValues(typeof(Rank)))
                    {
                        if (rank2 !=Rank.Ace)
                        {
                            IHand hand = new Hand();
                            ICard card1 = mockCard1.Object;
                            ICard card2 = mockCard2.Object;

                            hand.AddCard(card1);
                            hand.AddCard(card2);
                            hand.AddCard(card3);

                            mockCard2.Setup(c => c.rank).Returns(rank2);
                            int scoreCard1 = BlackjackGame.GetCardValue(card1);
                            int scoreCard2 = BlackjackGame.GetCardValue(card2);

                            int score = hand.Score(true);
                            int expectedScore = scoreCard1 + scoreCard2 + 1;
                            Assert.AreEqual(1, hand.AceCount());
                            Assert.IsTrue(expectedScore == score || expectedScore + 10 == score);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void return_score_of_2_card_hand_with_no_aces()
        {
            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                mockCard1.Setup(c => c.rank).Returns(rank1);
                foreach (Rank rank2 in Enum.GetValues(typeof(Rank)))
                {
                    IHand hand = new Hand();
                    ICard card1 = mockCard1.Object;
                    ICard card2 = mockCard2.Object;

                    hand.AddCard(card1);
                    hand.AddCard(card2);

                    mockCard2.Setup(c => c.rank).Returns(rank2);
                    int scoreCard1 = BlackjackGame.GetCardValue(card1);
                    int scoreCard2 = BlackjackGame.GetCardValue(card2);

                    int score = hand.Score(true);
                    int expectedScore = scoreCard1 + scoreCard2;
                    if (hand.AceCount() == 0)
                    {
                        Assert.IsTrue(expectedScore == score);
                    }
                }
            }
        }

        [TestMethod]
        public void return_score_of_3_card_hand_with_no_aces()
        {
            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                mockCard1.Setup(c => c.rank).Returns(rank1);
                foreach (Rank rank2 in Enum.GetValues(typeof(Rank)))
                {
                    mockCard2.Setup(c => c.rank).Returns(rank2);
                    foreach (Rank rank3 in Enum.GetValues(typeof(Rank)))
                    {
                        mockCard3.Setup(c => c.rank).Returns(rank3);
                        IHand hand = new Hand();
                        ICard card1 = mockCard1.Object;
                        ICard card2 = mockCard2.Object;
                        ICard card3 = mockCard3.Object;

                        hand.AddCard(card1);
                        hand.AddCard(card2);
                        hand.AddCard(card3);

                        int scoreCard1 = BlackjackGame.GetCardValue(card1);
                        int scoreCard2 = BlackjackGame.GetCardValue(card2);
                        int scoreCard3 = BlackjackGame.GetCardValue(card3);

                        int score = hand.Score(true);
                        int expectedScore = scoreCard1 + scoreCard2 + scoreCard3;
                        if (hand.AceCount() == 0)
                        {
                            Assert.IsTrue(expectedScore == score);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void return_number_of_aces_in_hand()
        {
            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                mockCard1.Setup(c => c.rank).Returns(rank1);
                foreach (Rank rank2 in Enum.GetValues(typeof(Rank)))
                {
                    mockCard2.Setup(c => c.rank).Returns(rank2);
                    foreach (Rank rank3 in Enum.GetValues(typeof(Rank)))
                    {
                        mockCard3.Setup(c => c.rank).Returns(rank3);
                        IHand hand = new Hand();
                        ICard card1 = mockCard1.Object;
                        ICard card2 = mockCard2.Object;
                        ICard card3 = mockCard3.Object;
                        int expectedAceCount = 0;

                        hand.AddCard(card1);
                        hand.AddCard(card2);
                        hand.AddCard(card3);

                        if (card1.rank == Rank.Ace)
                        {
                            expectedAceCount++;
                        }
                        if (card2.rank == Rank.Ace)
                        {
                            expectedAceCount++;
                        }
                        if (card3.rank == Rank.Ace)
                        {
                            expectedAceCount++;
                        }

                        Assert.AreEqual(expectedAceCount, hand.AceCount());
                    }
                }
            }
        }

        [TestMethod]
        public void return_score_of_one_card_hand()
        {
            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                IHand hand = new Hand();
                ICard card1 = mockCard1.Object;
                mockCard1.Setup(c => c.rank).Returns(rank1);
                hand.AddCard(card1);
                int expectedScore = BlackjackGame.GetCardValue(card1);
                Assert.AreEqual(expectedScore, hand.Score(true));
            }
        }

        [TestMethod]
        public void return_score_of_hand_when_blackjack()
        {

            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                if (rank1 > Rank.Nine)
                {
                    mockCard1.Setup(c => c.rank).Returns(rank1);
                    mockCard2.Setup(c => c.rank).Returns(Rank.Ace);
                    IHand hand = new Hand();
                    ICard card1 = mockCard1.Object;
                    ICard card2 = mockCard2.Object;

                    hand.AddCard(card1);
                    hand.AddCard(card2);

                    int score = hand.Score(true);
                    Assert.AreEqual(1, hand.AceCount());
                    Assert.AreEqual(21, score);
                    Assert.IsTrue(hand.IsBlackjack());
                }
            }
        }

        [TestMethod]
        public void return_number_of_hidden_cards_in_hand()
        {
            int expectedVisibleCount = 0;
            List<bool> falseTrue = new List<bool>() { false, true };
            foreach (bool v1 in falseTrue)
            {
                foreach (bool v2 in falseTrue)
                {
                    foreach (bool v3 in falseTrue)
                    {
                        mockCard1.Setup(c => c.Visible).Returns(v1);
                        mockCard2.Setup(c => c.Visible).Returns(v2);
                        mockCard3.Setup(c => c.Visible).Returns(v3);
                        IHand hand = new Hand();
                        ICard card1 = mockCard1.Object;
                        ICard card2 = mockCard2.Object;
                        ICard card3 = mockCard3.Object;
                        hand.AddCard(card1);
                        hand.AddCard(card2);
                        hand.AddCard(card3);
                        expectedVisibleCount = 0;
                        if (!v1) expectedVisibleCount++;
                        if (!v2) expectedVisibleCount++;
                        if (!v3) expectedVisibleCount++;
                        Assert.AreEqual(expectedVisibleCount, hand.HiddenCardsCount());
                    }
                }
            }            
        }



    }
}
