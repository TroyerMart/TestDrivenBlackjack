using BlackJack;
using Training_BlackJack.Interfaces;

namespace Training_BlackJack
{
    public class BlackjackGame
    {
        private IConsoleIO _io;
        private BlackjackOperations ops;

        public enum GameResult
        {
            DealerBlackjack,
            PlayerBlackjack,
            DealerWin,
            PlayerWin,
            Push
        }

        public BlackjackGame()
        {
            _io = Dependencies.consoleIO.make();
            ops = new BlackjackOperations(_io); // pass in dependencies??
        }

        public BlackjackGame(IConsoleIO io)
        {
            _io = io;
            ops = new BlackjackOperations(_io); // pass in dependencies??
        }

        public GameResult play(IDeck deck, Dealer dealer, IPlayer player)
        {
            //IDeck deck;
            PlayerAction lastDealerAction = PlayerAction.Invalid;
            PlayerAction lastPlayerAction = PlayerAction.Invalid;
            string output;
            //deck = ops.GetAndShuffleNewDeck();

            ops.DealInitialHands(deck, dealer, player);

            output = ops.GetScoreMessage(player);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(player.GetHand(), true);   // player can see their own cards
            ops.DisplayMessage(output);
            output = ops.GetScoreMessage(dealer, false);           // hide dealer's hidden cards from score
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(dealer.GetHand(), false);  // hide dealer's face down cards
            ops.DisplayMessage(output);

            ops.InteractWithPlayers(deck, dealer, player, lastDealerAction, lastPlayerAction);

            GameResult result = ops.GetFinalResults(dealer, player, lastDealerAction, lastPlayerAction);

            ops.DisplayResults(dealer, player, result);

            output = ops.GetScoreMessage(player);
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(player.GetHand(), true);   // player can see their own cards
            ops.DisplayMessage(output);
            output = ops.GetScoreMessage(dealer, true);            // reveal all cards
            ops.DisplayMessage(output);
            output = ops.GetHandMessage(dealer.GetHand(), true);   // reveal all cards
            ops.DisplayMessage(output);

            return result;
        }

        public static int GetCardValue(ICard card)
        {
            int value = 0;
            switch (card.rank)
            {
                case Rank.Ace:
                    {
                        value = 1;
                        break;
                    }
                case Rank.Ten:
                case Rank.Jack:
                case Rank.Queen:
                case Rank.King:
                    {
                        value = 10;
                        break;
                    }
                default:
                    {
                        value = (int)card.rank + 1;
                        break;
                    }
            }
            return value;
        }

    }
}
