using System.Diagnostics;
using System.Linq;
using Training_BlackJack;
using Training_BlackJack.Interfaces;
using static Training_BlackJack.BlackjackGame;

namespace BlackJack
{
    class Program
    {
        //static ConsoleIO io = new ConsoleIO(true);

        static void Main(string[] args)
        {
            IConsoleIO io = Dependencies.consoleIO.make();
            int gamesWon = 0;
            int totalGames = 0;

            //Process proc1 = Process.Start("cmd.exe");
            //proc1.StandardInput.WriteLine("Hello");

            //Process proc = new Process();
            //proc.StartInfo.FileName = "cmd.exe";
            //proc.Start();

            string playerName = io.PromptForString("Enter you name:");
            
            bool playAgain = true;
            do
            {
                BlackjackGame game = new BlackjackGame();
                totalGames++;
                Dealer dealer = new Dealer();
                IPlayer player = new HumanPlayer(playerName);
                IDeck deck = BlackjackOperations.GetAndShuffleNewDeck();

                GameResult result = game.play(deck, dealer, player);
                if (result == GameResult.PlayerWin || result == GameResult.PlayerBlackjack)
                {
                    gamesWon++;
                }

                string askPlayAgain = io.PromptForString("Play again?");
                playAgain = askPlayAgain.Length > 0 && (askPlayAgain.ToUpper().First() == 'Y');
            } while (playAgain);
            io.WriteLine("Thanks for playing!");
            io.WriteLine($"You won {gamesWon} out of {totalGames} games");

            // use when debugging to keep window open
            io.Read();

        }
    }
}
