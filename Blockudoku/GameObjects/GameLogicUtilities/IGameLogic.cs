using Blockudoku.GameObjects.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.GameLogicUtilities
{
    public interface IGameLogic
    {
        /// <summary>
        /// Determines if a given shape is playable on the board
        /// </summary>
        /// <param name="gameBoard">the board to evaluate</param>
        /// <param name="shape">The shape to check for playability</param>
        /// <returns></returns>
        bool IsShapePlayable(Block[,] gameBoard, Shape shape);

        /// <summary>
        /// Returns what blocks, if any, will be destroyed were a given shape be placed in a specific spot
        /// </summary>
        /// <param name="gameboard">the board to evaluate</param>
        /// <param name="shape">the shape that would be placed</param>
        /// <param name="startXIndex">x dimension index where the lead block of the shape would be placed</param>
        /// <param name="startYIndex">y dimension index where the lead block of the shape would be placed</param>
        /// <returns></returns>
        List<Point> PreviewMatches(Block[,] gameboard, Shape shape, int startXIndex, int startYIndex);

        /// <summary>
        /// Destroys matching blocks according to the given match logic and returns the score for the match
        /// if there was a match
        /// </summary>
        /// <param name="gameBoard">the board to evaluate for matches</param>
        /// <param name="message">description of the matches, if any</param>
        /// <param name="destroyBlocks">set to true if you want to attempt to destroy the matching blocks, false to just identify them</param>
        /// <returns></returns>
        int DestroyMatches(Block[,] gameBoard, ref int currentStreak, out string message);
        /// <summary>
        /// Determines whether or not the game is over
        /// </summary>
        /// <param name="shapes">the list of shapes in the player's inventory</param>
        /// <param name="gameBoard">the board to evaluate</param>
        /// <returns></returns>
        bool IsGameOver(List<Shape> shapes, Block[,] gameBoard);
        /// <summary>
        /// Points for a single match without a streak
        /// </summary>
        int SingleMatchScore { get; }

        /// <summary>
        /// Base points for a streak
        /// </summary>
        int BasePointsForStreak { get; }
        /// <summary>
        /// Points to increment off the base for each subsequent streak
        /// </summary>
        int StreakIncrement { get; }
        /// <summary>
        ///Base points for a combination of matches
        /// </summary>
        int BasePointsForCombo { get; }

        /// <summary>
        /// Points to increment the base score by per extra combination achieved
        /// </summary>
        int ComboPointIncrement { get; }
    }
}
