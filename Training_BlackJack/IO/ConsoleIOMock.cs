using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack.IO
{
    public class ConsoleIOMock : IConsoleIO
    {
        private List<string> _consoleReads = new List<string>();
        private List<string> _consoleWrites = new List<string>();
        private bool _debugEnabled = false;
        private bool _newLine = true;

        public ConsoleIOMock()
        {
        }

        public ConsoleIOMock(bool enableDebug)
        {
            _debugEnabled = enableDebug;
        }

        public void EnableDebug(bool enabled)
        {
            _debugEnabled = enabled;
        }

        public int Read()
        {   // simulates reading one character at a time from the input stream until end of line
            int singleValue = 0;
            string lineRead = ReadLine();
            if (lineRead == null)
            {
                return singleValue;
            }
            char charRead = lineRead.First();
            singleValue = (int)charRead;
            var restOfLine = lineRead.Remove(0, 1);
            if (restOfLine.Count() > 0)
            {   // if there are still characters to read, put it back
                _consoleReads.Insert(0, restOfLine);
            }
            if (_debugEnabled)
            {
                Debug.WriteLine($"READ: '{singleValue}' from '{lineRead}' ");
            }
            return singleValue;
        }

        public ConsoleKeyInfo ReadKey()
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            if (_consoleReads.Count == 0)
            {
                return null;
            }
            string lineRead = _consoleReads[0];
            _consoleReads.RemoveAt(0);
            if (_debugEnabled)
            {
                Debug.WriteLine($"READLINE: {lineRead}");
            }
            return lineRead;
        }

        public void Write(string value)
        {   
            if (_newLine || _consoleWrites.Count == 0)
            {
                _consoleWrites.Add(value);
            }
            else
            {
                string currentLine = _consoleWrites.Last();
                _consoleWrites.RemoveAt(_consoleWrites.Count - 1);
                _consoleWrites.Add(currentLine + value);
            }
            _newLine = false;
            if (_debugEnabled)
            {
                Debug.WriteLine($"WRITE: {value}");
            }
        }

        public void WriteLine(string value)
        {
            _consoleWrites.Add(value);
            _newLine = true;
            if (_debugEnabled)
            {
                Debug.WriteLine($"WRITELINE: {value}");
            }
        }

        public void ClearWriteConsole()
        {
            _consoleWrites.Clear();
        }

        public void ClearReadsQueue()
        {
            _consoleReads.Clear();
        }

        public void ResetConsole()
        {
            ClearReadsQueue();
            ClearWriteConsole();
        }

        public List<string> GetConsoleWrites(int numberToGet = 0)
        {
            if (numberToGet == 0 || numberToGet > _consoleWrites.Count) {
                numberToGet = _consoleWrites.Count;
            }
            return _consoleWrites.GetRange(0, numberToGet);
        }

        public List<string> GetConsoleReads(int numberToGet = 0)
        {
            if (numberToGet == 0 || numberToGet > _consoleReads.Count)
            {
                numberToGet = _consoleReads.Count;
            }
            return _consoleReads.ToList().GetRange(0, numberToGet);
        }

        public void LoadReadValue(string line)
        {
            List<string> lines = new List<string>();
            lines.Add(line);
            LoadReadValues(lines);
        }
        public void LoadReadValues(List<string> lines)
        {
            foreach (string line in lines)
            {
                _consoleReads.Add(line);
            }
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
