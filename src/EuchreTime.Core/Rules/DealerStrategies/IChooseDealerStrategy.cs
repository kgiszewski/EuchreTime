using System.Collections.Generic;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Decks;

namespace EuchreTime.Core.Rules.DealerStrategies
{
    public interface IChooseDealerStrategy
    {
        IPlayer ChooseDealer(IDeck deck, List<IPlayer> players);
    }
}
