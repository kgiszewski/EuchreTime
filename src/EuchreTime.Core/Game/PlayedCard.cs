using EuchreTime.Core.Players;
using MechanicGrip.Cards;

namespace EuchreTime.Core.Game
{
    public class PlayedCard
    {
        public IPlayer Player { get; set; }
        public ICard Card { get; set; }
    }
}
