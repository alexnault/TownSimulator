using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TileEngine
{
    public class GameObject
    {
        private Point _position;

        public Point Position 
        {
            get
            {
                return _position;
            } 
            set
            {
                _position = value;
                if (ObjectSprite != null)

                    ObjectSprite.PixelPosition =
                        new Vector2(
                            value.X * Engine.TileWidth + XDrawOffset,
                            value.Y * Engine.TileHeight + YDrawOffset);
            }
        }
        public Sprite ObjectSprite { get; set; }

        public int XDrawOffset { get; set; }
        public int YDrawOffset { get; set; }
        public bool IsSolid { get; set; }

        //TODO: Change the name
        public bool IsBig { get; set; }
        
        public GameObject()
        {
            XDrawOffset = 0;
            YDrawOffset = 0;
            IsSolid = false;
            IsBig = false;
            Position = new Point(0, 0);
        }


        public static GameObject LoadFromXml(XmlNode node)
        {
            GameObject gObj = new GameObject();
                        
            if(node.ChildNodes.Count > 0)
            {
                XmlNode sprite = node.ChildNodes[0];
                int textureID = int.Parse(sprite.Attributes["TextureID"].Value);

                gObj.ObjectSprite = new Sprite(textureID);

            }

            return gObj;
        }

        /// <summary>
        /// Get the Width and Height of the object in tiles.
        /// Note: Returns only a size of 1 if the object is not big.
        /// </summary>
        /// <returns>A point using the X as width in tiles and Y as height in Tiles.</returns>
        public Size GetTileSize()
        {
            Size size;
            if(IsBig)
            {
                int overflowX = ObjectSprite.Width - Engine.TileWidth;
                int overflowY = ObjectSprite.Height - Engine.TileHeight;

                int nbTilesWide = (int)Math.Ceiling((double)overflowX / (double)Engine.TileWidth) + 1;
                int nbTilesHigh = (int)Math.Ceiling((double)overflowY / (double)Engine.TileHeight) + 1;
                size = new Size(nbTilesWide, nbTilesHigh);
            }
            else
            {
                size = new Size(1, 1);
            }
            

            return size;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            

            if (ObjectSprite != null)
            {
                DrawingUtils.DrawRectangle(
                    new Rectangle(
                        (int)Position.X * Engine.TileWidth,
                        (int)Position.Y * Engine.TileHeight,
                        Engine.TileWidth,
                        Engine.TileHeight), Color.Blue);

                ObjectSprite.Draw(spriteBatch);
            }
            

        }

    }
}
