using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Players
{
    public class PlayerBase : IPlayer
    {
        public List<ICard> Cards { get; set; }
        public int TeamNumber { get; }

        public PlayerBase(int teamNumber)
        {
            TeamNumber = teamNumber;
            Cards = new List<ICard>();
        }
        
        public virtual void Deal(IGameState gameState)
        {
            var deck = gameState.Deck;

            deck.Initialize();

            for (var i = 0; i < 100; i++)
            {
                deck.Shuffle();
            }

            //find dealer position in the player list
            var players = gameState.Players.ToList();

            var indexOfDealer = players.ToList().FindIndex(x => x.GetHashCode() == GetHashCode());
            var dealToPlayerIndex = indexOfDealer + 1;

            //keep track of three vs two cards
            var isThreeCards = true;

            for (var i = 0; i < 8; i++)
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
                    players[dealToPlayerIndex].Cards.Add(deck.Cards.Pop());
                    players[dealToPlayerIndex].Cards.Add(deck.Cards.Pop());
                    players[dealToPlayerIndex].Cards.Add(deck.Cards.Pop());
                    isThreeCards = false;
                }
                else
                {
                    players[dealToPlayerIndex].Cards.Add(deck.Cards.Pop());
                    players[dealToPlayerIndex].Cards.Add(deck.Cards.Pop());
                    isThreeCards = true;
                }

                dealToPlayerIndex++;
            }

            //add rest to kitty
            gameState.Kitty = deck.Cards;

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
