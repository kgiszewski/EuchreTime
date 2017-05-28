using System;
using EuchreTime.Core.Players;

namespace EuchreTime.Core.Bidding
{
    public class AiOrderedUpDealerEventArgs : EventArgs
    {
        public IPlayer Player { get; }
        public bool ShouldOrderUp { get; }

        public AiOrderedUpDealerEventArgs(IPlayer player, bool shouldOrderUp)
        {
            Player = player;
            ShouldOrderUp = shouldOrderUp;
        }
    }
}
