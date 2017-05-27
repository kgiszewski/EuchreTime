using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Bidding;
using EuchreTime.Core.Game;
using EuchreTime.Core.Hand;
using EuchreTime.Core.Helpers;
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
        private readonly IRenderSuits _suitRenderer;

        protected EuchreGameBase(
            IGameState gameState,
            IHandleFirstRoundBidding firstRoundBidder,
            IHandleSecondRoundBidding secondRoundBidder,
            IPlayHands handPlayer,
            IRenderCards cardRenderer,
            IInputHelper inputHelper,
            IRenderSuits suitRenderer
        )
        {
            _gameState = gameState;
            _firstRoundBidder = firstRoundBidder;
            _secondRoundBidder = secondRoundBidder;
            _handPlayer = handPlayer;
            _cardRenderer = cardRenderer;
            _inputHelper = inputHelper;
            _suitRenderer = suitRenderer;
        }

        public void Play()
        {
            //register observers
            _firstRoundBidder.OnAiOrderedUpDealer += _firstRoundBidderOnOnAiOrderedUpDealer;
            _secondRoundBidder.OnAiChoseTrump += _trumpSelectedCallback;
            _handPlayer.OnAiPlayedCard += _aiPlayedCardCallback;
            _handPlayer.OnTrickCompleted += _handleOnTrickCompleted;
            _handPlayer.OnHandCompleted += _handleOnHandCompleted;

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

                var humanPlayerCards = _cardRenderer.RenderCards(_gameState.Players.First(x => x.IsHuman).Cards, new CardRenderingOptions
                {
                    ShowIndexes = true
                });

                System.Console.WriteLine(humanPlayerCards);

                //first round bid
                _firstRoundBidder.AskEachPlayerAboutTheTopCard(_gameState, _shouldHumanOrderUp, _humanChooseDiscard);

                //if no takers, second round bidding
                if (_gameState.Trump == null)
                {
                    _secondRoundBidder.AskEachPlayerAboutTrump(_gameState, _humanChooseSuit);
                }

                //play a hand only if trump has been chosen
                if (_gameState.Trump != null)
                {
                    //return the current player to the left of the dealer
                    _gameState.SetCurrentPlayerToLeftOfDealer();

                    _handPlayer.PlayHand(_gameState, _chooseHumanCard);
                }

                //advance the deal, reset state as-needed
                _gameState.SetCurrentPlayerToLeftOfDealer();
                _gameState.Dealer = _gameState.CurrentPlayer;

                _gameState.Trump = null;
                _gameState.TurnedUpCard = null;
                _gameState.OrderingUpPlayer = null;
                _gameState.Kitty.Clear();
                _gameState.CurrentHand.Clear();
                _gameState.LeadSuit = null;
                _clearPlayerCards();
            }
        }

        private void _handleOnHandCompleted(object sender, HandCompletedEventArgs e)
        {
            System.Console.WriteLine($"Team {e.HandWinningTeamNumber} won the hand with {e.WinningTeamTricksWon} tricks and scored {e.PointsScored} points.");

            System.Console.WriteLine("== Current Score ==");
            System.Console.WriteLine($"Team 1: {e.TeamOnePoints}");
            System.Console.WriteLine($"Team 2: {e.TeamTwoPoints}");
        }

        private void _handleOnTrickCompleted(object sender, TrickCompletedEventArgs e)
        {
            System.Console.WriteLine($"{e.TrickWinner.Name} (team {e.TrickWinner.TeamNumber}) won the trick.");
        }

        private void _firstRoundBidderOnOnAiOrderedUpDealer(object sender, AiOrderedUpDealerEventArgs e)
        {
            var action = e.ShouldOrderUp ? "to order up trump" : "to pass";

            System.Console.WriteLine($"{e.Player.Name} decided {action}.");
        }

        private void _trumpSelectedCallback(object sender, AiChoseTrumpEventArgs e)
        {
            System.Console.WriteLine($"{e.Suit.Name} has been chosen as the trump suit by {e.Player.Name}.");
        }

        private ISuit _humanChooseSuit()
        {
            var suitsToChooseFrom = CardHelper.GetSuitsToChooseFrom(_gameState.TurnedUpCard.Suit);

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

            return _gameState.CurrentPlayer.Cards[index - 1];
        }

        private bool _shouldHumanOrderUp()
        {
            var message = (!_gameState.Dealer.IsHuman)
                ? $"Do you want to order up {_gameState.Dealer.Name} with the {_gameState.TurnedUpCard.Rank.Name} of {_gameState.TurnedUpCard.Suit.Name}?"
                : $"Do you want to pick up the {_gameState.TurnedUpCard.Rank.Name} of {_gameState.TurnedUpCard.Suit.Name}?";

            var keyPressed =
            _inputHelper.GetValidInput(
                message,
                new List<char> { 'y', 'n' }
            );

            return keyPressed == 'Y';
        }

        private void _aiPlayedCardCallback(object sender, AiPlayedCardEventArgs e)
        {
            System.Console.WriteLine($"{_gameState.CurrentPlayer.Name} played the {e.Card.Rank.Name} of {e.Card.Suit.Name}:");

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

            System.Console.WriteLine($"Ordering up player: {_gameState.OrderingUpPlayer.Name}");

            if (_gameState.LeadSuit != null)
            {
                System.Console.WriteLine($"Lead: {_gameState.LeadSuit.Name}");
            }
            else
            {
                System.Console.WriteLine($"Lead: TBD");
            }

            System.Console.WriteLine($"Trump: {_gameState.Trump.Name}");

            foreach (var player in _gameState.Players)
            {
                System.Console.WriteLine($"{player.Name} (Team {player.TeamNumber}) Tricks: {player.TricksTaken}");
            }

            var keyPressed =
                _inputHelper.GetValidInput(
                    "It is your turn, which card would you like to play?",
                    CardHelper.GetValidIndexes(_gameState.LeadSuit, _gameState.Trump, _gameState.CurrentPlayer.Cards)
                );

            var indexOfCard = int.Parse(keyPressed.ToString()) - 1;

            var chosenCard = _gameState.CurrentPlayer.Cards[indexOfCard];

            System.Console.WriteLine($"You played the {chosenCard.Rank.Name} of {chosenCard.Suit.Name}:");

            renderedCards = _cardRenderer.RenderCards(new List<ICard> {chosenCard},  new CardRenderingOptions());

            System.Console.WriteLine(renderedCards);

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
