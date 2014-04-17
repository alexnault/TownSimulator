using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;

namespace TownSimulator.Buildings
{
    class House : Building
    {
        public override int NB_REQUIRED_WOOD { get { return 10; } }
        public override int NB_REQUIRED_STONE { get { return 0; } }

        public House(Town town, bool inConstruction = true) : base(town, inConstruction)
        {
            IsSolid = true;

            ObjectSprite = new Sprite(7, Position.X, Position.Y, 95, 94);
        }
    }
}
