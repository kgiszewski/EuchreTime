using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Console.Rendering
{
    public class CardRenderer : IRenderCards
    {
        private static string CardHeader = "+--------+";
        private static string CardFooter = "+--------+";
        private static string EmptyRow =   "|        |";
        private static int CardRows = 8;
        private readonly StringBuilder _sb;

        public CardRenderer()
        {
            _sb = new StringBuilder();
        }

        public string RenderCards(List<ICard> cards, CardRenderingOptions renderingOptions)
        {
            _sb.Clear();

            for (var row = 0; row < CardRows; row++)
            {
                var index = 0;

                if (renderingOptions.OrderByRanks)
                {
                    cards = cards.OrderBy(x => x.Suit.Name).ThenByDescending(x => x.Rank.Value).ToList();
                }

                foreach (var card in cards)
                {
                    index++;

                    switch (row)
                    {
                        case 0:
                            _sb.Append(CardHeader);
                            break;
                        case 1:
                            _sb.Append($"|{card.Rank.Symbol}");
                            _sb.Append(card.Rank.Symbol.Length == 2 ? "      " : "       ");

                            _sb.Append("|");
                            break;
                        case 2:
                            _sb.Append($"|{card.Suit.Symbol}       |");
                            break;
                        case 4:
                            _sb.Append($"|       {card.Suit.Symbol}|");
                            break;
                        case 5:
                            _sb.Append("|");
                            _sb.Append(card.Rank.Symbol.Length == 2 ? "      " : "       ");
                            _sb.Append($"{card.Rank.Symbol}|");
                            break;
                        case 6:
                            _sb.Append(CardFooter);
                            break;
                        case 7:
                            if (renderingOptions.ShowIndexes)
                            {
                                _sb.Append($"    {index}     ");
                            }
                            break;
                        default:
                            _sb.Append(EmptyRow);
                            break;
                    }
                }

                _sb.Append(Environment.NewLine);
            }

            return _sb.ToString();
        }
    }
}
