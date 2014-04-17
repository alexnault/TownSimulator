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
        private float _zDelta;
        private Vector2 _pixelPosition;


        public Vector2 PixelPosition 
        { 
            get
            {
                return _pixelPosition;
            }
            set
            {
                _pixelPosition = value;
                UpdateZIndex();
            }
        }

        public float ZIndex { get; private set;}
        public int Width { get; set; }
        public int Height { get; set; }
        public int TextureID { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get;set; }
        public Color DrawingColor { get; set; }
        public Rectangle TexturePortion { get; set; }
        
                
        public Sprite(int textureID, int posX = 0, int posY = 0, int width = 32, int height = 32)
        {
            PixelPosition = new Vector2(posX, posY);
            TextureID = textureID;
            Width = width;
            Height = height;
            Origin = new Vector2(0, 0);
            Rotation = 0;
            TexturePortion = new Rectangle(0, 0, Width, Height);
            DrawingColor = Color.White;

            _zDelta = (float)new Random().Next(1, 10000) / 1000000.0f;
        }

        private void UpdateZIndex()
        {
            float z = 0.0f; //Note: 0.0f = front, 1.0f = back.
            z = 1.0f - ((float)(PixelPosition.Y + Height) / (float)(TileMap.Height * Engine.TileHeight));

            z += _zDelta;
            ZIndex = Math.Min(z, 0.999f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    TextureManager.Get(TextureID),
                    new Rectangle((int)PixelPosition.X + (int)Origin.X, (int)PixelPosition.Y + (int)Origin.Y, Width, Height),
                    TexturePortion,
                    DrawingColor,
                    Rotation,
                    Origin,
                    SpriteEffects.None,
                    ZIndex);
        }
    }
}