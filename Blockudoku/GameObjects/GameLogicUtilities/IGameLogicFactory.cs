using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.GameLogicUtilities
{
    public interface IGameLogicFactory
    {
        IGameLogic CreateGameLogic(GameMode gameMode);
    }
}
