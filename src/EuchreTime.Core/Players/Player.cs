using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Players
{
    public class Player
    {
        private readonly List<Card> _cards;

        public int TeamNumber { get; }

        public Player(int teamNumber)
        {
            TeamNumber = teamNumber;
            _cards = new List<Card>();
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

            var cards = deck.GetCards();

            //find dealer position in the player list
            var players = gameState.GetPlayers().ToList();

            var indexOfDealer = players.ToList().FindIndex(x => x.GetHashCode() == GetHashCode());
            var dealToPlayerIndex = indexOfDealer + 1;

            //keep track of three vs two cards
            var isThreeCards = true;

            for(var i = 0; i < 8; i++)
            {
                if (dealToPlayerIndex > 3)
                {
                    dealToPlayerIndex = 0;
                }

                //flip the 3/2 pattern on the 4th handout
                if (i == 4)
                {
                    isThreeCards = !isThreeCards;
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

                dealToPlayerIndex++;
            }

            //add rest to kitty
            gameState.Kitty = cards;

            //turn up top card of kitty
            gameState.TurnedUpCard = gameState.Kitty.Pop();
        }

        public void OrderUpTrump()
        {
            
        }

        public void Pass()
        {
            
        }

        public void GoAlone()
        {
            
        }

        public void PlayCard()
        {
            
        }

        public void Discard()
        {
            
        }
    }
}
