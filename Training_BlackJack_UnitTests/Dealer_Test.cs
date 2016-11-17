using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack.Interfaces;
using Training_BlackJack;
using BlackJack;
using Moq;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class Dealer_Test
    {
        [TestMethod]
        public void return_player_name_default()
        {
            IPlayer dealer = new Dealer();
            Assert.AreEqual(Dealer.DEFAULT_NAME, dealer.GetName());
        }
        [TestMethod]
        public void return_player_name_provided()
        {
            string name = "Fast Eddy";
            IPlayer dealer = new Dealer(name);
            Assert.AreEqual(name, dealer.GetName());
        }

        [TestMethod]
        public void next_action_busted_because_total_is_greater_than_21()
        {  
            Mock<IHand> dealerHandMock = new Mock<IHand>();
            Mock<IHand> playerHandMock = new Mock<IHand>();
            dealerHandMock.Setup(h => h.Score(It.IsAny<bool>())).Returns(22);
            IPlayer dealer = new Dealer(dealerHandMock.Object);

            PlayerAction action = dealer.NextAction(playerHandMock.Object);

            Assert.AreEqual(PlayerAction.Busted, action);
        }
         
        [TestMethod]
        public void next_action_stand_because_total_is_greater_than_hard_16()
        {
            Mock<IHand> dealerHandMock = new Mock<IHand>();
            Mock<IHand> playerHandMock = new Mock<IHand>();
            dealerHandMock.SetupSequence(h => h.Score(It.IsAny<bool>()))
                .Returns(17)
                .Returns(18)
                .Returns(19)
                .Returns(20)
                .Returns(21);
            dealerHandMock.Setup(h => h.AceCount()).Returns(0);
            IPlayer dealer = new Dealer(dealerHandMock.Object);

            PlayerAction action17 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action18 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action19 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action20 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action21 = dealer.NextAction(playerHandMock.Object);

            Assert.AreEqual(PlayerAction.Stand, action17);
            Assert.AreEqual(PlayerAction.Stand, action18);
            Assert.AreEqual(PlayerAction.Stand, action19);
            Assert.AreEqual(PlayerAction.Stand, action20);
            Assert.AreEqual(PlayerAction.Stand, action21);
        }

        [TestMethod]
        public void next_action_stand_because_total_is_greater_than_soft_17()
        {
            Mock<IHand> dealerHandMock = new Mock<IHand>();
            Mock<IHand> playerHandMock = new Mock<IHand>();
            dealerHandMock.SetupSequence(h => h.Score(It.IsAny<bool>()))
                .Returns(18)
                .Returns(19)
                .Returns(20)
                .Returns(21);
            dealerHandMock.Setup(h => h.AceCount()).Returns(1);
            IPlayer dealer = new Dealer(dealerHandMock.Object);

            PlayerAction action18 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action19 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action20 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action21 = dealer.NextAction(playerHandMock.Object);

            Assert.AreEqual(PlayerAction.Stand, action18);
            Assert.AreEqual(PlayerAction.Stand, action19);
            Assert.AreEqual(PlayerAction.Stand, action20);
            Assert.AreEqual(PlayerAction.Stand, action21);
        }

        [TestMethod]
        public void next_action_hit_because_total_is_less_than_soft_18()
        {
            Mock<IHand> dealerHandMock = new Mock<IHand>();
            Mock<IHand> playerHandMock = new Mock<IHand>();
            dealerHandMock.SetupSequence(h => h.Score(It.IsAny<bool>()))
                .Returns(17)
                .Returns(16)
                .Returns(15)
                .Returns(14)
                .Returns(11)
                .Returns(10)
                .Returns(3);
            dealerHandMock.Setup(h => h.AceCount()).Returns(1);
            IPlayer dealer = new Dealer(dealerHandMock.Object);

            PlayerAction action17 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action16 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action15 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action14 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action11 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action10 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action3 = dealer.NextAction(playerHandMock.Object);

            Assert.AreEqual(PlayerAction.Hit, action17);
            Assert.AreEqual(PlayerAction.Hit, action16);
            Assert.AreEqual(PlayerAction.Hit, action15);
            Assert.AreEqual(PlayerAction.Hit, action14);
            Assert.AreEqual(PlayerAction.Hit, action11);
            Assert.AreEqual(PlayerAction.Hit, action10);
            Assert.AreEqual(PlayerAction.Hit, action3);
        }

        [TestMethod]
        public void next_action_hit_because_total_is_less_than_17()
        {
            Mock<IHand> dealerHandMock = new Mock<IHand>();
            Mock<IHand> playerHandMock = new Mock<IHand>();
            dealerHandMock.SetupSequence(h => h.Score(It.IsAny<bool>()))
                .Returns(16)
                .Returns(15)
                .Returns(14)
                .Returns(11)
                .Returns(10)
                .Returns(3);
            dealerHandMock.Setup(h => h.AceCount()).Returns(0);
            IPlayer dealer = new Dealer(dealerHandMock.Object);

            PlayerAction action16 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action15 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action14 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action11 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action10 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action3 = dealer.NextAction(playerHandMock.Object);

            Assert.AreEqual(PlayerAction.Hit, action16);
            Assert.AreEqual(PlayerAction.Hit, action15);
            Assert.AreEqual(PlayerAction.Hit, action14);
            Assert.AreEqual(PlayerAction.Hit, action11);
            Assert.AreEqual(PlayerAction.Hit, action10);
            Assert.AreEqual(PlayerAction.Hit, action3);
        }

        /******************* test sequences *******************/
        [TestMethod]
        public void draw_cards_until_busted_because_total_is_greater_than_21()
        {
            Mock<IHand> dealerHandMock = new Mock<IHand>();
            Mock<IHand> playerHandMock = new Mock<IHand>();
            dealerHandMock.SetupSequence(h => h.Score(It.IsAny<bool>()))
                .Returns(10)    //King
                .Returns(15)    //Five
                .Returns(22);   //Seven
            dealerHandMock.Setup(h => h.AceCount()).Returns(0);
            IPlayer dealer = new Dealer(dealerHandMock.Object);
            
            PlayerAction action1 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action2 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action3 = dealer.NextAction(playerHandMock.Object);

            Assert.AreEqual(PlayerAction.Hit, action1);
            Assert.AreEqual(PlayerAction.Hit, action2);
            Assert.AreEqual(PlayerAction.Busted, action3);
        }

        [TestMethod]
        public void draw_cards_with_one_ace_until_busted_because_total_is_greater_than_21()
        {
            Mock<IHand> dealerHandMock = new Mock<IHand>();
            Mock<IHand> playerHandMock = new Mock<IHand>();
            dealerHandMock.SetupSequence(h => h.Score(It.IsAny<bool>()))
                .Returns(5)    //Five
                .Returns(6)    //Ace
                .Returns(16)    //Jack
                .Returns(23);   //Seven
            dealerHandMock.SetupSequence(h => h.AceCount())
                .Returns(0)
                .Returns(1)
                .Returns(1)
                .Returns(1);
            IPlayer dealer = new Dealer(dealerHandMock.Object);

            PlayerAction action1 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action2 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action3 = dealer.NextAction(playerHandMock.Object);
            PlayerAction action4 = dealer.NextAction(playerHandMock.Object);

            Assert.AreEqual(PlayerAction.Hit, action1);
            Assert.AreEqual(PlayerAction.Hit, action2);
            Assert.AreEqual(PlayerAction.Hit, action3);
            Assert.AreEqual(PlayerAction.Busted, action4);
        }


    }
}
