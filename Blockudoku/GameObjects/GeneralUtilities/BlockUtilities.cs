using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.GeneralUtilities
{
    public static class BlockUtilities
    {
        public static double GetPythagorianDistance(int x1, int x2, int y1, int y2)
        {
            var distance = Math.Sqrt(Math.Pow((x1 - x2), 2.0) + Math.Pow((y1 - y2), 2.0));
            return distance;
        }
    }
}
