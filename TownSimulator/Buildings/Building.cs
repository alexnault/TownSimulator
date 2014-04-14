using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator.Buildings
{
    public abstract class Building : GameObject
    {
        Town town;

        public Building(Town town)
            :base()
        {
            town.AddBuilding(this);

            ObjectSprite = new Sprite(7, Position.X, Position.Y, 95, 94);
            IsSolid = true;
            IsBig = true;
        }        
    }
}
