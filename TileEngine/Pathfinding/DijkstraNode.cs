using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public class DijkstraNode
    {
        public int Distance;
        public DijkstraNode Previous;
        public Point Position;
    }
}
