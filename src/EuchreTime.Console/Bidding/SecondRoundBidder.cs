using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Console.Bidding
{
    public class SecondRoundBidder : IHandleSecondRoundBidding
    {
        private readonly IRenderSuits _suitRenderer;
        private readonly IInputHelper _inputHelper;

        public SecondRoundBidder(
            IRenderSuits suitRenderer,
            IInputHelper inputHelper
        )
        {
            _suitRenderer = suitRenderer;
            _inputHelper = inputHelper;
        }

        public void AskEachPlayerAboutTrump(IGameState gameState)
        {
            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                ISuit trumpSelected = null;

                if (gameState.CurrentPlayer.IsHuman)
                {
                    var suitsToChooseFrom = _getSuitsToChooseFrom(gameState);

                    var renderedSuits = _suitRenderer.RenderSuits(suitsToChooseFrom, true);

                    System.Console.WriteLine(renderedSuits);

                    var keyPressed =
                            _inputHelper.GetValidInput(
                                "Choose a suit or pass:",
                                new List<char> { '1', '2', '3', 'p' }
                            );

                    //TODO: handle input safely
                    if (keyPressed == 'P')
                    {
                        System.Console.WriteLine($"{gameState.CurrentPlayer.Name} has decided to pass.");
                    }
                    else
                    {
                        var trumpIndex = int.Parse(keyPressed.ToString());
                        trumpSelected = suitsToChooseFrom[trumpIndex - 1];
                    }
                }
                else
                {
                    trumpSelected = gameState.CurrentPlayer.PlayerStrategy.SecondRoundTrumpChoice(gameState);
                }

                if (trumpSelected != null)
                {
                    gameState.Trump = trumpSelected;

                    System.Console.WriteLine($"{trumpSelected.Name} has been chosen as the trump suit.");
                    break;
                }
            }
        }

        private List<ISuit> _getSuitsToChooseFrom(IGameState gameState)
        {
            var newDeck = new EuchreDeck();
            newDeck.Initialize();

            var allSuits = newDeck.Cards.Select(x => x.Suit).Distinct();

            return allSuits.Where(x => x.Name != gameState.TurnedUpCard.Suit.Name).ToList();
        }
    }
}
