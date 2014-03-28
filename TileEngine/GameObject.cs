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

        [System.Xml.Serialization.XmlAttribute]
        public Sprite ObjectSprite { get; set; }

        public int XDrawOffset { get; set; }
        public int YDrawOffset { get; set; }

        [System.Xml.Serialization.XmlAttribute]
        public bool IsSolid { get; set; }
        
        public GameObject()
        {
            IsSolid = false;
            Position = new Point(0, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (ObjectSprite != null)
            {
                ObjectSprite.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (ObjectSprite != null)
            {
                ObjectSprite.Draw(spriteBatch, Position.X * Engine.TileWidth + XDrawOffset, Position.Y * Engine.TileHeight + YDrawOffset);
            }
        }

    }
}
