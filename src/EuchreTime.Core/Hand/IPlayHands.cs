using System;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Hand
{
    public delegate void AiPlayedCardHandler(object sender, AiPlayedCardEventArgs e);
    public delegate void TrickCompletedHandler(object sender, TrickCompletedEventArgs e);
    public delegate void HandCompletedHandler(object sender, HandCompletedEventArgs e);

    public interface IPlayHands
    {
        event AiPlayedCardHandler OnAiPlayedCard;
        event TrickCompletedHandler OnTrickCompleted;
        event HandCompletedHandler OnHandCompleted;

        void PlayHand(IGameState gameState, Func<ICard> chooseHumanCard);
    }
}
