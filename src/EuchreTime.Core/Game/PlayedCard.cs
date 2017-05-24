using EuchreTime.Core.Players;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Game
{
    public class PlayedCard
    {
        public IPlayer Player { get; set; }
        public ICard Card { get; set; }
    }
}
