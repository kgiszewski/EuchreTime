using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Console.Bidding
{
    public class SecondRoundBidder : IHandleSecondRoundBidding
    {
        private readonly IRenderSuits _suitRenderer;

        public SecondRoundBidder(IRenderSuits suitRenderer)
        {
            _suitRenderer = suitRenderer;
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

                    System.Console.WriteLine("Choose a suit or pass (1, 2, 3 or p):");

                    System.Console.WriteLine(renderedSuits);

                    var keyChosen = char.ToUpperInvariant(System.Console.ReadKey(true).KeyChar);

                    //TODO: handle input safely
                    if (keyChosen == 'p')
                    {
                        System.Console.WriteLine($"{gameState.CurrentPlayer.Name} has decided to pass.");
                    }
                    else
                    {
                        var trumpIndex = int.Parse(keyChosen.ToString());
                        trumpSelected = suitsToChooseFrom[trumpIndex - 1];
                    }
                }
                else
                {
                    trumpSelected = gameState.CurrentPlayer.PlayerStrategy.SecondRoundTrumpChoice(gameState);
                }

                if (trumpSelected != null)
                {
                    //set trump
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
