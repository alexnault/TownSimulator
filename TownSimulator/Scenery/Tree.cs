using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator.Scenery
{
    class Tree : GameObject
    {
        private static Random _rand = new Random();

        public Tree()
        {
            IsSolid = true;

            ObjectSprite = new Sprite(5);
            int spriteType = _rand.Next(0, 2);
            switch (spriteType)
            {
                case(0):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(384, 0, 64, 128);
                    break;
                case(1):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(448, 0, 64, 128);
                    break;
            }
        }
    }
}
