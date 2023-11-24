using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class LargeTBlockDown : Shape
    {
        private Block[,] _blockMatrix;

        public LargeTBlockDown()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public LargeTBlockDown(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new LargeTBlockDown(this.Location);
            return shape;
        }

        //public override Shape Rotate90(Shape shapeToRotate, bool clockwise)
        //{
        //    Shape shape = this;
        //    if (clockwise)
        //    {
        //        shape = new LargeTBlockLeft(new Point(this.Location.X, this.Location.Y)) { CanRotate = true };
        //    }
        //    else
        //    {
        //        shape = new LargeTBlockRight(new Point(this.Location.X, this.Location.Y)) { CanRotate = true };
        //    }
        //    return shape;
        //}

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[3, 3];
            _blockMatrix[0, 0] = new Block();
            _blockMatrix[1, 0] = new Block();
            _blockMatrix[2, 0] = new Block();
            _blockMatrix[1, 1] = new Block();
            _blockMatrix[1, 2] = new Block();
            this.Blocks.Add(_blockMatrix[0, 0]);
            this.Blocks.Add(_blockMatrix[1, 0]);
            this.Blocks.Add(_blockMatrix[2, 0]);
            this.Blocks.Add(_blockMatrix[1, 1]);
            this.Blocks.Add(_blockMatrix[1, 2]);
        }
    }
}
