using Blockudoku.Constants;
using Blockudoku.GameObjects.Shapes;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace Blockudoku.GameObjects.GameLogicUtilities
{
    public class StandardGameLogic : IGameLogic
    {
        private const int SQUARE_SIZE = CommonConstants.SQUARE_SIZE;
        public int BasePointsForStreak => 27;

        public int StreakIncrement => 1;

        public int BasePointsForCombo => 37;

        public int ComboPointIncrement => 7;

        public int SingleMatchScore => 18;

        public int DestroyMatches(Block[,] gameBoard, ref int currentStreak, out string message)
        {
            //initialize the message
            message = string.Empty;
            //get the match count but don't break yet
            int rowsDestroyed = DestroyRows(gameBoard, false, out List<Point> rowBlocks);
            int columnsDestroyed = DestroyColumns(gameBoard, false, out List<Point> columnBlocks);
            int squaresDestroyed = DestroySquares(gameBoard, false, out List<Point> squareBlocks);

            //we need to evaluate all matches before breaking matching blocks, this is so we can
            //appropriately handle for combos
            var allBlocks = new List<Point>();
            allBlocks.AddRange(rowBlocks);
            allBlocks.AddRange(columnBlocks);
            allBlocks.AddRange(squareBlocks);

            // Check for special patterns
            if (IsHourglassPattern(gameBoard, rowBlocks, columnBlocks, squareBlocks))
            {
                message = "Hourglass!";
            }
            else if (IsSpinningTopPattern(gameBoard, rowBlocks, columnBlocks, squareBlocks))
            {
                message = "Spinning Top!";
            }
            else if (IsLollipopPattern(gameBoard, rowBlocks, columnBlocks, squareBlocks))
            {
                message = "Lollipop!";
            }

            foreach (var point in allBlocks)
            {
                if (gameBoard[point.X, point.Y]?.Destroy() ?? false)
                {
                    gameBoard[point.X, point.Y] = null;
                }
            }

            int total = rowsDestroyed + columnsDestroyed + squaresDestroyed;
            //update the message for combos
            if (total > 1 && string.IsNullOrEmpty(message))
            {
                message += $"{total}x Combo!";
            }
            //update the message for streaks
            if (total > 0 && ++currentStreak > 1)
            {
                if (!string.IsNullOrEmpty(message))
                    message += ";";
                message += $"{currentStreak}x Streak!";
            }
            else if (total > 0)
            {
                currentStreak = 1;
            }
            else
            {
                currentStreak = 0;
            }
            //score the play
            //1 total and no streak
            if (total == 1 && currentStreak <= 1)
            {
                return this.SingleMatchScore;
            }
            //evaluate streaks and combos if this is not a non-streak single match
            int comboScore = 0, streakScore = 0;
            if (total > 1)
            {
                comboScore = ((total - 1) * this.ComboPointIncrement) + this.BasePointsForCombo;
            }
            if (currentStreak > 1)
            {
                streakScore = ((currentStreak) * this.StreakIncrement) + this.BasePointsForStreak;
            }
            return comboScore + streakScore;
        }

        private bool IsHourglassPattern(Block[,] gameBoard, List<Point> rowBlocks, List<Point> columnBlocks, List<Point> squareBlocks)
        {
            // Check for two 3x3 squares with one 3x3 section away diagonally
            var squareCenters = GetSquareCenters(squareBlocks);
            if (squareCenters.Count < 2) return false;

            foreach (var center1 in squareCenters)
            {
                foreach (var center2 in squareCenters)
                {
                    if (center1 == center2) continue;
                    if (Math.Abs(center1.X - center2.X) == 3 && Math.Abs(center1.Y - center2.Y) == 3)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsSpinningTopPattern(Block[,] gameBoard, List<Point> rowBlocks, List<Point> columnBlocks, List<Point> squareBlocks)
        {
            // Check for a vertical or horizontal line passing through a 3x3 square
            var squareCenters = GetSquareCenters(squareBlocks);
            if (squareCenters.Count == 0) return false;

            foreach (var center in squareCenters)
            {
                if (IsVerticalLineThroughSquare(gameBoard, center, rowBlocks, columnBlocks))
                {
                    return true;
                }
                if (IsHorizontalLineThroughSquare(gameBoard, center, rowBlocks, columnBlocks))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsLollipopPattern(Block[,] gameBoard, List<Point> rowBlocks, List<Point> columnBlocks, List<Point> squareBlocks)
        {
            // Check for a vertical line passing through a 3x3 square at the top
            var squareCenters = GetSquareCenters(squareBlocks);
            if (squareCenters.Count == 0) return false;

            foreach (var center in squareCenters)
            {
                if (IsVerticalLineThroughSquareAtTop(gameBoard, center, rowBlocks, columnBlocks))
                {
                    return true;
                }
            }
            return false;
        }

        private List<Point> GetSquareCenters(List<Point> squareBlocks)
        {
            var squareCenters = new List<Point>();
            for (int i = 0; i < squareBlocks.Count; i += 9)
            {
                squareCenters.Add(new Point(squareBlocks[i].X + 1, squareBlocks[i].Y + 1));
            }
            return squareCenters;
        }

        private bool IsVerticalLineThroughSquare(Block[,] gameBoard, Point squareCenter, List<Point> rowBlocks, List<Point> columnBlocks)
        {
            // Check if there is a vertical line passing through the square
            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                if (gameBoard[i, squareCenter.Y] != null)
                {
                    // Check if the line is centered in the square
                    if (IsLineCenteredInSquare(gameBoard, i, squareCenter.Y, rowBlocks, columnBlocks))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsHorizontalLineThroughSquare(Block[,] gameBoard, Point squareCenter, List<Point> rowBlocks, List<Point> columnBlocks)
        {
            // Check if there is a horizontal line passing through the square
            for (int i = 0; i < gameBoard.GetLength(1); i++)
            {
                if (gameBoard[squareCenter.X, i] != null)
                {
                    // Check if the line is centered in the square
                    if (IsLineCenteredInSquare(gameBoard, squareCenter.X, i, rowBlocks, columnBlocks))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsVerticalLineThroughSquareAtTop(Block[,] gameBoard, Point squareCenter, List<Point> rowBlocks, List<Point> columnBlocks)
        {
            // Check if there is a vertical line passing through the square at the top
            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                if (gameBoard[i, squareCenter.Y] != null)
                {
                    // Check if the line is centered in the square and at the top
                    if (IsLineCenteredInSquareAtTop(gameBoard, i, squareCenter.Y, rowBlocks, columnBlocks))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsLineCenteredInSquare(Block[,] gameBoard, int x, int y, List<Point> rowBlocks, List<Point> columnBlocks)
        {
            // Check if the line is centered in the square
            int squareSize = 3;
            int startX = x - (squareSize - 1) / 2;
            int startY = y - (squareSize - 1) / 2;
            for (int i = 0; i < squareSize; i++)
            {
                for (int j = 0; j < squareSize; j++)
                {
                    if (gameBoard[startX + i, startY + j] == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsLineCenteredInSquareAtTop(Block[,] gameBoard, int x, int y, List<Point> rowBlocks, List<Point> columnBlocks)
        {
            // Check if the line is centered in the square and at the top
            int squareSize = 3;
            int startX = x - (squareSize - 1) / 2;
            int startY = y - (squareSize - 1) / 2;
            for (int i = 0; i < squareSize; i++)
            {
                for (int j = 0; j < squareSize; j++)
                {
                    if (gameBoard[startX + i, startY + j] == null)
                    {
                        return false;
                    }
                }
            }
            // Check if the line is at the top
            for (int i = 0; i < squareSize; i++)
            {
                if (gameBoard[startX - 1, startY + i] != null)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Destroys any rows and returns the number of rows destroyed
        /// </summary>
        /// <param name="gameboard">board to evaluate</param>
        /// <returns></returns>
        private int DestroyRows(Block[,] gameboard, bool destroyBlocks, out List<Point> rowBlocks)
        {
            int rowsDestroyed = 0;
            rowBlocks = new List<Point>();
            for (int i = 0; i < gameboard.GetLength(1); i++)
            {
                bool isRowDestroyable = true;
                //first pass to check for row destructibility
                for (int j = 0; j < gameboard.GetLength(0); j++)
                {
                    if (gameboard[j, i] == null)
                    {
                        isRowDestroyable = false;
                        break;
                    }
                }
                //if destructible, attempt to destroy the blocks in the row
                if (isRowDestroyable)
                {
                    rowsDestroyed++;
                    for (int j = 0; j < gameboard.GetLength(0); j++)
                    {
                        if (destroyBlocks && gameboard[j, i].Destroy())
                        {
                            gameboard[j, i] = null;
                        }
                        rowBlocks.Add(new Point(j, i));
                    }
                }
            }
            return rowsDestroyed;
        }
        /// <summary>
        /// Destroys any columns and returns the number of columns destroyed
        /// </summary>
        /// <param name="gameboard">board to evaluate</param>
        /// <returns></returns>
        private int DestroyColumns(Block[,] gameboard, bool destroyBlocks, out List<Point> blocks)
        {
            int columnsDestroyed = 0;
            blocks = new List<Point>();
            for (int i = 0; i < gameboard.GetLength(0); i++)
            {
                bool isRowDestroyable = true;
                //first pass to check for column destructibility
                for (int j = 0; j < gameboard.GetLength(1); j++)
                {
                    if (gameboard[i, j] == null)
                    {
                        isRowDestroyable = false;
                        break;
                    }
                }
                //if destructible, attempt to destroy the blocks in the column
                if (isRowDestroyable)
                {
                    columnsDestroyed++;
                    for (int j = 0; j < gameboard.GetLength(1); j++)
                    {
                        if (destroyBlocks && gameboard[i, j].Destroy())
                        {
                            gameboard[i, j] = null;
                        }
                        blocks.Add(new Point(i, j));
                    }
                }
            }
            return columnsDestroyed;
        }
        /// <summary>
        /// Destroys any squares and returns the number of squares destroyed
        /// </summary>
        /// <param name="gameboard">board to evaluate</param>
        /// <returns></returns>
        private int DestroySquares(Block[,] gameboard, bool destroyBlocks, out List<Point> squareBlocks)
        {
            int squaresDestroyed = 0;
            squareBlocks = new List<Point>();
            for (int a = 0; a <= gameboard.GetLength(1) - SQUARE_SIZE; a += SQUARE_SIZE)
            {
                for (int i = 0; i <= gameboard.GetLength(0) - SQUARE_SIZE; i += SQUARE_SIZE)
                {
                    bool canBeDestroyed = true;
                    for (int j = i; j < i + SQUARE_SIZE; j++)
                    {
                        for (int k = a; k < a + SQUARE_SIZE; k++)
                        {
                            if (gameboard[j, k] == null)
                                canBeDestroyed = false;
                        }
                    }
                    if (canBeDestroyed)
                    {
                        squaresDestroyed++;
                        for (int j = i; j < i + SQUARE_SIZE; j++)
                        {
                            for (int k = a; k < a + SQUARE_SIZE; k++)
                            {
                                if (destroyBlocks && gameboard[j, k].Destroy())
                                    gameboard[j, k] = null;

                                squareBlocks.Add(new Point(j, k));
                            }
                        }
                    }
                }
            }
            return squaresDestroyed;
        }

        /// <summary>
        /// Determines whether or not the game is over
        /// </summary>
        /// <param name="shapes"></param>
        /// <param name="gameBoard"></param>
        /// <returns></returns>
        public bool IsGameOver(List<Shape> shapes, Block[,] gameBoard)
        {
            return !shapes.Any(s => this.IsShapePlayable(gameBoard, s));
        }

        public bool IsShapePlayable(Block[,] gameBoard, Shape shape)
        {
            //scan the board for playable spots
            int xLimit = gameBoard.GetLength(0) - shape.BlockMatrix.GetLength(0);
            int yLimit = gameBoard.GetLength(1) - shape.BlockMatrix.GetLength(1);
            for (int i = 0; i <= xLimit; i++)
            {
                for (int j = 0; j <= yLimit; j++)
                {
                    if (shape.CanBePlaceInBoardLocation(gameBoard, i, j))
                        return true;
                }
            }
            return false;
        }

        public List<Point> PreviewMatches(Block[,] gameboard, Shape shape, int startXIndex, int startYIndex)
        {
            List<Point> returnBlocks = new List<Point>();
            //indentify indexes for block in the shape
            //apply the offset
            //add each block in the shape by the offset indices to the board
            for (int i = 0; i < shape.BlockMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < shape.BlockMatrix.GetLength(1); j++)
                {
                    int xOffset = i + startXIndex;
                    int yOffset = j + startYIndex;

                    if (shape.BlockMatrix[i, j] != null)
                    {
                        gameboard[xOffset, yOffset] = shape.BlockMatrix[i, j];
                    }
                }
            }

            //evaluate rows, columns, and squares that can be matched (but don't break anything)
            int rows = DestroyRows(gameboard, false, out List<Point> rowBlocks);
            int columns = DestroyColumns(gameboard, false, out List<Point> columnBlocks);
            int squares = DestroySquares(gameboard, false, out List<Point> squareBlocks);

            if (rows > 0)
            {
                returnBlocks.AddRange(rowBlocks);
            }
            if (columns > 0)
            {
                returnBlocks.AddRange(columnBlocks);
            }
            if (squares > 0)
            {
                returnBlocks.AddRange(squareBlocks);
            }

            //remove the shape from the board to re-establish the existing board
            for (int i = 0; i < shape.BlockMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < shape.BlockMatrix.GetLength(1); j++)
                {
                    int xOffset = i + startXIndex;
                    int yOffset = j + startYIndex;

                    if (shape.BlockMatrix[i, j] != null)
                    {
                        gameboard[xOffset, yOffset] = null;
                    }
                }
            }
            //return any matching blocks in the list
            return returnBlocks;
        }
    }
}
