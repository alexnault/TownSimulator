using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine
{
    public class Sprite
    {

        public Vector2 PixelPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
      

        public int TextureID { get; set; }
        public Rectangle BoundingBox { get; private set; }

        
        public Sprite(int textureID, int posX = 0, int posY = 0, int width = 32, int height = 32)
        {
            PixelPosition = new Vector2(posX, posY);
            TextureID = textureID;
            Width = width;
            Height = height;
        }
        
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, (int)PixelPosition.X, (int)PixelPosition.Y);
        }


        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(
                            TextureManager.Get(TextureID),
                            new Rectangle(x, y, Width, Height),
                            null,
                            Color.White,
                            0,
                            new Vector2(0, 0),
                            SpriteEffects.None,
                            0.5f);




            //spriteBatch.Draw(, , Color.White);
        }

    }
}