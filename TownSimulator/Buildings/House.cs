using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;

namespace TownSimulator.Buildings
{
    public class House : Building
    {
        public House() : base()
        {
            IsSolid = true;

            ObjectSprite = new Sprite(7, Position.X, Position.Y, 95, 94);
        }
    }
}
