using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.GameLogicUtilities
{
    public class GameLogicFactory : IGameLogicFactory
    {
        public IGameLogic CreateGameLogic(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.CUSTOM:
                    return new CustomModeGameLogic();
                case GameMode.INDI_MODE:
                    return new IndiModeGameLogic();
                case GameMode.STANDARD:
                default:
                    return new StandardGameLogic();
            }
        }
    }
}
