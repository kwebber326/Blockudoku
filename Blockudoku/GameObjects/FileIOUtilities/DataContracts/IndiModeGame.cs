using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.FileIOUtilities.DataContracts
{
    public class IndiModeGame : SavedGame
    {
        public int Passes { get; set; }

        public int Rotations { get; set; }
    }
}
