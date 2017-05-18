using System.Collections.Generic;
using EuchreTime.Core.Game;
using EuchreTime.Core.Rules.PlayerStrategies;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Players
{
    public class PlayerBase : IPlayer
    {
        public List<ICard> Cards { get; set; }
        public int TeamNumber { get; }
        public IPlayerStrategy PlayerStrategy { get; }
        public bool IsHuman { get; }
        public int TricksTaken { get; set; }

        public PlayerBase(int teamNumber, IPlayerStrategy playerStrategy, bool isHuman)
        {
            TeamNumber = teamNumber;
            Cards = new List<ICard>();
            PlayerStrategy = playerStrategy;
            IsHuman = isHuman;
        }
        
        public virtual void Deal(IGameState gameState)
        {
            var deck = gameState.Deck;

            deck.Initialize();

            for (var i = 0; i < 100; i++)
            {
                deck.Shuffle();
            }

            gameState.CurrentPlayer = gameState.Dealer;

            //keep track of three vs two cards
            var isThreeCards = true;

            for (var i = 0; i < 8; i++)
            {
                gameState.AdvanceToNextPlayer();

                //flip the 3/2 pattern on the 4th handout
                if (i == 4)
                {
                    isThreeCards = !isThreeCards;
                }

                if (isThreeCards)
                {
                    gameState.CurrentPlayer.Cards.Add(deck.Cards.Pop());
                    gameState.CurrentPlayer.Cards.Add(deck.Cards.Pop());
                    gameState.CurrentPlayer.Cards.Add(deck.Cards.Pop());
                    isThreeCards = false;
                }
                else
                {
                    gameState.CurrentPlayer.Cards.Add(deck.Cards.Pop());
                    gameState.CurrentPlayer.Cards.Add(deck.Cards.Pop());
                    isThreeCards = true;
                }
            }

            //add rest to kitty
            gameState.Kitty = deck.Cards;

            //turn up top card of kitty
            gameState.TurnedUpCard = gameState.Kitty.Pop();
        }
    }
}
