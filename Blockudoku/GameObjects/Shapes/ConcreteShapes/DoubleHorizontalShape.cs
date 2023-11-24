﻿using Blockudoku.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class DoubleHorizontalShape : Shape
    {
        private Block[,] _blockMatrix;

        public DoubleHorizontalShape()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public DoubleHorizontalShape(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new DoubleHorizontalShape(this.Location);
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[2, 1];
            _blockMatrix[0, 0] = new Block();
            _blockMatrix[1, 0] = new Block();
            this.Blocks.Add(_blockMatrix[0, 0]);
            this.Blocks.Add(_blockMatrix[1, 0]);
        }
    }
}
