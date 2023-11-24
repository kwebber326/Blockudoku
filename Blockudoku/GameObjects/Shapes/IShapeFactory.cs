using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.GameObjects.Shapes
{
    public interface IShapeFactory
    {
        Shape GenerateShape();

        Shape GenerateShape(string typeName);

        List<Shape> GetShapeList();
    }
}
