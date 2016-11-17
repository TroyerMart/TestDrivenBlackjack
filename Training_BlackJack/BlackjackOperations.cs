using BlackJack;
using Training_BlackJack.Exceptions;
using Training_BlackJack.Interfaces;
using static Training_BlackJack.BlackjackGame;

namespace Training_BlackJack
{
    public class BlackjackOperations
    {
        public const int MAXSCORE = 21;
        IConsoleIO _io;

        public BlackjackOperations(IConsoleIO io) // what needs to be passed in??
        {
            _io = io;
        }

        public static IDeck GetAndShuffleNewDeck()
        {   
            IDeck newDeck = new Deck();
            newDeck.Shuffle();
            return newDeck;
        }

        public void DealInitialHands(IDeck deck, Dealer dealer, IPlayer player)
        {
            player.AddCardToHand(deck.DealTopCard(false));
            dealer.AddCardToHand(deck.DealTopCard(false));
            player.AddCardToHand(deck.DealTopCard(true));
            dealer.AddCardToHand(deck.DealTopCard(true));
        }

        public void DisplayResults(Dealer dealer, IPlayer player, GameResult result)
        {
            string displayResults = GetGameResultsDisplay(dealer, player, result);
            _io.WriteLine(displayResults);
        }

        public void InteractWithPlayers(IDeck deck, Dealer dealer, IPlayer player, PlayerAction lastDealerAction, PlayerAction lastPlayerAction)
        {
            lastPlayerAction = InteractWithPlayer(deck, player, dealer);
            if (lastPlayerAction != PlayerAction.Busted)
            {
                lastDealerAction = InteractWithDealer(deck, dealer, player);
            }
            else
            {
                lastDealerAction = PlayerAction.Stand;
            }
        }

        public string GetGameResultsDisplay(IPlayer dealer, IPlayer player, GameResult result)
        {
            string displayResults = "";
            switch (result)
            {
                case GameResult.DealerBlackjack:
                    {
                        displayResults += $"{dealer.GetName()} has Blackjack!";
                    }
                    break;
                case GameResult.PlayerBlackjack:
                    {
                        displayResults += $"{player.GetName()} has Blackjack!";
                    }
                    break;
                case GameResult.DealerWin:
                    {
                        displayResults += $"{dealer.GetName()} wins!";
                    }
                    break;
                case GameResult.PlayerWin:
                    {
                        displayResults += $"{player.GetName()} wins!";
                    }
                    break;
                case GameResult.Push:
                    {
                        displayResults += "This round was a Push";
                    }
                    break;
            }
            return displayResults;
        }

        public string GetHandHeaderMessage(IPlayer player)
        {
            string header;
            if (player.GetPlayerType() == "HumanPlayer")
            {
                header = $"Player {player.GetName()} has: ";
            }
            else if (player.GetPlayerType() == "Dealer")
            {
                header = $"Dealer {player.GetName()} has: ";
            }
            else
            {
                header = $"Computer Player {player.GetName()} has: ";
            }
            return header;
        }
        public string GetScoreMessage(IPlayer player, bool showFinalScore = false)
        {
            string header;
            int score;
            if (player.GetPlayerType() == "HumanPlayer")
            {
                score = player.GetHand().Score(true);   // always show score
                header = $"Player {player.GetName()}, Score={score}";
            }
            else if (player.GetPlayerType() == "Dealer")
            {
                score = player.GetHand().Score(showFinalScore);
                header = $"Dealer Score Showing={score}";
            }
            else
            {
                score = player.GetHand().Score(showFinalScore);
                header = $"Player {player.GetName()}, Score={score}";
            }
            return header;
        }
        public string GetHandMessage(IHand handToShow, bool showHiddenCards)
        {
            string display = handToShow.DisplayHand(showHiddenCards);
            return display;
        }

        public string GetActionMessage(IPlayer player, PlayerAction action)
        {
            string showAction = "";
            if (player.GetPlayerType() == "HumanPlayer")
            {
                showAction = $"Player {player.GetName()} Action={action}";
            }
            else if (player.GetPlayerType() == "Dealer")
            {
                showAction = $"Dealer Action={action}";
            }
            else
            {
                showAction = $"Computer Player {player.GetName()} Action={action}";
            }
            return showAction;
        }

        public GameResult GetFinalResults(Dealer dealer, IPlayer player, PlayerAction lastDealerAction = PlayerAction.Stand, PlayerAction lastPlayerAction = PlayerAction.Stand)
        {
            IHand dealerHand = dealer.GetHand();
            IHand playerHand = player.GetHand();
            int dealerScore = dealerHand.Score(true);
            int playerScore = playerHand.Score(true);

            if (playerScore > MAXSCORE || lastPlayerAction == PlayerAction.Busted)
            {
                return GameResult.DealerWin;
            }
            if (dealerScore > MAXSCORE || lastDealerAction == PlayerAction.Busted)
            {
                return GameResult.PlayerWin;
            }
            if (dealerHand.IsBlackjack() && playerHand.IsBlackjack())
            {
                return GameResult.Push;
            }
            if (dealerHand.IsBlackjack())
            {
                return GameResult.DealerBlackjack;
            }
            if (playerHand.IsBlackjack())
            {
                return GameResult.PlayerBlackjack;
            }
            if (dealerScore == playerScore)
            {
                return GameResult.Push;
            }
            if (dealerScore > playerScore)
            {
                return GameResult.DealerWin;
            }
            else
            {
                return GameResult.PlayerWin;
            }
        }

        internal PlayerAction InteractWithDealer(IDeck deck, Dealer dealer, IPlayer player)
        {
            PlayerAction action;
            string actionDisplay;
            do
            {
                action = dealer.NextAction(player.GetHand());
                actionDisplay = GetActionMessage(dealer, action);
                DisplayMessage(actionDisplay);
                if (action == PlayerAction.Hit)
                {
                    ICard card = deck.DealTopCard(true);
                    dealer.AddCardToHand(card);
                    int score = dealer.GetHand().Score(true);
                    if (score > MAXSCORE)
                    {
                        action = PlayerAction.Busted;
                        actionDisplay = GetActionMessage(dealer, action);
                        DisplayMessage(actionDisplay);
                    }
                    string handDisplay = GetHandMessage(dealer.GetHand(), false);
                    DisplayMessage(handDisplay);
                }
            } while (action != PlayerAction.Busted && action != PlayerAction.Stand);
            return action;
        }

        internal PlayerAction InteractWithPlayer(IDeck deck, IPlayer player, Dealer dealer)
        {
            PlayerAction action;
            string actionDisplay;
            do
            {
                action = player.NextAction(dealer.GetHand());
                actionDisplay = GetActionMessage(player, action);
                DisplayMessage(actionDisplay);
                if (action == PlayerAction.Hit)
                {
                    ICard card = deck.DealTopCard(true);
                    player.AddCardToHand(card);
                    int score = player.GetHand().Score(true);
                    if (score > MAXSCORE)
                    {
                        action = PlayerAction.Busted;
                        actionDisplay = GetActionMessage(player, action);
                        DisplayMessage(actionDisplay);
                    }
                    string scoreDisplay = GetScoreMessage(player);
                    DisplayMessage(scoreDisplay);
                    string handDisplay = GetHandMessage(player.GetHand(), true);
                    DisplayMessage(handDisplay);
                }
            } while (action != PlayerAction.Busted && action != PlayerAction.Stand);
            return action;
        }

        public void DisplayMessage(string message)
        {
            try
            {
                _io.WriteLine(message);
            } catch
            {
                throw new GameException("Error writing to IO");
            }
        }

        public static int GetCardValue(Card card)
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
