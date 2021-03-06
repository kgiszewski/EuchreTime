﻿using System.Collections.Generic;
using MechanicGrip.Suits;

namespace EuchreTime.Console.Rendering
{
    public interface IRenderSuits
    {
        string RenderSuits(List<ISuit> suitsToChooseFrom, bool displayIndex = false);
    }
}
