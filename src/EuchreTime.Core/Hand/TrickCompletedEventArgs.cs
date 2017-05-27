using System;
using EuchreTime.Core.Players;

namespace EuchreTime.Core.Hand
{
    public class TrickCompletedEventArgs : EventArgs
    {
        public IPlayer TrickWinner { get; set; }

        public TrickCompletedEventArgs(IPlayer trickWinner)
        {
            TrickWinner = trickWinner;
        }
    }
}
