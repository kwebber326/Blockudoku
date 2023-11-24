using Blockudoku.GameObjects.GameLogicUtilities;
using Blockudoku.GameObjects.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.FileIOUtilities.DataContracts
{
    public abstract class SavedGame
    {
        public Block[,] GameBoard { get; set; }

        public int Score { get; set; }

        public GameMode GameMode { get; set; }

        public List<Shape> Inventory { get; set; }

        public int CurrentStreak { get; set; }
    }
}
