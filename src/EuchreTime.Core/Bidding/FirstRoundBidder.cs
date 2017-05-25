using System;
using System.Linq;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Bidding
{
    public class FirstRoundBidder : IHandleFirstRoundBidding
    {
        public event AiOrderedUpDealerHandler OnAiOrderedUpDealer;

        public void AskEachPlayerAboutTheTopCard(
            IGameState gameState, 
            Func<bool> shouldHumanOrderUp, 
            Func<ICard> humanChooseDiscard
        )
        {
            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                var shouldOrderUp = false;

                if (gameState.CurrentPlayer.IsHuman)
                {
                    shouldOrderUp = shouldHumanOrderUp();
                }
                else
                {
                    shouldOrderUp = gameState.CurrentPlayer.PlayerStrategy.ShouldOrderUpDealerInFirstRound(gameState);

                    OnAiOrderedUpDealer?.Invoke(this, new AiOrderedUpDealerEventArgs(gameState.CurrentPlayer, shouldOrderUp));
                }

                if (shouldOrderUp)
                {
                    //order up the dealer
                    if (gameState.Dealer.IsHuman)
                    {
                        var cardToRemove = humanChooseDiscard();

                        gameState.CurrentPlayer.Cards.Remove(cardToRemove);

                        gameState.Kitty.Push(cardToRemove);
                    }
                    else
                    {
                        gameState.Dealer.DiscardWhenOrderedUp(gameState);
                    }

                    gameState.Dealer.Cards.Add(gameState.TurnedUpCard);
                    gameState.Trump = gameState.TurnedUpCard.Suit;
                    gameState.OrderingUpPlayer = gameState.CurrentPlayer;
                    break;
                }
            }
        }
    }
}
