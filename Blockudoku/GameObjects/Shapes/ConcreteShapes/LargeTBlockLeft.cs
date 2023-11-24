using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class LargeTBlockLeft : Shape
    {
        private Block[,] _blockMatrix;

        public LargeTBlockLeft()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public LargeTBlockLeft(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new LargeTBlockLeft(this.Location);
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[3, 3];
            _blockMatrix[2, 0] = new Block();
            _blockMatrix[2, 1] = new Block();
            _blockMatrix[2, 2] = new Block();
            _blockMatrix[1, 1] = new Block();
            _blockMatrix[0, 1] = new Block();
            this.Blocks.Add(_blockMatrix[2, 0]);
            this.Blocks.Add(_blockMatrix[2, 1]);
            this.Blocks.Add(_blockMatrix[2, 2]);
            this.Blocks.Add(_blockMatrix[1, 1]);
            this.Blocks.Add(_blockMatrix[0, 1]);
        }
    }
}
