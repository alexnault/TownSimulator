using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator.Buildings
{
    public class House : GameObject
    {
        public House()
            :base()
        {
            ObjectSprite = new Sprite(7, 0, 0, 69, 77);
            IsSolid = true;
        }        
    }
}
