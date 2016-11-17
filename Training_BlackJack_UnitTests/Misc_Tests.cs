using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class Misc_Tests
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
        public void SequenceTest()
        {
            mockCard1.SetupSequence(c => c.suit)
                .Returns(Suit.Clubs)
                .Returns(Suit.Diamonds)
                .Returns(Suit.Hearts)
                .Returns(Suit.Spades);

            Assert.AreEqual(Suit.Clubs, mockCard1.Object.suit);
            Assert.AreEqual(Suit.Diamonds, mockCard1.Object.suit);
            Assert.AreEqual(Suit.Hearts, mockCard1.Object.suit);
            Assert.AreEqual(Suit.Spades, mockCard1.Object.suit);
        }


    }
}
