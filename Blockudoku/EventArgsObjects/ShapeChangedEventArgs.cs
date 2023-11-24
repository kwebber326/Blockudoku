using Blockudoku.GameObjects.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.EventArgsObjects
{
    public class ShapeChangedEventArgs : EventArgs
    {
        public Shape NewShape { get; set; }
    }
}
