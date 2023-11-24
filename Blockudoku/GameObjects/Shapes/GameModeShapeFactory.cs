using Blockudoku.GameObjects.GameLogicUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes
{
    public static class GameModeShapeFactory
    {
        public static IShapeFactory GetFactoryFromGameMode(GameMode gameMode)
        {
            switch(gameMode)
            {
                case GameMode.STANDARD:
                case GameMode.INDI_MODE:
                case GameMode.CUSTOM:
                    return new StandardModeShapeFactory();
                default:
                    return null;
            }
        }
    }
}
