using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class QuadrupleHorizontal : Shape
    {
        private Block[,] _blockMatrix;

        public QuadrupleHorizontal()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public QuadrupleHorizontal(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new QuadrupleHorizontal(this.Location);
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[4, 1];
            _blockMatrix[0, 0] = new Block();
            _blockMatrix[1, 0] = new Block();
            _blockMatrix[2, 0] = new Block();
            _blockMatrix[3, 0] = new Block();
            this.Blocks.Add(_blockMatrix[0, 0]);
            this.Blocks.Add(_blockMatrix[1, 0]);
            this.Blocks.Add(_blockMatrix[2, 0]);
            this.Blocks.Add(_blockMatrix[3, 0]);
        }
    }
}
