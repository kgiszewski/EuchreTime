using System;
using EuchreTime.Core.Players;
using MechanicGrip.Suits;

namespace EuchreTime.Core.Bidding
{
    public class AiChoseTrumpEventArgs : EventArgs
    {
        public IPlayer Player { get; }
        public ISuit Suit { get; }

        public AiChoseTrumpEventArgs(IPlayer player, ISuit suit)
        {
            Player = player;
            Suit = suit;
        }
    }
}
