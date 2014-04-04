using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator.Buildings
{
    public class Building : GameObject
    {
        public Building()
            :base()
        {
            ObjectSprite = new Sprite(7, Position.X, Position.Y, 95, 94);
            IsSolid = true;
            IsBig = true;
        }        
    }
}
