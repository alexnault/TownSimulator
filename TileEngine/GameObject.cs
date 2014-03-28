using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public class GameObject
    {
        public Point Position { get; set; }
        public Sprite ObjectSprite { get; set; }
        public bool IsSolid { get; set; }
        
        public GameObject()
        {
            IsSolid = false;
            Position = new Point(0, 0);
        }

        public void Update(GameTime gameTime)
        {
            if (ObjectSprite != null)
            {
                ObjectSprite.PixelPosition = Engine.ConvertCellToPosition(Position);
                ObjectSprite.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ObjectSprite != null)
            {
                ObjectSprite.Draw(spriteBatch);
            }
        }


    }
}
