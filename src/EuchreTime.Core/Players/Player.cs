using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Players
{
    public class Player
    {
        private readonly int _teamNumber;
        private List<Card> _cards;

        public int TeamNumber
        {
            get { return _teamNumber; }
        }

        public Player(int teamNumber)
        {
            _teamNumber = teamNumber;
        }

        public IEnumerable<Card> GetCards()
        {
            return _cards;
        }

        public void Deal(GameState gameState)
        {
            var deck = gameState.GetDeck();

            deck.Initialize();

            for (var i = 0; i < 100; i++)
            {
                deck.Shuffle();
            }

            var cards = deck.GetDeck();

            //find dealer position in the player list
            var players = gameState.GetPlayers().ToList();

            var indexOfDealer = players.ToList().FindIndex(x => x.GetHashCode() == GetHashCode());

            //keep track of three vs two cards
            var isThreeCards = true;

            for(var i = 0; i < 8; i++)
            {
                var dealToPlayerIndex = indexOfDealer + 1;

                if (dealToPlayerIndex > 3)
                {
                    dealToPlayerIndex = 0;
                }

                if (isThreeCards)
                {
                    players[dealToPlayerIndex]._cards.Add(cards.Pop());
                    players[dealToPlayerIndex]._cards.Add(cards.Pop());
                    players[dealToPlayerIndex]._cards.Add(cards.Pop());
                    isThreeCards = false;
                }
                else
                {
                    players[dealToPlayerIndex]._cards.Add(cards.Pop());
                    players[dealToPlayerIndex]._cards.Add(cards.Pop());
                    isThreeCards = true;
                }
            }

            //add rest to kitty
            gameState.Kitty = cards;
            gameState.Trump = gameState.Kitty.Peek().Suit;
        }
    }
}
