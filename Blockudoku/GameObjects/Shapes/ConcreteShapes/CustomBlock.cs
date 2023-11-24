using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class CustomBlock : Shape
    {
        private Block[,] _blockMatrix;

        public CustomBlock(Block[,] blockMatrix)
        {
            _blockMatrix = blockMatrix;
            Draw();
            SetLeadBlock();
        }

        public CustomBlock(Block[,] blockMatrix, Point location) : base(location)
        {
            _blockMatrix = blockMatrix;
            InitializeBlockList();
            Draw();
            SetLeadBlock();
        }

        private void InitializeBlockList()
        {
            this.Blocks = new List<Block>();
            for (int i = 0; i < _blockMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _blockMatrix.GetLength(1); j++)
                {
                    if (_blockMatrix[i, j] != null)
                    {
                        this.Blocks.Add(_blockMatrix[i, j]);
                    }
                }
            }
        }

        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            int xLength = _blockMatrix.GetLength(0), yLength = _blockMatrix.GetLength(1);
            var newBlockMatrix = new Block[xLength, yLength];
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    newBlockMatrix[i, j] = new Block()
                    {
                        XOffset = _blockMatrix[i, j].XOffset,
                        YOffset = _blockMatrix[i, j].YOffset,
                        XIndex = _blockMatrix[i, j].XIndex,
                        YIndex = _blockMatrix[i, j].YIndex
                    };
                }
            }
            var shape = new CustomBlock(newBlockMatrix, new Point(this.Location.X, this.Location.Y));
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
           
        }
    }
}
