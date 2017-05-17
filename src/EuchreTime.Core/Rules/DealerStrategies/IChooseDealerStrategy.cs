using System.Collections.Generic;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Decks;

namespace EuchreTime.Core.Rules.DealerStrategies
{
    public interface IChooseDealerStrategy
    {
        Player ChooseDealer(IDeck deck, List<Player> players);
    }
}
