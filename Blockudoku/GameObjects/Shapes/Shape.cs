using Blockudoku.Constants;
using Blockudoku.GameObjects.Shapes.ConcreteShapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockudoku.GameObjects.Shapes
{
    public abstract class Shape : PictureBox
    {
        protected bool _canRotate;

        public Shape()
        {
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.BackColor = Color.Transparent;
        }

        public Shape(Point location)
        {
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.BackColor = Color.Transparent;
            this.Location = location;
        }

        public abstract Block[,] BlockMatrix { get; }

        public Block LeadBlock { get; private set; }

        public bool CanRotate
        {
            get
            {
                return _canRotate;
            }
            set
            {
                _canRotate = value;
                this.BorderStyle = _canRotate ? BorderStyle.Fixed3D : BorderStyle.None;
            }
        }

        public List<Block> Blocks
        {
            get; protected set;
        }

        public virtual void SetShapePlayability(bool isPlayable)
        {
            foreach (var block in this.Blocks)
            {
                block.CanBePlaced = isPlayable;
            }
            this.Draw();
        }



        protected abstract void InitializeBlockMatrix();

        public bool IsPlayable()
        {
            return this.Blocks?.All(b => b.CanBePlaced) ?? false;
        }

        public void SetShapeHealth(int health)
        {
            if (this.Blocks == null)
            {
                return;
            }
            foreach (var block in this.Blocks)
            {
                block.SetHealth(health);
            }
        }

        public virtual Shape Rotate90(Shape shapeToRotate, bool clockwise)
        {
            if (shapeToRotate == null)
                return shapeToRotate;

            if (clockwise)
            {
                return RotateClockwise(shapeToRotate);
            }
            return RotateCounterClockwise(shapeToRotate);
        }

        private Block GetCenterBlockToRotateAround(Shape shapeToRotate)
        {
            var blockMatrix = shapeToRotate.BlockMatrix;

            int maxXLength = blockMatrix.GetLength(0);
            int maxYLength = blockMatrix.GetLength(1);

            int centerX = maxXLength / 2;
            int centerY = maxYLength / 2;
            int itemsAvailable = maxYLength * maxXLength;
            int itemsChecked = 0;

            int upIndex = centerY;
            int downIndex = centerY;
            int leftIndex = centerX;
            int rightIndex = centerX;

            HashSet<Point> pointsChecked = new HashSet<Point>();

            while (itemsChecked < itemsAvailable)
            {
                if (upIndex >= 0 && blockMatrix[centerX, upIndex] != null)
                {
                    return blockMatrix[centerX, upIndex];
                }
                else if (upIndex >= 0)
                {
                    Point p = new Point(centerX, upIndex);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                    if (upIndex > 0)
                        upIndex--;
                }

                if (downIndex < maxYLength && blockMatrix[centerX, downIndex] != null)
                {
                    return blockMatrix[centerX, downIndex];
                }
                else if (downIndex < maxXLength)
                {
                    Point p = new Point(centerX, downIndex);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                    downIndex++;
                }

                if (leftIndex >= 0 && blockMatrix[leftIndex, centerY] != null)
                {
                    return blockMatrix[leftIndex, centerY];
                }
                else if (leftIndex >= 0)
                {
                    Point p = new Point(leftIndex, centerY);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                    if (leftIndex > 0)
                        leftIndex--;
                }

                if (rightIndex < maxXLength && blockMatrix[rightIndex, centerY] != null)
                {
                    return blockMatrix[rightIndex, centerY];
                }
                else if (rightIndex < maxXLength)
                {
                    Point p = new Point(rightIndex, centerY);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                    rightIndex++;
                }

                //check diagonals
                if (rightIndex < maxXLength && upIndex >= 0 && blockMatrix[rightIndex, upIndex] != null)
                {
                    return blockMatrix[rightIndex, upIndex];
                }
                else if (rightIndex < maxXLength && upIndex >= 0)
                {
                    Point p = new Point(rightIndex, upIndex);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                }

                if (leftIndex >= 0 && upIndex >= 0 && blockMatrix[leftIndex, upIndex] != null)
                {
                    return blockMatrix[leftIndex, upIndex];
                }
                else if (leftIndex >= 0 && upIndex >= 0)
                {
                    Point p = new Point(leftIndex, upIndex);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                }

                if (rightIndex < maxXLength && downIndex < maxYLength && blockMatrix[rightIndex, downIndex] != null)
                {
                    return blockMatrix[rightIndex, downIndex];
                }
                else if (rightIndex < maxXLength && downIndex < maxYLength)
                {
                    Point p = new Point(rightIndex, downIndex);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                }

                if (leftIndex >= 0 && downIndex < maxYLength && blockMatrix[leftIndex, downIndex] != null)
                {
                    return blockMatrix[leftIndex, downIndex];
                }
                else if (leftIndex >= 0 && downIndex < maxYLength)
                {
                    Point p = new Point(leftIndex, downIndex);
                    pointsChecked.Add(p);
                    itemsChecked = pointsChecked.Count;
                }
            }

            return null;
        }

        private Shape RotateCounterClockwise(Shape shapeToRotate)
        {
            Block centerBlock = GetCenterBlockToRotateAround(shapeToRotate);
            if (centerBlock != null)
            {
                List<Point> newIndices = new List<Point>() { new Point(centerBlock.XIndex, centerBlock.YIndex) };
                var oldMatrix = shapeToRotate.BlockMatrix;
                int oldXLength = oldMatrix.GetLength(0);
                int oldYLength = oldMatrix.GetLength(1);
                for (int i = 0; i < oldXLength; i++)
                {
                    for (int j = 0; j < oldYLength; j++)
                    {
                        var block = oldMatrix[i, j];
                        if (block != null && block != centerBlock)
                        {

                            //get x and y differences from center
                            int xDiff = block.XIndex - centerBlock.XIndex;
                            int yDiff = block.YIndex - centerBlock.YIndex;
                            int newX = centerBlock.XIndex;
                            int newY = centerBlock.YIndex;
                            //horizontal diff only 
                            if (xDiff != 0 && yDiff == 0)
                            {
                                newY = centerBlock.YIndex - xDiff;
                            }//vertical diff only
                            else if (xDiff == 0 && yDiff != 0)
                            {
                                newX = centerBlock.XIndex + yDiff;
                            }
                            //diagonal down left
                            else if (xDiff < 0 && yDiff > 0)
                            {
                                newX = centerBlock.XIndex + yDiff;
                                newY = centerBlock.YIndex - xDiff;
                            }
                            //diagonal down right
                            else if (xDiff > 0 && yDiff > 0)
                            {
                                newY = centerBlock.YIndex - xDiff;
                                newX = centerBlock.XIndex + yDiff;
                            }
                            //diagonal up right
                            else if (xDiff > 0 && yDiff < 0)
                            {
                                newX = centerBlock.XIndex + yDiff;
                                newY = centerBlock.YIndex - xDiff;
                            }
                            //diagonal up left
                            else if (xDiff < 0 && yDiff < 0)
                            {
                                newX = centerBlock.XIndex + yDiff;
                                newY = centerBlock.YIndex - xDiff;
                            }
                            newIndices.Add(new Point(newX, newY));
                        }
                    }
                }
                //readjust the matrix indices to center after a rotation
                int minX = newIndices.Select(i => i.X).Min();
                int minY = newIndices.Select(i => i.Y).Min();

                List<Point> newMatrixIndices = newIndices.Select(p => new Point(p.X - minX, p.Y - minY)).ToList();

                int newXLength = newMatrixIndices.Select(i => i.X).Max() + 1;
                int newYLength = newMatrixIndices.Select(i => i.Y).Max() + 1;
                //initialize the new matrix
                Block[,] newMatrix = new Block[newXLength, newYLength];
                foreach (var index in newMatrixIndices)
                {
                    newMatrix[index.X, index.Y] = new Block();
                }

                //create the customBlock to return
                CustomBlock customBlock = new CustomBlock(newMatrix, new Point(shapeToRotate.Location.X, shapeToRotate.Location.Y));
                customBlock.CanRotate = true;
                return customBlock;
            }

            return shapeToRotate;
        }

        private Shape RotateClockwise(Shape shapeToRotate)
        {
            Block centerBlock = GetCenterBlockToRotateAround(shapeToRotate);
            if (centerBlock != null)
            {
                List<Point> newIndices = new List<Point>() { new Point(centerBlock.XIndex, centerBlock.YIndex) };
                var oldMatrix = shapeToRotate.BlockMatrix;
                int oldXLength = oldMatrix.GetLength(0);
                int oldYLength = oldMatrix.GetLength(1);
                for (int i = 0; i < oldXLength; i++)
                {
                    for (int j = 0; j < oldYLength; j++)
                    {
                        var block = oldMatrix[i, j];
                        if (block != null && block != centerBlock)
                        {

                            //get x and y differences from center
                            int xDiff = block.XIndex - centerBlock.XIndex;
                            int yDiff = block.YIndex - centerBlock.YIndex;
                            int newX = centerBlock.XIndex;
                            int newY = centerBlock.YIndex;
                            //horizontal diff only 
                            if (xDiff != 0 && yDiff == 0)
                            {
                                newY = centerBlock.YIndex + xDiff;
                            }//vertical diff only
                            else if (xDiff == 0 && yDiff != 0)
                            {
                                newX = centerBlock.XIndex - yDiff;
                            }
                            //diagonal down left
                            else if (xDiff < 0 && yDiff > 0)
                            {
                                newX = centerBlock.XIndex - yDiff;
                                newY = centerBlock.YIndex + xDiff;
                            }
                            //diagonal down right
                            else if (xDiff > 0 && yDiff > 0)
                            {
                                newY = centerBlock.YIndex + xDiff;
                                newX = centerBlock.XIndex - yDiff;
                            }
                            //diagonal up right
                            else if (xDiff > 0 && yDiff < 0)
                            {
                                newX = centerBlock.XIndex - yDiff;
                                newY = centerBlock.YIndex + xDiff;
                            }
                            //diagonal up left
                            else if (xDiff < 0 && yDiff < 0)
                            {
                                newX = centerBlock.XIndex - yDiff;
                                newY = centerBlock.YIndex + xDiff;
                            }
                            newIndices.Add(new Point(newX, newY));
                        }
                    }
                }
                //readjust the matrix indices to center after a rotation
                int minX = newIndices.Select(i => i.X).Min();
                int minY = newIndices.Select(i => i.Y).Min();

                List<Point> newMatrixIndices = newIndices.Select(p => new Point(p.X - minX, p.Y - minY)).ToList();

                int newXLength = newMatrixIndices.Select(i => i.X).Max() + 1;
                int newYLength = newMatrixIndices.Select(i => i.Y).Max() + 1;
                //initialize the new matrix
                Block[,] newMatrix = new Block[newXLength, newYLength];
                foreach (var index in newMatrixIndices)
                {
                    newMatrix[index.X, index.Y] = new Block();
                }

                //create the customBlock to return
                CustomBlock customBlock = new CustomBlock(newMatrix, new Point(shapeToRotate.Location.X, shapeToRotate.Location.Y));
                customBlock.CanRotate = true;
                return customBlock;
            }

            return shapeToRotate;
        }

        public abstract Shape ShallowCopy();

        protected virtual void Draw()
        {

            int width = CommonConstants.BLOCK_SIZE;//width/height of each block
            int height = CommonConstants.BLOCK_SIZE;
            int imageWidth = this.BlockMatrix.GetLength(0) * width; //image width should match the size of the matrix * dimensions of a block
            int imageHeight = this.BlockMatrix.GetLength(1) * height;
            int x = 0, y = 0;//current x and y to draw at
            int maxXIndex = this.BlockMatrix.GetLength(0); //length of each dimension of the block matrix
            int maxYIndex = this.BlockMatrix.GetLength(1);
            Bitmap finalImage = new Bitmap(imageWidth, imageHeight);
            using (Graphics g = Graphics.FromImage(finalImage))
            {
                for (int i = 0; i < maxXIndex; i++)
                {
                    for (int j = 0; j < maxYIndex; j++)
                    {
                        if (this.BlockMatrix[i, j] != null)
                        {
                            g.DrawImage(this.BlockMatrix[i, j].Image, new Rectangle(new Point(x, y), new Size(width, height)));
                        }
                        if (j < maxYIndex - 1)
                            y += height + 1;
                    }
                    if (i < maxXIndex - 1)
                        x += width + 1;
                    y = 0;//reset Y since we are draw down for each column
                }
            }
            this.Image = finalImage;
        }

        protected virtual void SetLeadBlock()
        {
            bool setLeadBlock = false;
            int leadBlockXIndex = 0;
            int leadBlockYIndex = 0;
            for (int i = 0; i < this.BlockMatrix.GetLength(1); i++)
            {
                for (int j = 0; j < this.BlockMatrix.GetLength(0); j++)
                {
                    if (this.BlockMatrix[j, i] != null)
                    {
                        if (!setLeadBlock)
                        {
                            this.LeadBlock = this.BlockMatrix[j, i];
                            leadBlockXIndex = j;
                            leadBlockYIndex = i;
                            setLeadBlock = true;
                        }

                        int xOffset = ((j - leadBlockXIndex) * CommonConstants.BLOCK_SIZE) + (j - leadBlockXIndex);
                        int yOffset = ((i - leadBlockYIndex) * CommonConstants.BLOCK_SIZE) + (i - leadBlockYIndex);
                        this.BlockMatrix[j, i].XOffset = xOffset;
                        this.BlockMatrix[j, i].YOffset = yOffset;
                        this.BlockMatrix[j, i].XIndex = j;
                        this.BlockMatrix[j, i].YIndex = i;
                    }
                }
            }
        }

        public virtual bool CanBePlaceInBoardLocation(Block[,] boardMatrix, int xOffSet, int yOffset)
        {
            bool isInBorderWidthWise = xOffSet + this.BlockMatrix.GetLength(0) <= boardMatrix.GetLength(0);
            bool isinBorderHeightWise = yOffset + this.BlockMatrix.GetLength(1) <= boardMatrix.GetLength(1);

            if (!(isinBorderHeightWise && isInBorderWidthWise))
            {
                this.SetShapePlayability(false);
                return false;
            }

            for (int i = 0; i < this.BlockMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < this.BlockMatrix.GetLength(1); j++)
                {
                    if (this.BlockMatrix[i, j] != null && boardMatrix[i + xOffSet, j + yOffset] != null)
                    {
                        this.SetShapePlayability(false);
                        return false;//collision found, cannot place block
                    }
                }

            }
            //if the shape is inside the game board completely and has no collisions, it can be placed
            this.SetShapePlayability(true);
            return true;
        }
    }
}
