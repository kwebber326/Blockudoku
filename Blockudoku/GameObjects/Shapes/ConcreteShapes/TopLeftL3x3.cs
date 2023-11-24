﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes.ConcreteShapes
{
    public class TopLeftL3x3 : Shape
    {
        private Block[,] _blockMatrix;

        public TopLeftL3x3()
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }

        public TopLeftL3x3(Point location) : base(location)
        {
            InitializeBlockMatrix();
            Draw();
            SetLeadBlock();
        }
        public override Block[,] BlockMatrix => _blockMatrix;

        public override Shape ShallowCopy()
        {
            var shape = new TopLeftL3x3(this.Location);
            return shape;
        }

        protected override void InitializeBlockMatrix()
        {
            this.Blocks = new List<Block>();
            _blockMatrix = new Block[3, 3];
            _blockMatrix[0, 0] = new Block();
            _blockMatrix[1, 0] = new Block();
            _blockMatrix[2, 0] = new Block();
            _blockMatrix[2, 1] = new Block();
            _blockMatrix[2, 2] = new Block();
            this.Blocks.Add(_blockMatrix[0, 0]);
            this.Blocks.Add(_blockMatrix[1, 0]);
            this.Blocks.Add(_blockMatrix[2, 0]);
            this.Blocks.Add(_blockMatrix[2, 1]);
            this.Blocks.Add(_blockMatrix[2, 2]);
        }
    }
}
