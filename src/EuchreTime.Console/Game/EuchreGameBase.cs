using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Bidding;
using EuchreTime.Core.Game;
using EuchreTime.Core.Hand;
using EuchreTime.Core.Helpers;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Console.Game
{
    public abstract class EuchreGameBase : IEuchreGame
    {
        private readonly IGameState _gameState;
        private readonly IHandleFirstRoundBidding _firstRoundBidder;
        private readonly IHandleSecondRoundBidding _secondRoundBidder;
        private readonly IPlayHands _handPlayer;
        private readonly IRenderCards _cardRenderer;
        private readonly IInputHelper _inputHelper;
        private readonly ICardHelper _cardHelper;
        private readonly IRenderSuits _suitRenderer;

        protected EuchreGameBase(
            IGameState gameState,
            IHandleFirstRoundBidding firstRoundBidder,
            IHandleSecondRoundBidding secondRoundBidder,
            IPlayHands handPlayer,
            IRenderCards cardRenderer,
            IInputHelper inputHelper,
            ICardHelper cardHelper,
            IRenderSuits suitRenderer
        )
        {
            _gameState = gameState;
            _firstRoundBidder = firstRoundBidder;
            _secondRoundBidder = secondRoundBidder;
            _handPlayer = handPlayer;
            _cardRenderer = cardRenderer;
            _inputHelper = inputHelper;
            _cardHelper = cardHelper;
            _suitRenderer = suitRenderer;
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

                System.Console.WriteLine("The following card was turned up:");

                var turnedUpRenderedCard = _cardRenderer.RenderCards(new List<ICard> { _gameState.TurnedUpCard }, new CardRenderingOptions());

                System.Console.WriteLine(turnedUpRenderedCard);

                System.Console.WriteLine("Your cards are the following:");

                var humanPlayerCards = _cardRenderer.RenderCards(_gameState.Dealer.Cards, new CardRenderingOptions
                {
                    ShowIndexes = true
                });

                System.Console.WriteLine(humanPlayerCards);

                //first round bid
                _firstRoundBidder.AskEachPlayerAboutTheTopCard(_gameState, _shouldHumanOrderUp, _aiOrderUpCallback, _humanChooseDiscard);

                //if no takers, second round bidding
                if (_gameState.Trump == null)
                {
                    _secondRoundBidder.AskEachPlayerAboutTrump(_gameState, _humanChooseSuit, _trumpSelectedCallback);
                }

                //play a hand only if trump has been chosen
                if (_gameState.Trump != null)
                {
                    //return the current player to the left of the dealer
                    _gameState.SetCurrentPlayerToLeftOfDealer();

                    _handPlayer.PlayHand(_gameState, _chooseHumanCard, _aiChoseCardCallback);
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

        private void _trumpSelectedCallback(ISuit suit, IPlayer player)
        {
            System.Console.WriteLine($"{suit.Name} has been chosen as the trump suit by {player.Name}.");
        }

        private ISuit _humanChooseSuit()
        {
            var suitsToChooseFrom = _cardHelper.GetSuitsToChooseFrom(_gameState);

            var renderedSuits = _suitRenderer.RenderSuits(suitsToChooseFrom, true);

            System.Console.WriteLine(renderedSuits);

            var keyPressed =
                    _inputHelper.GetValidInput(
                        "Choose a suit or pass:",
                        new List<char> { '1', '2', '3', 'p' }
                    );

            if (keyPressed == 'P')
            {
                System.Console.WriteLine($"{_gameState.CurrentPlayer.Name} has decided to pass.");
                return null;
            }
            else
            {
                var trumpIndex = int.Parse(keyPressed.ToString());
                return suitsToChooseFrom[trumpIndex - 1];
            }
        }

        private ICard _humanChooseDiscard()
        {
            var keyPressed =
                            _inputHelper.GetValidInput(
                                "Enter the card number to discard:",
                                new List<char> { '1', '2', '3', '4', '5' }
                            );

            var index = int.Parse(keyPressed.ToString());

            return _gameState.CurrentPlayer.Cards[index];
        }

        private void _aiOrderUpCallback(bool didAiOrderUp)
        {
            var action = didAiOrderUp ? "to order up trump" : "to pass";

            System.Console.WriteLine($"{_gameState.CurrentPlayer.Name} decided {action}.");
        }

        private bool _shouldHumanOrderUp()
        {
            var keyPressed =
            _inputHelper.GetValidInput(
                $"Do you wish to order up {_gameState.Dealer.Name} with the {_gameState.TurnedUpCard.Rank.Name} of {_gameState.TurnedUpCard.Suit.Name}?",
                new List<char> { 'y', 'n' }
            );

            return keyPressed == 'Y';
        }

        private void _aiChoseCardCallback(ICard chosenCard)
        {
            System.Console.WriteLine($"{_gameState.CurrentPlayer.Name} played the {chosenCard.Rank.Name} of {chosenCard.Suit.Name}:");

            var renderedCards = _cardRenderer.RenderCards(_gameState.CurrentHand.Select(x => x.Card).ToList(), new CardRenderingOptions());

            System.Console.WriteLine(renderedCards);
        }

        private ICard _chooseHumanCard()
        {
            var renderedCards = _cardRenderer.RenderCards(_gameState.CurrentPlayer.Cards, new CardRenderingOptions
            {
                ShowIndexes = true
            });

            System.Console.WriteLine(renderedCards);

            var keyPressed =
                _inputHelper.GetValidInput(
                    "It is your turn, which card would you like to play?",
                    _cardHelper.GetValidIndexes(_gameState.LeadSuit, _gameState.CurrentPlayer.Cards)
                );

            var indexOfCard = int.Parse(keyPressed.ToString()) - 1;

            var chosenCard = _gameState.CurrentPlayer.Cards[indexOfCard];

            System.Console.WriteLine($"You played the {chosenCard.Rank.Name} of {chosenCard.Suit.Name}:");

            return chosenCard;
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
