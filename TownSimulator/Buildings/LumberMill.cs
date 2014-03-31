using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator.Buildings
{
    class LumberMill : GameObject
    {
        public LumberMill() : base()
        {
            IsSolid = true;
            ObjectSprite = new Sprite(4); // TODO Need proper sprite
        }
    }
}
