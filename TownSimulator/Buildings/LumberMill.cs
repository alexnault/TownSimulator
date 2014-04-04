using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator.Buildings
{
    public class LumberMill : Building
    {
        public LumberMill() : base()
        {
            IsSolid = true;
            ObjectSprite = new Sprite(7, Position.X, Position.Y, 95, 94); // TODO Need proper sprite
        }
    }
}
