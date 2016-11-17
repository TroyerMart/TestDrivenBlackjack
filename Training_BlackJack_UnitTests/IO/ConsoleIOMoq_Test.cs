using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack;
using Training_BlackJack.Interfaces;
using Moq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Training_BlackJack_UnitTests
{
    [TestClass]
    public class ConsoleIOTest
    {
        private Mock<IConsoleIO> mockConsoleIO = new Mock<IConsoleIO>();
        private Stack<string> _log = new Stack<string>();

        [TestMethod]
        public void allow_read_from_console()
        {
            string inputLine = "read in a line";
            _log.Push(inputLine);
            //ConsoleIO io = new ConsoleIO();
            //_mockMyService.Setup(s => s.ComputeIt(It.IsAny<int>())).Returns((int x) => x * x * x);
            mockConsoleIO.Setup(io => io.ReadLine()).Returns(_log.Pop());

            var lineRead = mockConsoleIO.Object.ReadLine();

            Assert.AreEqual(inputLine, lineRead);

        }

        [TestMethod]
        public void allow_output_to_console()
        {
            string output = "output data";
            mockConsoleIO.Setup(io => io.WriteLine(It.IsAny<string>())).Callback((string x) => _log.Push(x));
            mockConsoleIO.Object.WriteLine(output);
            string value = _log.Pop();
            Assert.AreEqual<string>(output, value);
        }

 
    }
}
