using System;
using System.Linq;
using System.Text;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Decks;

namespace EuchreTime.Console.Rendering
{
    public class SuitRenderer : IRenderSuits
    {
        private readonly StringBuilder _sb;
        private static int _numberRows = 2;

        public SuitRenderer()
        {
            _sb = new StringBuilder();
        }

        public string RenderSuits(IGameState gameState)
        {
            var newDeck = new EuchreDeck();
            newDeck.Initialize();

            var allSuits = newDeck.Cards.Select(x => x.Suit).Distinct();

            var suitsToChooseFrom = allSuits.Where(x => x.Name != gameState.TurnedUpCard.Suit.Name).ToList();

            for (var row = 0; row < _numberRows; row++)
            {
                var index = 0;

                foreach (var suit in suitsToChooseFrom)
                {
                    index++;

                    switch (row)
                    {
                        case 0:
                            _sb.Append($"{suit.Symbol} ");
                            break;

                        case 1:
                            _sb.Append($"{index} ");
                            break;
                    }
                }

                _sb.Append(Environment.NewLine);
            }

            return _sb.ToString();
        }
    }
}
