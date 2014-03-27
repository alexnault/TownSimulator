using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace TileEngine
{
    public class TileMap
    {
        public List<TileLayer> Layers = new List<TileLayer>();
        public CollisionLayer CollisionLayer;

        public int GetWidthInPixels()
        {
            return GetWidth() * Engine.TileWidth;
        }                   
        public int GetHeightInPixels()
        {
            return GetHeight() * Engine.TileHeight;
        }

        public int GetWidth()
        {
            int width = 0;    //Initialiser au cas ou

            foreach (TileLayer layer in Layers)
                width = (int)Math.Max(width, layer.Width);

            return width;

        }
        public int GetHeight()
        {
            int height = 0;    //Initialiser au cas ou

            foreach (TileLayer layer in Layers)
                height = (int)Math.Max(height, layer.Height);

            return height;

        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Point min = Engine.ConvertPositionToCell(camera.Position);
            Point max = Engine.ConvertPositionToCell(
                camera.Position + new Vector2(
                    spriteBatch.GraphicsDevice.Viewport.Width + Engine.TileWidth,
                    spriteBatch.GraphicsDevice.Viewport.Height + Engine.TileHeight)
                );


            foreach (TileLayer layer in Layers)
                layer.Draw(spriteBatch, camera, min, max);
            

        }
    }
}
