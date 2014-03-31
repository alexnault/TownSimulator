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

        public static bool PlaceBigObjectCentered(GameObject gameObject, Tile bottomCenter)
        {
            if (gameObject.ObjectSprite == null) return false;

            bool objectPlaced = false;

            int overflowX = gameObject.ObjectSprite.Width - Engine.TileWidth;
            int overflowY = gameObject.ObjectSprite.Height - Engine.TileHeight;

            int nbTilesWide = (int)Math.Ceiling((double)overflowX / (double)Engine.TileWidth) + 1;
            int nbTilesHigh = (int)Math.Ceiling((double)overflowY / (double)Engine.TileHeight) + 1;

            List<Tile> tiles = GetTileArea(bottomCenter.Position, nbTilesWide, nbTilesHigh);
            if (tiles != null)
            {
                //TODO etre sur que personne touche au TileMap avant que la boucle finisse? Mutex?
                //Aussi etre sur que les tiles ont toutes ete remplis par les Objects house 

                /////////////////////////////////////////////////////////////////////
                //Devrait etre atomique / mutex pour que personne n'ajoute rien au tilemap pendant

                foreach (Tile t in tiles)
                {
                    if (t.IsSolid) return false;
                }

                tiles.Remove(bottomCenter);

                foreach (Tile t in tiles)
                    t.AddObject(gameObject);

                bottomCenter.AddObject(gameObject);

                //////////////////////////////////////////////////////////////////////

                //Set the offset
                int xOffset = Engine.TileWidth - ((Engine.TileWidth * nbTilesWide - gameObject.ObjectSprite.Width) / 2);
                int yOffset = (Engine.TileHeight * nbTilesHigh - gameObject.ObjectSprite.Height) + Engine.TileHeight;

                gameObject.XDrawOffset = -xOffset;
                gameObject.YDrawOffset = -yOffset;

                objectPlaced = true;
            }

            return objectPlaced;
        }

        /// <summary>
        /// Get the tiles from the bottom center going outward.
        /// </summary>
        /// <param name="bottomCenter"></param>
        /// <param name="width">Number of tile wide.</param>
        /// <param name="height">Number of tile high.</param>
        /// <returns></returns>
        public static List<Tile> GetTileArea(Point bottomCenter, int width, int height)
        {
            List<Tile> tiles = new List<Tile>();

            for (int x = -(width / 2); x <= (width / 2); x++)
            {
                for (int y = -(height - 1); y <= 0; y++)
                {
                    int posX = bottomCenter.X + x;
                    int posY = bottomCenter.Y + y;

                    if (posX < 0 ||
                        posY < 0 ||
                        posX >= TileMap.Width ||
                        posY >= TileMap.Height)
                    {
                        return null;
                    }

                    tiles.Add(TileMap.Tiles[posX, posY]);
                }
            }
            return tiles;
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
