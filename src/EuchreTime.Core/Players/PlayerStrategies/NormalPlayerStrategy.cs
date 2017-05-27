using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Players.PlayerStrategies
{
    public class NormalPlayerStrategy : IPlayerStrategy
    {
        public bool ShouldOrderUpDealerInFirstRound(IGameState gameState)
        {
            //TODO: add strategy
            //if I'm dealing, consider picking up based on a total number of trump I would have
            //else consider which team is dealing
            //if other team and bower, pass
            //else count how many trump I have, make a decision
            return false;
        }

        public ISuit SecondRoundTrumpChoice(IGameState gameState)
        {
            //TODO: add strategy
            //if I have at least 3 trump of any suit, order up
            // else pass

            //use get valid index and fuzz the remaining suits?
            return null;
        }

        public ICard ChooseLeadCard(IGameState gameState)
        {
            //TODO: add strategy
            //if I ordered up, lead a strong trump
            //else
            //lead high off suit
            return gameState.CurrentPlayer.Cards[0];
        }

        public ICard ChooseNonLeadCard(IGameState gameState)
        {
            //TODO: add strategy
            //get valid indexes
            //if partner is winning, consider throwing low
            //else consider throwing high or trumping
            return gameState.CurrentPlayer.Cards[0];
        }
    }
}
