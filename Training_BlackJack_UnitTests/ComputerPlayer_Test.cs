using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack.Interfaces;
using Training_BlackJack;
using BlackJack;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class ComputerPlayer_Test
    {
        [TestMethod]
        public void return_player_name_default()
        {
            IPlayer player = new ComputerPlayer();
            Assert.AreEqual(ComputerPlayer.DEFAULT_NAME, player.GetName());
        }
        [TestMethod]
        public void return_player_name_provided()
        {
            string name = "Kenny";
            IPlayer player = new ComputerPlayer(name);
            Assert.AreEqual(name, player.GetName());
        }

        [TestMethod]
        public void add_card_to_hand()
        {
            IPlayer player = new ComputerPlayer();
            Card card = new Card(Suit.Clubs, Rank.Ace);
            player.GetHand().AddCard(card);
            Assert.AreEqual(1, player.GetHand().Size());
        }

        [TestMethod]
        public void reset_hand()
        {
            IPlayer player = new ComputerPlayer();
            Card card = new Card(Suit.Clubs, Rank.Ace);
            player.GetHand().AddCard(card);
            Assert.AreEqual(1, player.GetHand().Size());
            player.ClearHand();
            Assert.AreEqual(0, player.GetHand().Size());
        }

        [TestMethod]
        public void next_action_busted_because_total_is_greater_than_21()
        {
            IPlayer player = new ComputerPlayer();
            Hand dealerHand = new Hand();
            Card card0 = new Card(Suit.Spades, Rank.King);
            dealerHand.AddCard(card0);

            Card card1 = new Card(Suit.Clubs, Rank.King);
            Card card2 = new Card(Suit.Clubs, Rank.Seven);
            Card card3 = new Card(Suit.Clubs, Rank.Five);
            player.GetHand().AddCard(card1);
            player.GetHand().AddCard(card2);
            player.GetHand().AddCard(card3);
            PlayerAction action = player.NextAction(dealerHand);

            Assert.AreEqual(PlayerAction.Busted, action);
        }

        [TestMethod]
        public void next_action_hit_because_score_less_than_probable_dealers_score()
        {
            IPlayer player = new ComputerPlayer();
            Hand dealerHand = new Hand();
            Card dealerCard1 = new Card(Suit.Spades, Rank.Ten);
            dealerCard1.Visible = false;
            Card dealerCard2 = new Card(Suit.Hearts, Rank.Eight);
            dealerHand.AddCard(dealerCard1);
            dealerHand.AddCard(dealerCard2);

            Card playerCard1 = new Card(Suit.Clubs, Rank.King);
            Card playerCard2 = new Card(Suit.Diamonds, Rank.Five);
            player.GetHand().AddCard(playerCard1);
            player.GetHand().AddCard(playerCard2);
            PlayerAction action = player.NextAction(dealerHand);
            Assert.AreEqual(PlayerAction.Hit, action);
        }

        [TestMethod]
        public void next_action_stand_because_score_greater_or_equal_than_probable_dealers_score()
        {
            IPlayer player = new ComputerPlayer();
            Hand dealerHand = new Hand();
            Card dealerCard1 = new Card(Suit.Spades, Rank.Eight);
            dealerCard1.Visible = false;
            Card dealerCard2 = new Card(Suit.Hearts, Rank.Seven);
            dealerHand.AddCard(dealerCard1);
            dealerHand.AddCard(dealerCard2);

            Card playerCard1 = new Card(Suit.Clubs, Rank.King);
            Card playerCard2 = new Card(Suit.Diamonds, Rank.Eight);
            player.GetHand().AddCard(playerCard1);
            player.GetHand().AddCard(playerCard2);
            PlayerAction action = player.NextAction(dealerHand);
            Assert.AreEqual(PlayerAction.Stand, action);
        }

        [TestMethod]
        public void next_action_stay_when_total_is_17_or_more()
        {
            IPlayer player = new ComputerPlayer();
            Dealer dealer = new Dealer();
            Card card0 = new Card(Suit.Spades, Rank.King);
            Card card1 = new Card(Suit.Spades, Rank.Six);
            dealer.AddCardToHand(card0);
            dealer.AddCardToHand(card1);

            Card card2 = new Card(Suit.Clubs, Rank.King);
            Card card3 = new Card(Suit.Clubs, Rank.Seven);
            player.AddCardToHand(card1);
            player.AddCardToHand(card2);
            PlayerAction action = player.NextAction(dealer.GetHand());

            Assert.AreEqual(PlayerAction.Stand, action);
        }

    }
}
