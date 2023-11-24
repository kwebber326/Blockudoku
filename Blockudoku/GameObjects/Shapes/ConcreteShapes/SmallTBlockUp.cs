using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class SmallTBlockUp : Shape
    {
        private Block[,] _blockMatrix;

        public SmallTBlockUp()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public SmallTBlockUp(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new SmallTBlockUp(this.Location);
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[3, 2];
            _blockMatrix[0, 1] = new Block();
            _blockMatrix[1, 1] = new Block();
            _blockMatrix[2, 1] = new Block();
            _blockMatrix[1, 0] = new Block();
            this.Blocks.Add(_blockMatrix[0, 1]);
            this.Blocks.Add(_blockMatrix[1, 1]);
            this.Blocks.Add(_blockMatrix[2, 1]);
            this.Blocks.Add(_blockMatrix[1, 0]);
        }
    }
}
