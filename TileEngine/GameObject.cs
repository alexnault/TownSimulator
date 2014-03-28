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
        //private Vector2 _position;

        public Vector2 Position { get; set; }
        //{ 
        //    get
        //    {
        //        return _position;   
        //    } 
        //    set
        //    {
        //        _position = value;
        //        if (ObjectSprite != null)
        //            ObjectSprite.PixelPosition = new Vector2(_position.X, _position.Y);
        //    } 
        //}

        public Sprite ObjectSprite { get; set; }
        public bool IsSolid { get; set; }
        
        public GameObject()
        {
            IsSolid = false;
            Position = new Vector2(0, 0);
        }

        public void Update(GameTime gameTime)
        {
            if (ObjectSprite != null) 
                ObjectSprite.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ObjectSprite != null)
                ObjectSprite.Draw(spriteBatch, (int)Position.X, (int)Position.Y);
        }


    }
}
