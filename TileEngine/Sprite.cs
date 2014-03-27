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

        private Vector2 _pixelPosition;
        private int _width;
        private int _height;

        public Vector2 PixelPosition
        {
            get { return _pixelPosition; }
            set 
            {
                _pixelPosition = value;
                BoundingBox = new Rectangle((int)_pixelPosition.X, (int)_pixelPosition.Y, BoundingBox.Width, BoundingBox.Height);
            }
        }

        public int Width
        {
            get { return _width; }
            set 
            {
                _width = value;
                BoundingBox = new Rectangle((int)_pixelPosition.X, (int)_pixelPosition.Y, _width, _height);
            }
        }

        public int Height
        {
            get { return _height; }
            set 
            { 
                _height = value;
                BoundingBox = new Rectangle((int)_pixelPosition.X, (int)_pixelPosition.Y, _width, _height);
            }
        }

        public Texture2D Texture { get; set; }
        public Rectangle BoundingBox { get; private set; }

        
        public Sprite(Texture2D texture, int posX = 0, int posY = 0, int width = 32, int height = 32)
        {            
            PixelPosition = new Vector2(posX, posY);
            Texture = texture;
            Width = width;
            Height = height;
            BoundingBox = new Rectangle((int)PixelPosition.X, (int)PixelPosition.Y, width, height);
        }
        
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, BoundingBox, Color.White);
        }

    }
}
