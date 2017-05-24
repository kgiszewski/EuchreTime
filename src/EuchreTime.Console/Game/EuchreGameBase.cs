using System.Linq;
using EuchreTime.Console.Bidding;
using EuchreTime.Console.Hand;
using EuchreTime.Core.Game;

namespace EuchreTime.Console.Game
{
    public abstract class EuchreGameBase : IEuchreGame
    {
        private readonly IGameState _gameState;
        private readonly IHandleFirstRoundBidding _firstRoundBidder;
        private readonly IHandleSecondRoundBidding _secondRoundBidder;
        private readonly IPlayHands _handPlayer;

        protected EuchreGameBase(
            IGameState gameState,
            IHandleFirstRoundBidding firstRoundBidder,
            IHandleSecondRoundBidding secondRoundBidder,
            IPlayHands handPlayer
        )
        {
            _gameState = gameState;
            _firstRoundBidder = firstRoundBidder;
            _secondRoundBidder = secondRoundBidder;
            _handPlayer = handPlayer;
        }

        public void Play()
        {
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;
            System.Console.WriteLine("Welcome to Euchre Time!");
            System.Console.WriteLine($"There are {_gameState.Players.Count(x => x.IsHuman)} human players and {_gameState.Players.Count(x => !x.IsHuman)} computer players.");

            //main game loop here
            while (!_gameState.WinningConditions.HasAnyTeamWon(_gameState))
            {
                _gameState.Dealer.Deal(_gameState);
                
                System.Console.WriteLine($"{_gameState.Dealer.Name} has dealt the cards.");

                //first round bid
                _firstRoundBidder.AskEachPlayerAboutTheTopCard(_gameState);

                //if no takers, second round bidding
                if (_gameState.Trump == null)
                {
                    _secondRoundBidder.AskEachPlayerAboutTrump(_gameState);
                }

                //play a hand only if trump has been chosen
                if (_gameState.Trump != null)
                {
                    //return the current player to the left of the dealer
                    _gameState.SetCurrentPlayerToLeftOfDealer();

                    _handPlayer.PlayHand(_gameState);
                }

                //advance the deal, reset state as-needed
                _gameState.SetCurrentPlayerToLeftOfDealer();
                _gameState.Dealer = _gameState.CurrentPlayer;

                _gameState.Trump = null;
                _gameState.TurnedUpCard = null;
                _gameState.OrderingUpPlayer = null;
                _gameState.Kitty.Clear();
                _gameState.CurrentHand.Clear();
                _clearPlayerCards();
            }
        }

        private void _clearPlayerCards()
        {
            foreach (var player in _gameState.Players)
            {
                player.Cards.Clear();
            }
        }
    }
}
