using System.Collections.Generic;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Console.Rendering
{
    public interface IRenderCards
    {
        string RenderCards(List<ICard> cards, CardRenderingOptions renderingOptions);
    }
}
