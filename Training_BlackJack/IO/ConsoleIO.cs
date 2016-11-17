using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack
{
    public class ConsoleIO : IConsoleIO
    {
        private bool _debugEnabled = false;

        public ConsoleIO()
        {
        }

        public ConsoleIO(bool enableDebug)
        {
            _debugEnabled = enableDebug;
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
        public int Read()
        {
            return Console.Read();
        }
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
            if (_debugEnabled)
            {
                Debug.WriteLine(value);
            }
        }

        public void Write(string value)
        {
            Console.Write(value);
            if (_debugEnabled)
            {
                Debug.Write(value);
            }
        }

        public void EnableDebug(bool enabled)
        {
            _debugEnabled = enabled;
        }

        public string PromptForString(string message)
        {
            WriteLine(message);
            string input = ReadLine();
            return input;
        }

        public char PromptForChar(string message)
        {
            WriteLine(message);
            int input = Read();
            return (char)input;
        }
    }
}
