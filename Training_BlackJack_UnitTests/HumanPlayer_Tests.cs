using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack.Interfaces;
using Training_BlackJack;
using BlackJack;
using Training_BlackJack.IO;
using Moq;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class HumanPlayer_Tests
    {
        ConsoleIOMock mockConsoleIO = new ConsoleIOMock();

        [TestMethod]
        public void return_player_name_default()
        {
            IPlayer player = new HumanPlayer();
            Assert.AreEqual(HumanPlayer.DEFAULT_NAME, player.GetName());
        }
        [TestMethod]
        public void return_player_name_provided()
        {
            string name = "Sharky";
            IPlayer player = new HumanPlayer(name);
            Assert.AreEqual(name, player.GetName());
        }

        [TestMethod]
        public void add_card_to_hand()
        {
            IPlayer player = new HumanPlayer();
            Mock<ICard> mockCard = new Mock<ICard>();
            player.AddCardToHand(mockCard.Object);
            Assert.AreEqual(1, player.GetHand().Size());
        }

        [TestMethod]
        public void add_cards_to_hand()
        {
            IPlayer player = new HumanPlayer();
            Mock<ICard> mockCard1 = new Mock<ICard>();
            Mock<ICard> mockCard2 = new Mock<ICard>();
            player.AddCardToHand(mockCard1.Object);
            player.AddCardToHand(mockCard2.Object);
            Assert.AreEqual(2, player.GetHand().Size());
        }

        [TestMethod]
        public void reset_hand()
        {
            IHand hand = new Hand();
            Mock<ICard> mockCard1 = new Mock<ICard>();
            Mock<ICard> mockCard2 = new Mock<ICard>();
            hand.AddCard(mockCard1.Object);
            hand.AddCard(mockCard2.Object);
            IPlayer player = new HumanPlayer(hand);
            int sizeBefore = player.GetHand().Size();
            player.ClearHand();
            int sizeAfter = player.GetHand().Size();
            Assert.AreEqual(2, sizeBefore);
            Assert.AreEqual(0, sizeAfter);
        }

        [TestMethod]
        public void parse_action_valid_single_char_typed_lowercase()
        {
            Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
            IPlayer player = new HumanPlayer(mockConsoleIO.Object);
            mockConsoleIO.SetupSequence(io => io.PromptForString(It.IsAny<string>()))
                .Returns("h")
                .Returns("s")
                .Returns("b");
            PlayerAction action1 = player.NextAction(null);
            PlayerAction action2 = player.NextAction(null);
            PlayerAction action3 = player.NextAction(null);

            Assert.AreEqual(PlayerAction.Hit, action1);
            Assert.AreEqual(PlayerAction.Stand, action2);
            Assert.AreEqual(PlayerAction.Busted, action3);
        }

        [TestMethod]
        public void parse_action_valid_single_char_typed()
        {
            Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
            IPlayer player = new HumanPlayer(mockConsoleIO.Object);
            mockConsoleIO.SetupSequence(io => io.PromptForString(It.IsAny<string>()))
                .Returns("B")
                .Returns("H")
                .Returns("S");
            PlayerAction action1 = player.NextAction(null);
            PlayerAction action2 = player.NextAction(null);
            PlayerAction action3 = player.NextAction(null);

            Assert.AreEqual(PlayerAction.Busted, action1);
            Assert.AreEqual(PlayerAction.Hit, action2);
            Assert.AreEqual(PlayerAction.Stand, action3);
        }
        
        [TestMethod]
        public void parse_action_valid_char_typed_with_whitespace()
        {
            Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
            IPlayer player = new HumanPlayer(mockConsoleIO.Object);
            mockConsoleIO.SetupSequence(io => io.PromptForString(It.IsAny<string>()))
                .Returns("  B ")
                .Returns("S  ")
                .Returns("  h");
            PlayerAction action1 = player.NextAction(null);
            PlayerAction action2 = player.NextAction(null);
            PlayerAction action3 = player.NextAction(null);

            Assert.AreEqual(PlayerAction.Busted, action1);
            Assert.AreEqual(PlayerAction.Stand, action2);
            Assert.AreEqual(PlayerAction.Hit, action3);
        }

        [TestMethod]
        public void parse_action_valid_char_typed_additional_chars()
        {
            Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
            IPlayer player = new HumanPlayer(mockConsoleIO.Object);
            mockConsoleIO.SetupSequence(io => io.PromptForString(It.IsAny<string>()))
                .Returns("Bx y ")
                .Returns("SB")
                .Returns("H?");
            PlayerAction action1 = player.NextAction(null);
            PlayerAction action2 = player.NextAction(null);
            PlayerAction action3 = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Busted, action1);
            Assert.AreEqual(PlayerAction.Stand, action2);
            Assert.AreEqual(PlayerAction.Hit, action3);
        }

        [TestMethod]
        public void parse_action_whitespace_or_null_typed()
        {
            Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
            IPlayer player = new HumanPlayer(mockConsoleIO.Object);
            mockConsoleIO.SetupSequence(io => io.PromptForString(It.IsAny<string>()))
                .Returns(null)
                .Returns("")
                .Returns("   ");
            PlayerAction action1 = player.NextAction(null);
            PlayerAction action2 = player.NextAction(null);
            PlayerAction action3 = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Invalid, action1);
            Assert.AreEqual(PlayerAction.Invalid, action2);
            Assert.AreEqual(PlayerAction.Invalid, action3);
        }

        [TestMethod]
        public void parse_action_invalid_char_typed()
        {
            Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
            IPlayer player = new HumanPlayer(mockConsoleIO.Object);
            mockConsoleIO.SetupSequence(io => io.PromptForString(It.IsAny<string>()))
                .Returns("&^")
                .Returns("F")
                .Returns(" X ");
            PlayerAction action1 = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Invalid, action1);
        }

        [TestMethod]
        public void parse_action_invalid_char_typed_before_valid_one_entered()
        {
            Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
            IPlayer player = new HumanPlayer(mockConsoleIO.Object);
            mockConsoleIO.SetupSequence(io => io.PromptForString(It.IsAny<string>()))
                .Returns("&^")
                .Returns("F")
                .Returns(" X ")
                .Returns("S");
            PlayerAction action1 = player.NextAction(null);

            Assert.AreEqual(PlayerAction.Stand, action1);
        }

        /*************** Custom Mock IO Tests *******************/
        [TestMethod]
        public void parse_action_valid_single_char_typed_lowercase_custom()
        {
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            mockConsoleIO.LoadReadValue("h");
            mockConsoleIO.LoadReadValue("s");
            mockConsoleIO.LoadReadValue("b");

            IPlayer player = new HumanPlayer();
            PlayerAction action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Hit, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Stand, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Busted, action);
            Assert.AreEqual(3, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void parse_action_valid_single_char_typed_custom()
        {
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            mockConsoleIO.LoadReadValue("B");
            mockConsoleIO.LoadReadValue("H");
            mockConsoleIO.LoadReadValue("S");

            IPlayer player = new HumanPlayer();
            PlayerAction action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Busted, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Hit, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Stand, action);
            Assert.AreEqual(3, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void parse_action_valid_char_typed_with_whitespace_custom()
        {
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            mockConsoleIO.LoadReadValue("  B ");
            mockConsoleIO.LoadReadValue("S  ");
            mockConsoleIO.LoadReadValue("  h");

            IPlayer player = new HumanPlayer();
            PlayerAction action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Busted, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Stand, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Hit, action);
            Assert.AreEqual(3, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void parse_action_valid_char_typed_additional_chars_custom()
        {
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            mockConsoleIO.LoadReadValue("Bx y ");
            mockConsoleIO.LoadReadValue("SB");
            mockConsoleIO.LoadReadValue("H?");

            IPlayer player = new HumanPlayer();
            PlayerAction action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Busted, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Stand, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Hit, action);
            Assert.AreEqual(3, mockConsoleIO.GetConsoleWrites().Count);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void parse_action_whitespace_or_null_typed_custom()
        {
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            mockConsoleIO.LoadReadValue(null);
            mockConsoleIO.LoadReadValue("");
            mockConsoleIO.LoadReadValue("  ");

            IPlayer player = new HumanPlayer();
            PlayerAction action;
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Invalid, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Invalid, action);
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Invalid, action);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void parse_action_invalid_char_typed_custom()
        {
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            mockConsoleIO.LoadReadValue("&^");
            mockConsoleIO.LoadReadValue("F");
            mockConsoleIO.LoadReadValue(" X ");

            IPlayer player = new HumanPlayer();
            PlayerAction action;
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Invalid, action);

            Dependencies.consoleIO.reset();
        }

        [TestMethod]
        public void parse_action_invalid_char_typed_before_valid_one_entered_custom()
        {
            Dependencies.consoleIO.ConstantValue = mockConsoleIO;
            mockConsoleIO.ResetConsole();
            mockConsoleIO.LoadReadValue("&^");
            mockConsoleIO.LoadReadValue("F");
            mockConsoleIO.LoadReadValue(" X ");
            mockConsoleIO.LoadReadValue("S");

            IPlayer player = new HumanPlayer();
            PlayerAction action;
            action = player.NextAction(null);
            Assert.AreEqual(PlayerAction.Stand, action);

            Dependencies.consoleIO.reset();
        }

        [TestCleanup]
        public void CleanUp()
        {   // reset Dependencies after test
            typeof(Training_BlackJack.Dependencies).TypeInitializer.Invoke(null, null);
        }
    }
}
