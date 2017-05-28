using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Helpers;
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
        public const int Spades = 1;
        public const int Clubs = 2;
        public const int Hearts = 3;
        public const int Diamonds = 4;

        public readonly IInputHelper _inputHelper = new InputHelper();

        [TestCase(1, Spades, true)]
        [TestCase(1, Hearts, false)]
        [TestCase(3, Hearts, true)]
        [TestCase(3, Diamonds, false)]
        public void ContainsLeft(int handId, int trumpSuitId, bool expectedResult)
        {
            var cards = _getCards(handId).OrderBySuitsAndRanks();
            var trumpSuit = _getSuit(trumpSuitId);

            _dumpCardsToConsole(cards);

            var result = CardHelper.ContainsLeft(trumpSuit, cards);

            Assert.AreEqual(result, expectedResult);
        }

        [TestCase(1, Spades, true)]
        [TestCase(1, Hearts, false)]
        [TestCase(3, Hearts, false)]
        [TestCase(3, Diamonds, true)]
        public void ContainsRight(int handId, int trumpSuitId, bool expectedResult)
        {
            var cards = _getCards(handId).OrderBySuitsAndRanks();
            var trumpSuit = _getSuit(trumpSuitId);

            _dumpCardsToConsole(cards);

            var result = CardHelper.ContainsRight(trumpSuit, cards);

            Assert.AreEqual(result, expectedResult);
        }

        [TestCase(1, Spades, Spades, "4,5")] 
        [TestCase(1, Spades, null, "1,2,3,4,5")] 
        [TestCase(2, Spades, Spades, "1,2,3,4,5")] 
        [TestCase(2, Spades, null, "1,2,3,4,5")] 
        [TestCase(3, Hearts, Clubs, "1")]
        [TestCase(4, Diamonds, Hearts, "5")]
        public void CheckForValidIndexes(int handId, int trumpSuitId, int? leadSuitId, string expectedResultString)
        {
            var cards = _getCards(handId).OrderBySuitsAndRanks();
            var trumpSuit = _getSuit(trumpSuitId);
            var leadSuit = _getSuit(leadSuitId);

            Assert.AreEqual(5, cards.Count);

            System.Console.WriteLine($"Trump: {trumpSuit?.Name}");
            System.Console.WriteLine($"Lead: {leadSuit?.Name}");

            _dumpCardsToConsole(cards);

            var result = _inputHelper.GetValidIndexes(leadSuit, trumpSuit, cards).OrderBy(x => x.ToString());
            var expectedResult = _getExpectedResultAsCharList(expectedResultString).OrderBy(x => x.ToString());

            Assert.AreEqual(expectedResult.Count(), result.Count());

            foreach (var item in expectedResult)
            {
                var actualItem = result.FirstOrDefault(x => x.ToString() == item.ToString());

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
                System.Console.WriteLine($"{counter} - {card.Rank.Name} of {card.Suit.Name}");
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
                case 3:
                    return euchreDeck.Cards.Where(
                            (x =>
                                (x.Suit == Suit.Clubs && x.Rank.Name == Rank.Nine)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.Queen)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.Jack)
                                || (x.Suit == Suit.Hearts && x.Rank.Name == Rank.Nine)
                                || (x.Suit == Suit.Spades && x.Rank.Name == Rank.King)
                            )
                        ).ToList();
                case 4:
                    return euchreDeck.Cards.Where(
                            (x =>
                                (x.Suit == Suit.Clubs && x.Rank.Name == Rank.Nine)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.Ace)
                                || (x.Suit == Suit.Diamonds && x.Rank.Name == Rank.Queen)
                                || (x.Suit == Suit.Hearts && x.Rank.Name == Rank.Jack)
                                || (x.Suit == Suit.Hearts && x.Rank.Name == Rank.Nine)
                            )
                        ).ToList();
                default:
                    return null;
            }
        }
    }
}
