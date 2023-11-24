using Blockudoku.GameObjects.Shapes.ConcreteShapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes
{
    public class StandardModeShapeFactory : IShapeFactory
    {
        public Shape GenerateShape()
        {
            Random random = new Random();
            List<Shape> shapes = this.GetShapeList();
            int count = shapes.Count;
            int index = random.Next(0, count);
            var shape = shapes[index];
            return shape;
        }

        public Shape GenerateShape(string typeName)
        {
            switch (typeName)
            {
                case nameof(SingleBlockShape):
                    return new DoubleVerticalShape();
                case nameof(DoubleVerticalShape):
                    return new DoubleHorizontalShape();
                case nameof(DoubleHorizontalShape):
                    return new DoubleHorizontalShape();
                case nameof(BottomRightL2x2):
                    return new BottomRightL2x2();
                case nameof(BottomLeftL2x2):
                    return new BottomLeftL2x2();
                case nameof(TopLeftL2x2):
                    return new TopLeftL2x2();
                case nameof(TopRightL2x2):
                    return new TopRightL2x2();
                case nameof(TripleHorizonal):
                    return new TripleHorizonal();
                case nameof(BottomLeftL3x2):
                    return new BottomLeftL3x2();
                case nameof(BottomRightL3x2):
                    return new BottomRightL3x2();
                case nameof(TopRightL3x2):
                    return new TopRightL3x2();
                case nameof(TopLeftL3x2):
                    return new TopLeftL3x2();
                case nameof(TripleVertical):
                    return new TripleVertical();
                case nameof(QuadrupleHorizontal):
                    return new QuadrupleHorizontal();
                case nameof(QuadrupleVertical):
                    return new QuadrupleVertical();
                case nameof(QuintupleHorizontal):
                    return new QuintupleHorizontal();
                case nameof(QuintupleVertical):
                    return new QuintupleVertical();
                case nameof(PlusBlock):
                    return new PlusBlock();
                case nameof(TopLeftL3x3):
                    return new TopLeftL3x3();
                case nameof(TopRightL3x3):
                    return new TopRightL3x3();
                case nameof(BottomRightL3x3):
                    return new BottomRightL3x3();
                case nameof(BottomLeftL3x3):
                    return new BottomLeftL3x3();
                case nameof(SBlockLeft):
                    return new SBlockLeft();
                case nameof(SBlockRight):
                    return new SBlockRight();
                case nameof(SBlockUpLeft):
                    return new SBlockUpLeft();
                case nameof(SBlockUpRight):
                    return new SBlockUpRight();
                case nameof(VerticalLBlockUpLeft):
                    return new VerticalLBlockUpLeft();
                case nameof(VerticalLBlockUpRight):
                    return new VerticalLBlockUpRight();
                case nameof(VerticalLBlockDownLeft):
                    return new VerticalLBlockDownLeft();
                case nameof(VerticalLBlockDownRight):
                    return new VerticalLBlockDownRight();
                case nameof(DoubleDiagonalLeft):
                    return new DoubleDiagonalLeft();
                case nameof(DoubleDiagonalRight):
                    return new DoubleDiagonalRight();
                case nameof(TripleDiagonalLeft):
                    return new TripleDiagonalLeft();
                case nameof(TripleDiagonalRight):
                    return new TripleDiagonalRight();
                case nameof(SquareBlock2x2):
                    return new SquareBlock2x2();
                case nameof(SmallTBlockUp):
                    return new SmallTBlockUp();
                case nameof(SmallTBlockDown):
                    return new SmallTBlockDown();
                case nameof(SmallTBlockLeft):
                    return new SmallTBlockLeft();
                case nameof(SmallTBlockRight):
                    return new SmallTBlockRight();
                case nameof(LargeTBlockUp):
                    return new LargeTBlockUp();
                case nameof(LargeTBlockDown):
                    return new LargeTBlockDown();
                case nameof(LargeTBlockLeft):
                    return new LargeTBlockLeft();
                case nameof(LargeTBlockRight):
                    return new LargeTBlockRight();
                case nameof(CBlockLeft):
                    return new CBlockLeft();
                case nameof(CBlockRight):
                    return new CBlockRight();
                case nameof(CBlockUp):
                    return new CBlockUp();
                case nameof(CBlockDown):
                    return new CBlockDown();
            }
            return GenerateShape();
        }

        public List<Shape> GetShapeList()
        {
            return new List<Shape>()
            {
                  new SingleBlockShape(),
                  new DoubleVerticalShape(),
                  new DoubleHorizontalShape(),
                  new BottomRightL2x2(),
                  new BottomLeftL2x2(),
                  new TopLeftL2x2(),
                  new TopRightL2x2(),
                  new TripleHorizonal(),
                  new BottomLeftL3x2(),
                  new BottomRightL3x2(),
                  new TopRightL3x2(),
                  new TopLeftL3x2(),
                  new TripleVertical(),
                  new QuadrupleHorizontal(),
                  new QuadrupleVertical(),
                  new QuintupleHorizontal(),
                  new QuintupleVertical(),
                  new PlusBlock(),
                  new TopLeftL3x3(),
                  new TopRightL3x3(),
                  new BottomRightL3x3(),
                  new BottomLeftL3x3(),
                  new SBlockLeft(),
                  new SBlockRight(),
                  new SBlockUpLeft(),
                  new SBlockUpRight(),
                  new VerticalLBlockUpLeft(),
                  new VerticalLBlockUpRight(),
                  new VerticalLBlockDownLeft(),
                  new VerticalLBlockDownRight(),
                  new DoubleDiagonalLeft(),
                  new DoubleDiagonalRight(),
                  new TripleDiagonalLeft(),
                  new TripleDiagonalRight(),
                  new SquareBlock2x2(),
                  new SmallTBlockUp(),
                  new SmallTBlockDown(),
                  new SmallTBlockLeft(),
                  new SmallTBlockRight(),
                  new LargeTBlockUp(),
                  new LargeTBlockDown(),
                  new LargeTBlockLeft(),
                  new LargeTBlockRight(),
                  new CBlockLeft(),
                  new CBlockRight(),
                  new CBlockUp(),
                  new CBlockDown()
            };
        }
    }
}
