using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_BlackJack.Interfaces
{
    public interface IConsoleIO
    {
        void WriteLine(string value);
        void Write(string value);

        string ReadLine();
        int Read();

        ConsoleKeyInfo ReadKey();

        string PromptForString(string message);
        char PromptForChar(string message);

        void EnableDebug(bool enabled);
    }
}
