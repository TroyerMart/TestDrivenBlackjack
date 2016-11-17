using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training_BlackJack.IO;
using System.Collections.Generic;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack_UnitTests.IO
{
    [TestClass]
    public class ConsoleIOMock_Tests
    {
        ConsoleIOMock mockConsoleIO = new ConsoleIOMock();

        [TestMethod]
        public void allow_writeline_to_console()
        {
            mockConsoleIO.ClearWriteConsole();

            string output1 = "output data 1";
            string output2 = "output data 2";

            mockConsoleIO.WriteLine(output1);
            int writesCount1 = mockConsoleIO.GetConsoleWrites().Count;
            mockConsoleIO.WriteLine(output2);
            int writesCount2 = mockConsoleIO.GetConsoleWrites().Count;

            List<string> writes = mockConsoleIO.GetConsoleWrites();

            Assert.AreEqual<string>(output1, writes[0]);
            Assert.AreEqual<string>(output2, writes[1]);
            Assert.AreEqual(1, writesCount1);
            Assert.AreEqual(2, writesCount2);
        }

        [TestMethod]
        public void allow_readline_from_console()
        {
            mockConsoleIO.ClearReadsQueue();
            string inputLine1 = "read in a line1";
            string inputLine2 = "read in a line2";
            List<string> linesToRead = new List<string>() { inputLine1, inputLine2 };
            mockConsoleIO.LoadReadValues(linesToRead);
            var readQueueCount0 = mockConsoleIO.GetConsoleReads().Count;

            var lineRead1 = mockConsoleIO.ReadLine();
            var readQueueCount1 = mockConsoleIO.GetConsoleReads().Count;
            var lineRead2 = mockConsoleIO.ReadLine();
            var readQueueCount2 = mockConsoleIO.GetConsoleReads().Count;

            Assert.AreEqual(inputLine1, lineRead1);
            Assert.AreEqual(inputLine2, lineRead2);
            Assert.AreEqual(linesToRead.Count, readQueueCount0);
            Assert.AreEqual(linesToRead.Count - 1, readQueueCount1);
            Assert.AreEqual(linesToRead.Count - 2, readQueueCount2);
        }

        [TestMethod]
        public void allow_writes_to_console()
        {
            mockConsoleIO.ClearWriteConsole();

            string output1 = "output data 1";

            foreach (char ch in output1)
            {
                mockConsoleIO.Write(ch.ToString());
            }
            int writesCount1 = mockConsoleIO.GetConsoleWrites().Count;           
            List<string> writes = mockConsoleIO.GetConsoleWrites();

            Assert.AreEqual<string>(output1, writes[0]);            
            Assert.AreEqual(1, writesCount1);            
        }

        [TestMethod]
        public void allow_multiple_character_reads_from_console()
        {
            // this is having trouble moving from one line to next.  Need some sort of EOL indicator implemented

            mockConsoleIO.ClearReadsQueue();
            string inputLine1 = "Mary had a";
            string inputLine2 = "little lamb";
            int ch1;
            List<string> linesToRead = new List<string>() { inputLine1, inputLine2 };
            mockConsoleIO.LoadReadValues(linesToRead);

            string lineRead1 = "";
            do
            {
                ch1 = mockConsoleIO.Read();
                lineRead1 += (char)ch1;
            } while (ch1 != 0);
            //var readQueueCount1 = mockConsoleIO.GetConsoleReads().Count;
            
            //Assert.AreEqual(inputLine1, lineRead1);
            //Assert.AreEqual(inputLine2, lineRead2);
            //Assert.AreEqual(linesToRead.Count, readQueueCount0);
            //Assert.AreEqual(linesToRead.Count - 1, readQueueCount1);
            //Assert.AreEqual(linesToRead.Count - 2, readQueueCount2);
        }

    }
}
