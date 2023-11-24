using Blockudoku.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class SingleBlockShape : Shape
    {
        private Block[,] _blockMatrix;

        public SingleBlockShape()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public SingleBlockShape(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public override Block[,] BlockMatrix => _blockMatrix;
        public override Shape ShallowCopy()
        {
            var shape = new SingleBlockShape(this.Location);
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[1, 1];
            _blockMatrix[0, 0] = new Block();
            this.Blocks.Add(_blockMatrix[0, 0]);
        }
    }
}
