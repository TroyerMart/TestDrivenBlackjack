using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack.IO;
using Training_BlackJack;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class BlackjackGame_IntegrationTests
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
        public void play_a_simulated_game_player_wins()
        {
            //Dependencies.randomSeeds.ConstantValue = 1234;
            Dependencies.randomInstance.ConstantValue = new Random(1234);

            mockConsoleIO.LoadReadValue("S");

            BlackjackGame game = new BlackjackGame();
            string playerName = "Billy Joel";
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer(playerName);
            IDeck deck = BlackjackOperations.GetAndShuffleNewDeck();
            game.play(deck, dealer, player);

            int i = 0;
            Assert.AreEqual($"Player {playerName}, Score=18", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Eight of Hearts,Queen of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=4", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Four of Clubs", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Enter Action H)it, S)tand, B)ust:", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName} Action=Stand", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Hit", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Four of Clubs,Two of Spades", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Hit", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Four of Clubs,Two of Spades,Ace of Diamonds", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Hit", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Busted", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Four of Clubs,Two of Spades,Ace of Diamonds,Six of Spades", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{playerName} wins!", mockConsoleIO.GetConsoleWrites()[i++]);

            Assert.AreEqual($"Player {playerName}, Score=18", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Eight of Hearts,Queen of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=23", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Ten of Hearts,Four of Clubs,Two of Spades,Ace of Diamonds,Six of Spades", mockConsoleIO.GetConsoleWrites()[i++]);
            Dependencies.consoleIO.reset();
            //Dependencies.randomSeeds.reset();
            Dependencies.randomInstance.reset();
        }

        [TestMethod]
        public void play_a_simulated_game_dealer_wins()
        {
            //Dependencies.randomSeeds.ConstantValue = 1235;
            Dependencies.randomInstance.ConstantValue = new Random(1235);
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();

            mockConsoleIO.LoadReadValue("H");
            mockConsoleIO.LoadReadValue("S");

            BlackjackGame game = new BlackjackGame();
            string playerName = "Billy Joel";
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer(playerName);
            IDeck deck = BlackjackOperations.GetAndShuffleNewDeck();
            game.play(deck, dealer, player);

            int i = 0;
            Assert.AreEqual($"Player {playerName}, Score=14", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Spades,Four of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=4", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Four of Clubs", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Enter Action H)it, S)tand, B)ust:", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName} Action=Hit", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName}, Score=20", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Spades,Four of Hearts,Six of Spades", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Enter Action H)it, S)tand, B)ust:", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName} Action=Stand", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Hit", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Four of Clubs,Seven of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Stand", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer wins!", mockConsoleIO.GetConsoleWrites()[i++]);

            Assert.AreEqual($"Player {playerName}, Score=20", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Spades,Four of Hearts,Six of Spades", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=21", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"King of Clubs,Four of Clubs,Seven of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Dependencies.consoleIO.reset();
            Dependencies.randomSeeds.reset();
            Dependencies.randomInstance.reset();
        }

        [TestMethod]
        public void play_a_simulated_game_player_busts()
        {
            //Dependencies.randomSeeds.ConstantValue = 1236;
            Dependencies.randomInstance.ConstantValue = new Random(1236);
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();

            mockConsoleIO.LoadReadValue("H");

            BlackjackGame game = new BlackjackGame();
            string playerName = "Mickey";
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer(playerName);
            IDeck deck = BlackjackOperations.GetAndShuffleNewDeck();
            game.play(deck, dealer, player);

            int i = 0;
            Assert.AreEqual($"Player {playerName}, Score=14", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Hearts,Four of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=10", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Queen of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Enter Action H)it, S)tand, B)ust:", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName} Action=Hit", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName} Action=Busted", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName}, Score=24", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Hearts,Four of Hearts,Ten of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer wins!", mockConsoleIO.GetConsoleWrites()[i++]);

            Assert.AreEqual($"Player {playerName}, Score=24", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Hearts,Four of Hearts,Ten of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=16", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Six of Hearts,Queen of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Dependencies.consoleIO.reset();
            //Dependencies.randomSeeds.reset();
            Dependencies.randomInstance.reset();
        }

        [TestMethod]
        public void play_a_simulated_game_dealer_busts()
        {
            //Dependencies.randomSeeds.ConstantValue = 1236;
            Dependencies.randomInstance.ConstantValue = new Random(1236);

            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();

            mockConsoleIO.LoadReadValue("S");

            BlackjackGame game = new BlackjackGame();
            string playerName = "Mickey";
            Dealer dealer = new Dealer();
            IPlayer player = new HumanPlayer(playerName);
            IDeck deck = BlackjackOperations.GetAndShuffleNewDeck();
            game.play(deck, dealer, player);

            int i = 0;
            Assert.AreEqual($"Player {playerName}, Score=14", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Hearts,Four of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=10", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Queen of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Enter Action H)it, S)tand, B)ust:", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Player {playerName} Action=Stand", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Hit", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Action=Busted", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{Hand.FACEDOWN},Queen of Hearts,Ten of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"{playerName} wins!", mockConsoleIO.GetConsoleWrites()[i++]);

            Assert.AreEqual($"Player {playerName}, Score=14", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Jack of Hearts,Four of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Dealer Score Showing=26", mockConsoleIO.GetConsoleWrites()[i++]);
            Assert.AreEqual($"Six of Hearts,Queen of Hearts,Ten of Hearts", mockConsoleIO.GetConsoleWrites()[i++]);
            Dependencies.consoleIO.reset();
            //Dependencies.randomSeeds.reset();
            Dependencies.randomInstance.reset();
        }

        [TestCleanup]
        public void CleanUp()
        {   // reset Dependencies after test
            typeof(Training_BlackJack.Dependencies).TypeInitializer.Invoke(null, null);
        }
    }
}
