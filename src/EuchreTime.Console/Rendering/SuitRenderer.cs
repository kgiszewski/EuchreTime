using System;
using System.Collections.Generic;
using System.Text;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Console.Rendering
{
    public class SuitRenderer : IRenderSuits
    {
        private readonly StringBuilder _sb;
        private static int _numberRows = 2;

        public SuitRenderer()
        {
            _sb = new StringBuilder();
        }

        public string RenderSuits(List<ISuit> suitsToChooseFrom, bool displayIndex = false)
        {
            _sb.Clear();

            for (var row = 0; row < _numberRows; row++)
            {
                var index = 0;

                foreach (var suit in suitsToChooseFrom)
                {
                    index++;

                    switch (row)
                    {
                        case 0:
                            _sb.Append($"{suit.Symbol} ");
                            break;

                        case 1:
                            if (displayIndex)
                            {
                                _sb.Append($"{index} ");
                            }
                            break;
                    }
                }

                _sb.Append(Environment.NewLine);
            }

            return _sb.ToString();
        }
    }
}
