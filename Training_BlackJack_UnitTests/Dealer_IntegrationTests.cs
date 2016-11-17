using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack.Interfaces;
using BlackJack;
using Training_BlackJack;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class Dealer_IntegrationTests
    {
        [TestMethod]
        public void next_action_busted_because_total_is_greater_than_21()
        {
            IPlayer dealer = new Dealer();
            IHand playerHand = new Hand();
            Card card0 = new Card(Suit.Spades, Rank.King);
            playerHand.AddCard(card0);

            Card card1 = new Card(Suit.Clubs, Rank.King);
            Card card2 = new Card(Suit.Clubs, Rank.Seven);
            Card card3 = new Card(Suit.Clubs, Rank.Five);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            dealer.AddCardToHand(card3);
            PlayerAction action = dealer.NextAction(playerHand);

            Assert.AreEqual(PlayerAction.Busted, action);
        }

        [TestMethod]
        public void next_action_stand_because_total_is_greater_than_hard_16()
        {
            IPlayer dealer = new Dealer();
            Hand playerHand = new Hand();
            Card card0 = new Card(Suit.Spades, Rank.King);
            playerHand.AddCard(card0);

            Card card1 = new Card(Suit.Clubs, Rank.King);
            Card card2 = new Card(Suit.Clubs, Rank.Seven);
            dealer.GetHand().AddCard(card1);
            dealer.GetHand().AddCard(card2);
            PlayerAction action = dealer.NextAction(playerHand);

            Assert.AreEqual(PlayerAction.Stand, action);
        }

        [TestMethod]
        public void next_action_stand_because_total_is_greater_than_soft_17()
        {
            IPlayer dealer = new Dealer();
            Hand playerHand = new Hand();
            Card card0 = new Card(Suit.Spades, Rank.King);
            playerHand.AddCard(card0);

            Card card1 = new Card(Suit.Clubs, Rank.Ace);
            Card card2 = new Card(Suit.Clubs, Rank.Seven);
            dealer.GetHand().AddCard(card1);
            dealer.GetHand().AddCard(card2);
            PlayerAction action = dealer.NextAction(playerHand);

            Assert.AreEqual(PlayerAction.Stand, action);
        }

        [TestMethod]
        public void next_action_hit_because_total_is_less_than_soft_18()
        {
            IPlayer dealer = new Dealer();
            Hand playerHand = new Hand();
            Card card0 = new Card(Suit.Spades, Rank.King);
            playerHand.AddCard(card0);

            Card card1 = new Card(Suit.Clubs, Rank.Ace);
            Card card2 = new Card(Suit.Clubs, Rank.Five);
            dealer.GetHand().AddCard(card1);
            dealer.GetHand().AddCard(card2);
            PlayerAction action = dealer.NextAction(playerHand);

            Assert.AreEqual(PlayerAction.Hit, action);
        }

        [TestMethod]
        public void next_action_hit_because_total_is_less_than_17()
        {
            IPlayer dealer = new Dealer();
            Hand playerHand = new Hand();
            Card card0 = new Card(Suit.Spades, Rank.King);
            playerHand.AddCard(card0);

            Card card1 = new Card(Suit.Clubs, Rank.Two);
            Card card2 = new Card(Suit.Clubs, Rank.Seven);
            dealer.GetHand().AddCard(card1);
            dealer.GetHand().AddCard(card2);
            PlayerAction action = dealer.NextAction(playerHand);

            Assert.AreEqual(PlayerAction.Hit, action);
        }

        /******************* test sequences *******************/
        [TestMethod]
        public void draw_cards_until_busted_because_total_is_greater_than_21()
        {
            IPlayer dealer = new Dealer();
            Hand playerHand = new Hand();
            Card card0 = new Card(Suit.Spades, Rank.King);
            playerHand.AddCard(card0);

            Card card1 = new Card(Suit.Clubs, Rank.King);
            Card card2 = new Card(Suit.Hearts, Rank.Five);
            Card card3 = new Card(Suit.Spades, Rank.Seven);

            dealer.GetHand().AddCard(card1);
            PlayerAction action1 = dealer.NextAction(playerHand);
            dealer.GetHand().AddCard(card2);
            PlayerAction action2 = dealer.NextAction(playerHand);
            dealer.GetHand().AddCard(card3);
            PlayerAction action3 = dealer.NextAction(playerHand);

            Assert.AreEqual(PlayerAction.Hit, action1);
            Assert.AreEqual(PlayerAction.Hit, action2);
            Assert.AreEqual(PlayerAction.Busted, action3);
        }


    }
}
