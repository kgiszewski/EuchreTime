using System;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Hand
{
    public class AiPlayedCardEventArgs : EventArgs
    {
        public ICard Card { get; }

        public AiPlayedCardEventArgs(ICard card)
        {
            Card = card;
        }
    }
}
