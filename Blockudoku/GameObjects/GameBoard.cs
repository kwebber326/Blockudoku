using Blockudoku.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects
{
    public class GameBoard
    {
        public GameBoard()
        {
            this.Board = new Block[CommonConstants.GAME_BOARD_SIZE, CommonConstants.GAME_BOARD_SIZE];
        }

        public GameBoard(Block[,] customGrid)
        {
            this.Board = customGrid ?? new Block[CommonConstants.GAME_BOARD_SIZE, CommonConstants.GAME_BOARD_SIZE];
        }

        public Block[,] Board
        {
            get; private set;
        }

        public int Score
        {
            get; private set;
        }
    }
}
