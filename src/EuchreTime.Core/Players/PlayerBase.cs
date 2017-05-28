using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Extensions;
using EuchreTime.Core.Game;
using EuchreTime.Core.Helpers;
using EuchreTime.Core.Players.PlayerStrategies;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Players
{
    public abstract class PlayerBase : IPlayer
    {
        public List<ICard> Cards { get; set; }
        public int TeamNumber { get; }
        public IPlayerStrategy PlayerStrategy { get; }
        public bool IsHuman { get; }
        public int TricksTaken { get; set; }
        public string Name { get; set; }

        public virtual IPlayer Partner(IGameState gameState)
        {
            return gameState.Players.First(x => x.TeamNumber == TeamNumber);
        }

        protected PlayerBase(string name, int teamNumber, IPlayerStrategy playerStrategy, bool isHuman)
        {
            TeamNumber = teamNumber;
            Cards = new List<ICard>();
            PlayerStrategy = playerStrategy;
            IsHuman = isHuman;
            Name = name;
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

            //order each players cards
            foreach (var player in gameState.Players)
            {
                player.Cards = player.Cards.OrderBySuitsAndRanks();
            }
        }

        public virtual void DiscardWhenOrderedUp(IGameState gameState)
        {
            //discard lowest non-trump
            var nonTrumpCards = Cards.Where(x => x.Suit != gameState.TurnedUpCard.Suit || !x.IsTheLeft(gameState.TurnedUpCard.Suit)).OrderBy(x => x.Rank.Value).ToList();
            var trumpCards = Cards.Where(x => x.Suit == gameState.TurnedUpCard.Suit || x.IsTheLeft(gameState.TurnedUpCard.Suit)).ToList();

            var cardToRemove = nonTrumpCards[0];
            nonTrumpCards.RemoveAt(0);

            gameState.Kitty.Push(cardToRemove);

            nonTrumpCards.AddRange(trumpCards);

            Cards = nonTrumpCards;
        }

        public virtual ICard ChooseCardToPlay(IGameState gameState)
        {
            //if no card have been played in the current hand, then this is lead
            if (!gameState.CurrentHand.Any())
            {
                return gameState.CurrentPlayer.PlayerStrategy.ChooseLeadCard(gameState);
            }
            else
            {
                return gameState.CurrentPlayer.PlayerStrategy.ChooseLeadCard(gameState);
            }
        }

        public bool IsDealing(IGameState gameState)
        {
            return this == gameState.Dealer;
        }
    }
}
