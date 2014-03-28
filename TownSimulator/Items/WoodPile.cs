using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TownSimulator.Items
{
    class WoodPile : Item
    {
        public WoodPile()
        {
            ObjectSprite = new TileEngine.Sprite(4) { ZIndex = 0.6f };
            
        }
    }
}
