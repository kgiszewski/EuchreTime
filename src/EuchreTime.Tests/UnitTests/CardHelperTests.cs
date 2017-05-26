using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Helpers;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Ranks;
using MechanicGrip.Core.Suits;
using NUnit.Framework;

namespace EuchreTime.Tests.UnitTests
{
    [TestFixture]
    public class CardHelperTests
    {
        [TestCase(1, 1, 1, "1,5")] //spades, spades
        [TestCase(1, 1, null, "1,2,3,4,5")] //spades, no lead
        [TestCase(2, 1, 1, "1,2,3,4,5")] //spades, spades
        [TestCase(2, 1, null, "1,2,3,4,5")] //spades, no lead
        public void CheckForValidIndexes(int handId, int trumpSuitId, int? leadSuitId, string expectedResultString)
        {
            var cards = _getCards(handId);
            var trumpSuit = _getSuit(trumpSuitId);
            var leadSuit = _getSuit(leadSuitId);

            Assert.AreEqual(5, cards.Count);

            Console.WriteLine($"Trump: {trumpSuit?.Name}");
            Console.WriteLine($"Lead: {leadSuit?.Name}");

            _dumpCardsToConsole(cards);

            var result = CardHelper.GetValidIndexes(leadSuit, trumpSuit, cards).OrderBy(x => x.ToString());
            var expectedResult = _getExpectedResultAsCharList(expectedResultString).OrderBy(x => x.ToString());

            Assert.AreEqual(expectedResult.Count(), result.Count());

            foreach (var item in expectedResult)
            {
                var actualItem = result.First(x => x.ToString() == item.ToString());

                Assert.AreEqual(item, actualItem);
            }
        }

        private List<char> _getExpectedResultAsCharList(string expected)
        {
            var result = new List<char>();

            var expectedSplit = expected.Split(new[] {","}, StringSplitOptions.None);

            foreach (var item in expectedSplit)
            {
                result.Add(Convert.ToChar(item));
            }

            return result;
        }

        private void _dumpCardsToConsole(List<ICard> cards)
        {
            var counter = 1;

            foreach (var card in cards)
            {
                Console.WriteLine($"{counter} - {card.Rank.Name} of {card.Suit.Name}");
                counter++;
            }
        } 
        
        private ISuit _getSuit(int? suit)
        {
            switch (suit)
            {
                case 1:
                    return Suit.Spades;
                case 2:
                    return Suit.Clubs;
                case 3:
                    return Suit.Hearts;
                case 4:
                    return Suit.Diamonds;
                default:
                    return null;
            }
        }

        private List<ICard> _getCards(int hand)
        {
            var euchreDeck = new EuchreDeck();
            euchreDeck.Initialize();

            switch (hand)
            {
                case 1:
                    return euchreDeck.Cards.Where(
                            (x => 
                                (x.Suit == Suit.Spades && x.Rank.Name == Rank.Jack)
                                || (x.Suit == Suit.Clubs && x.Rank.Name == Rank.Jack)
                                || (x.Suit == Suit.Clubs && x.Rank.Name == Rank.Ace)
                                || (x.Suit == Suit.Clubs && x.Rank.Name == Rank.King)
                                || (x.Suit == Suit.Clubs && x.Rank.Name == Rank.Queen)
                            )
                        ).ToList();
                case 2:
                    return euchreDeck.Cards.Where(
                            (x =>
                                (x.Suit == Suit.Hearts && x.Rank.Name == Rank.Jack)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.Jack)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.Ace)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.King)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.Queen)
                            )
                        ).ToList();
                default:
                    return null;
            }
        }
    }
}
