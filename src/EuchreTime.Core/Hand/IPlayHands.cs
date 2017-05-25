using System;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Hand
{
    public delegate void AiPlayedCardHandler(object sender, AiPlayedCardEventArgs e);

    public interface IPlayHands
    {
        event AiPlayedCardHandler OnAiPlayedCard;

        void PlayHand(IGameState gameState, Func<ICard> chooseHumanCard);
    }
}
