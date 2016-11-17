using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack;
using Training_BlackJack.IO;
using Training_BlackJack.Interfaces;
using BlackJack;
using static Training_BlackJack.BlackjackGame;
using Moq;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class BlackjackGame_Tests
    {
        ConsoleIOMock mockConsoleIO;
        BlackjackOperations ops;

        [TestInitialize]
        public void Setup()
        {
            mockConsoleIO = new ConsoleIOMock();
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            ops = new BlackjackOperations(mockConsoleIO);
        }

        [TestMethod]
        public void get_hand_header_message_for_player()
        {
            string name = "John";
            Mock<IPlayer> mockPlayer = new Mock<IPlayer>();
            mockPlayer.Setup(p => p.GetPlayerType()).Returns("HumanPlayer");
            mockPlayer.Setup(p => p.GetName()).Returns(name);
            string output = ops.GetHandHeaderMessage(mockPlayer.Object);
            Assert.AreEqual($"Player {name} has: ", output);
        }

        [TestMethod]
        public void get_hand_header_message_for_dealer()
        {
            string name = "Slick";
            Mock<IPlayer> mockPlayer = new Mock<IPlayer>();
            mockPlayer.Setup(p => p.GetPlayerType()).Returns("Dealer");
            mockPlayer.Setup(p => p.GetName()).Returns(name);
            string output = ops.GetHandHeaderMessage(mockPlayer.Object);
            Assert.AreEqual($"Dealer {name} has: ", output);
        }


        /*************** Custom Mock IO Tests *******************/

        [TestMethod]
        public void show_players_cards_custom()
        {
            string output;
            string name = "John";
            IPlayer player = new HumanPlayer(name);
            ICard card1 = new Card(Suit.Clubs, Rank.Ace);
            ICard card2 = new Card(Suit.Hearts, Rank.Two);
            player.AddCardToHand(card1);
            player.AddCardToHand(card2);

            output = ops.GetHandHeaderMessage(player);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(player.GetHand(), true);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Player {name} has: ", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual("Ace of Clubs,Two of Hearts", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }


        [TestMethod]
        public void show_dealers_cards_hide_not_visible()
        {
            string output;
            string name = "Swifty";
            IPlayer dealer = new Dealer(name);
            ICard card1 = new Card(Suit.Clubs, Rank.Jack);
            card1.Visible = false;
            ICard card2 = new Card(Suit.Hearts, Rank.Seven);
            ICard card3 = new Card(Suit.Hearts, Rank.Four);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            dealer.AddCardToHand(card3);

            output = ops.GetHandHeaderMessage(dealer);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(dealer.GetHand(), false);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Dealer {name} has: ", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual($"{Hand.FACEDOWN},Seven of Hearts,Four of Hearts", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void show_dealers_cards_show_not_visible()
        {
            string name = "Swifty";
            string output;
            IPlayer dealer = new Dealer(name);
            ICard card1 = new Card(Suit.Diamonds, Rank.King);
            ICard card2 = new Card(Suit.Spades, Rank.Jack);
            ICard card3 = new Card(Suit.Hearts, Rank.Ace);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            dealer.AddCardToHand(card3);

            output = ops.GetHandHeaderMessage(dealer);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(dealer.GetHand(), false);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Dealer {name} has: ", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual("King of Diamonds,Jack of Spades,Ace of Hearts", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void show_players_score_and_hand()
        {
            int score;
            string name = "John";
            string output;
            IPlayer player = new HumanPlayer(name);
            ICard card1 = new Card(Suit.Clubs, Rank.Ace);
            card1.Visible = false;
            ICard card2 = new Card(Suit.Hearts, Rank.Two);
            player.AddCardToHand(card1);
            player.AddCardToHand(card2);
            score = 3;
            output = ops.GetScoreMessage(player);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(player.GetHand(), true);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Player {player.GetName()}, Score={score}", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual("Ace of Clubs,Two of Hearts", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            mockConsoleIO.ClearWriteConsole();
            ICard card3 = new Card(Suit.Hearts, Rank.Nine);
            player.AddCardToHand(card3);
            score = 12;
            output = ops.GetScoreMessage(player);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(player.GetHand(), true);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Player {player.GetName()}, Score={score}", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual("Ace of Clubs,Two of Hearts,Nine of Hearts", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            mockConsoleIO.ClearWriteConsole();
            ICard card4 = new Card(Suit.Spades, Rank.Eight);
            player.AddCardToHand(card4);
            score = 20;
            output = ops.GetScoreMessage(player);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(player.GetHand(), true);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Player {player.GetName()}, Score={score}", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual("Ace of Clubs,Two of Hearts,Nine of Hearts,Eight of Spades", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void show_dealers_score_and_hand()
        {
            string output;
            int score;
            string name = "John";
            IPlayer dealer = new Dealer(name);
            ICard card1 = new Card(Suit.Clubs, Rank.Nine);
            card1.Visible = false;
            ICard card2 = new Card(Suit.Hearts, Rank.Two);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            score = 2;
            output = ops.GetScoreMessage(dealer);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(dealer.GetHand(), false);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Dealer Score Showing={score}", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual($"{Hand.FACEDOWN},Two of Hearts", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            mockConsoleIO.ClearWriteConsole();
            ICard card3 = new Card(Suit.Hearts, Rank.Five);
            dealer.AddCardToHand(card3);
            score = 7;
            output = ops.GetScoreMessage(dealer);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(dealer.GetHand(), false);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Dealer Score Showing={score}", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual($"{Hand.FACEDOWN},Two of Hearts,Five of Hearts", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            mockConsoleIO.ClearWriteConsole();
            ICard card4 = new Card(Suit.Spades, Rank.Eight);
            dealer.AddCardToHand(card4);
            score = 15;
            output = ops.GetScoreMessage(dealer);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(dealer.GetHand(), false);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Dealer Score Showing={score}", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual($"{Hand.FACEDOWN},Two of Hearts,Five of Hearts,Eight of Spades", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual(2, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void verify_initial_hands_dealt_alternately()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            Deck deck = new BlackJack.Deck();

            ICard card0 = deck.PeekCard(0);
            ICard card1 = deck.PeekCard(1);
            ICard card2 = deck.PeekCard(2);
            ICard card3 = deck.PeekCard(3);

            ops.DealInitialHands(deck, dealer, player);
            ICard playerCard0 = player.GetHand().GetCards()[0];
            ICard playerCard1 = player.GetHand().GetCards()[1];
            ICard dealerCard0 = dealer.GetHand().GetCards()[0];
            ICard dealerCard1 = dealer.GetHand().GetCards()[1];
            Assert.IsTrue(card0.Equals(playerCard0));
            Assert.IsTrue(card1.Equals(dealerCard0));
            Assert.IsTrue(card2.Equals(playerCard1));
            Assert.IsTrue(card3.Equals(dealerCard1));
        }

        [TestMethod]
        public void verify_initial_hands_first_card_dealt_facedown()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            Deck deck = new BlackJack.Deck();

            ICard card0 = deck.PeekCard(0);
            ICard card1 = deck.PeekCard(1);
            ICard card2 = deck.PeekCard(2);
            ICard card3 = deck.PeekCard(3);

            ops.DealInitialHands(deck, dealer, player);
            ICard playerCard0 = player.GetHand().GetCards()[0];
            ICard playerCard1 = player.GetHand().GetCards()[1];
            ICard dealerCard0 = dealer.GetHand().GetCards()[0];
            ICard dealerCard1 = dealer.GetHand().GetCards()[1];
            Assert.IsFalse(playerCard0.Visible);
            Assert.IsFalse(dealerCard0.Visible);
            Assert.IsTrue(playerCard1.Visible);
            Assert.IsTrue(dealerCard1.Visible);
        }

        [TestMethod]
        public void get_final_results_dealer_and_player_have_blackjack()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.Ace);
            ICard card2 = new Card(Suit.Diamonds, Rank.Ten);
            ICard card3 = new Card(Suit.Spades, Rank.King);
            ICard card4 = new Card(Suit.Hearts, Rank.Ace);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            player.AddCardToHand(card3);
            player.AddCardToHand(card4);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.Push, result);
        }

        [TestMethod]
        public void get_final_results_dealer_has_blackjack()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.Ace);
            ICard card2 = new Card(Suit.Diamonds, Rank.Ten);
            ICard card3 = new Card(Suit.Spades, Rank.Seven);
            ICard card4 = new Card(Suit.Hearts, Rank.Ace);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            player.AddCardToHand(card3);
            player.AddCardToHand(card4);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.DealerBlackjack, result);
        }
        [TestMethod]
        public void get_final_results_player_has_blackjack()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.King);
            ICard card2 = new Card(Suit.Diamonds, Rank.Ten);
            ICard card3 = new Card(Suit.Spades, Rank.King);
            ICard card4 = new Card(Suit.Hearts, Rank.Ace);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            player.AddCardToHand(card3);
            player.AddCardToHand(card4);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.PlayerBlackjack, result);
        }
        [TestMethod]
        public void get_final_results_dealer_wins()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.King);
            ICard card2 = new Card(Suit.Diamonds, Rank.Ten);
            ICard card3 = new Card(Suit.Spades, Rank.King);
            ICard card4 = new Card(Suit.Hearts, Rank.Nine);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            player.AddCardToHand(card3);
            player.AddCardToHand(card4);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.DealerWin, result);
        }
        [TestMethod]
        public void get_final_results_player_wins()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.Seven);
            ICard card2 = new Card(Suit.Diamonds, Rank.Ten);
            ICard card3 = new Card(Suit.Spades, Rank.King);
            ICard card4 = new Card(Suit.Hearts, Rank.Eight);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            player.AddCardToHand(card3);
            player.AddCardToHand(card4);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.PlayerWin, result);
        }
        [TestMethod]
        public void get_final_results_when_dealer_and_player_have_same_score_push()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.Queen);
            ICard card2 = new Card(Suit.Diamonds, Rank.Ten);
            ICard card3 = new Card(Suit.Spades, Rank.King);
            ICard card4 = new Card(Suit.Hearts, Rank.Jack);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            player.AddCardToHand(card3);
            player.AddCardToHand(card4);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.Push, result);
        }

        [TestMethod]
        public void get_final_results_when_dealer_busts()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.Queen);
            ICard card2 = new Card(Suit.Diamonds, Rank.Five);
            ICard card3 = new Card(Suit.Spades, Rank.Seven);
            ICard card4 = new Card(Suit.Hearts, Rank.Jack);
            ICard card5 = new Card(Suit.Clubs, Rank.Eight);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            dealer.AddCardToHand(card3);
            player.AddCardToHand(card4);
            player.AddCardToHand(card5);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.PlayerWin, result);
        }

        [TestMethod]
        public void get_final_results_when_player_busts()
        {
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer();
            ICard card1 = new Card(Suit.Clubs, Rank.Queen);
            ICard card2 = new Card(Suit.Diamonds, Rank.King);
            ICard card3 = new Card(Suit.Spades, Rank.Six);
            ICard card4 = new Card(Suit.Hearts, Rank.Jack);
            ICard card5 = new Card(Suit.Clubs, Rank.Eight);
            dealer.AddCardToHand(card1);
            dealer.AddCardToHand(card2);
            player.AddCardToHand(card3);
            player.AddCardToHand(card4);
            player.AddCardToHand(card5);
            GameResult result = ops.GetFinalResults(dealer, player);
            Assert.AreEqual(GameResult.DealerWin, result);
        }

        [TestMethod]
        public void display_results_dealer_blackjack()
        {
            IPlayer dealer = new Dealer();
            IPlayer player = new HumanPlayer("Joe");
            GameResult result = GameResult.DealerBlackjack;
            string display = ops.GetGameResultsDisplay(dealer, player, result);
            string expected = $"{dealer.GetName()} has Blackjack!";
            Assert.AreEqual(expected, display);
        }
        [TestMethod]
        public void display_results_player_blackjack()
        {
            IPlayer dealer = new Dealer();
            IPlayer player = new HumanPlayer("Joe");
            GameResult result = GameResult.PlayerBlackjack;
            string display = ops.GetGameResultsDisplay(dealer, player, result);
            string expected = $"{player.GetName()} has Blackjack!";
            Assert.AreEqual(expected, display);
        }
        [TestMethod]
        public void display_results_dealer_wins()
        {
            IPlayer dealer = new Dealer();
            IPlayer player = new HumanPlayer("Joe");
            GameResult result = GameResult.DealerWin;
            string display = ops.GetGameResultsDisplay(dealer, player, result);
            string expected = $"{dealer.GetName()} wins!";
            Assert.AreEqual(expected, display);
        }
        [TestMethod]
        public void display_results_player_wins()
        {
            IPlayer dealer = new Dealer();
            IPlayer player = new HumanPlayer("Joe");
            GameResult result = GameResult.PlayerWin;
            string display = ops.GetGameResultsDisplay(dealer, player, result);
            string expected = $"{player.GetName()} wins!";
            Assert.AreEqual(expected, display);
        }
        [TestMethod]
        public void display_results_push()
        {
            IPlayer dealer = new Dealer();
            IPlayer player = new HumanPlayer("Joe");
            GameResult result = GameResult.Push;
            string display = ops.GetGameResultsDisplay(dealer, player, result);
            string expected = $"This round was a Push";
            Assert.AreEqual(expected, display);
        }

        [TestMethod]
        public void show_player_action()
        {
            PlayerAction action;
            IPlayer player = new HumanPlayer("Joe");
            string output;

            action = PlayerAction.Busted;
            output = ops.GetActionMessage(player, action);
            ops.DisplayMessage(output);
            action = PlayerAction.Hit;
            output = ops.GetActionMessage(player, action);
            ops.DisplayMessage(output);
            action = PlayerAction.Stand;
            output = ops.GetActionMessage(player, action);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Player {player.GetName()} Action=Busted", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual($"Player {player.GetName()} Action=Hit", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual($"Player {player.GetName()} Action=Stand", mockConsoleIO.GetConsoleWrites()[2]);
            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void show_dealer_action()
        {
            PlayerAction action;
            IPlayer dealer = new Dealer();
            string output;

            action = PlayerAction.Busted;
            output = ops.GetActionMessage(dealer, action);
            ops.DisplayMessage(output);
            action = PlayerAction.Hit;
            output = ops.GetActionMessage(dealer, action);
            ops.DisplayMessage(output);
            action = PlayerAction.Stand;
            output = ops.GetActionMessage(dealer, action);
            ops.DisplayMessage(output);
            Assert.AreEqual($"Dealer Action=Busted", mockConsoleIO.GetConsoleWrites()[0]);
            Assert.AreEqual($"Dealer Action=Hit", mockConsoleIO.GetConsoleWrites()[1]);
            Assert.AreEqual($"Dealer Action=Stand", mockConsoleIO.GetConsoleWrites()[2]);
            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void get_value_of_card()
        {
            int expectedValue = 0;
            foreach (Rank rank1 in Enum.GetValues(typeof(Rank)))
            {
                Card card = new Card(Suit.Diamonds, rank1);
                int value = BlackjackGame.GetCardValue(card);
                if (card.rank > Rank.Nine)
                {
                    expectedValue = 10;
                }
                else
                {
                    expectedValue = (int)card.rank + 1;
                }

                Assert.AreEqual(expectedValue, value);
            }
        }


        [TestCleanup]
        public void CleanUp()
        {   // reset Dependencies after test
            typeof(Training_BlackJack.Dependencies).TypeInitializer.Invoke(null, null);
        }

    }
}
