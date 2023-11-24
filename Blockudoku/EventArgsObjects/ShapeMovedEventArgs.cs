using Blockudoku.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockudoku.EventArgsObjects
{
    public class ShapeMovedEventArgs
    {   
        public List<Block> Blocks { get; set; }
    }
}
