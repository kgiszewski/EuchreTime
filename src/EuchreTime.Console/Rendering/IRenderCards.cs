using System.Collections.Generic;
using MechanicGrip.Cards;

namespace EuchreTime.Console.Rendering
{
    public interface IRenderCards
    {
        string RenderCards(List<ICard> cards, CardRenderingOptions renderingOptions);
    }
}
