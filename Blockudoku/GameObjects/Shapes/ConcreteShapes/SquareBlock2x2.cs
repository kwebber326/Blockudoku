using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class SquareBlock2x2 : Shape
    {
        private Block[,] _blockMatrix;

        public SquareBlock2x2()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public SquareBlock2x2(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new SquareBlock2x2(this.Location);
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[2, 2];
            _blockMatrix[0, 0] = new Block();
            _blockMatrix[0, 1] = new Block();
            _blockMatrix[1, 0] = new Block();
            _blockMatrix[1, 1] = new Block();
            this.Blocks.Add(_blockMatrix[0, 0]);
            this.Blocks.Add(_blockMatrix[0, 1]);
            this.Blocks.Add(_blockMatrix[1, 0]);
            this.Blocks.Add(_blockMatrix[1, 1]);
        }
    }
}
