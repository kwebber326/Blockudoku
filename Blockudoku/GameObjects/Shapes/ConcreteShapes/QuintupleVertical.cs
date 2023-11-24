using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class QuintupleVertical : Shape
    {
        private Block[,] _blockMatrix;

        public QuintupleVertical()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public QuintupleVertical(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new QuintupleVertical(this.Location);
            return shape;
        }

        public override Shape Rotate90(Shape shapeToRotate, bool clockwise)
        {
            Shape shape = new QuintupleHorizontal(new Point(this.Location.X, this.Location.Y)) { CanRotate = true };

            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[1, 5];
            _blockMatrix[0, 0] = new Block();
            _blockMatrix[0, 1] = new Block();
            _blockMatrix[0, 2] = new Block();
            _blockMatrix[0, 3] = new Block();
            _blockMatrix[0, 4] = new Block();
            this.Blocks.Add(_blockMatrix[0, 0]);
            this.Blocks.Add(_blockMatrix[0, 1]);
            this.Blocks.Add(_blockMatrix[0, 2]);
            this.Blocks.Add(_blockMatrix[0, 3]);
            this.Blocks.Add(_blockMatrix[0, 4]);
        }
    }
}
