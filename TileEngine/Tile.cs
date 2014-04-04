using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public class Tile
    {

        private bool _isSolid;
        private List<GameObject> _objects;

        public Point Position { get; private set; }
        public int GroundTextureID { get; set; }
        public bool IsSolid
        {
            get
            {
                bool isItSolid = _isSolid;
                foreach (GameObject obj in _objects)
                {
                    if (obj.IsSolid)
                    {
                        isItSolid = true;
                        break;
                    }
                }
                return isItSolid;
            }
        }


        public Tile(int textureID, int posX, int posY, bool isSolid = false)
        {
            _isSolid = isSolid;
            _objects = new List<GameObject>();

            GroundTextureID = textureID;
            Position = new Point(posX, posY);
        }


        

        public T GetFirstObject<T>() where T : GameObject
        {
            foreach (GameObject go in _objects)
            {
                if (go.GetType() == typeof(T))
                    return (T)go;
            }
            return null;
        }

        public bool ContainsObject<T>(int nb = 1) where T : GameObject
        {
            int count = 0;
            foreach (GameObject go in _objects)
            {
                if (go.GetType() == typeof(T))
                {
                    count++;
                    if (count == nb) return true;
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject obj in _objects)
            {
                obj.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in _objects)
            {
                obj.Draw(spriteBatch);
            }
        }

        public void AddObject(GameObject obj)
        {
            AddObject(obj, true);
        }

        private void AddObject(GameObject obj, bool isMainTile)
        {
            if (obj.IsBig && isMainTile)
            {
                PlaceBigObjectCentered(obj);
            }
            else
            {
                _objects.Add(obj);
            }

            obj.Position = Position;            
        }
        
        private bool PlaceBigObjectCentered(GameObject gameObject)
        {
            if (gameObject.ObjectSprite == null) return false;

            bool objectPlaced = false;

            Point size = gameObject.GetTileSize();
            List<Tile> tiles = TileMap.GetTileArea(Position, size.X, size.Y);
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

                tiles.Remove(this);

                foreach (Tile t in tiles)
                    t.AddObject(gameObject, false);

                AddObject(gameObject, false);

                //////////////////////////////////////////////////////////////////////

                //Set the offset
                //int xOffset = Engine.TileWidth - ((Engine.TileWidth * nbTilesWide - gameObject.ObjectSprite.Width) / 2);
                int xOffset = (gameObject.ObjectSprite.Width - Engine.TileWidth) / 2;

                int yOffset = (Engine.TileHeight * size.Y - gameObject.ObjectSprite.Height) + Engine.TileHeight;

                gameObject.XDrawOffset = -xOffset;
                gameObject.YDrawOffset = -yOffset;

                objectPlaced = true;
            }

            return objectPlaced;
        }
        public void RemoveObject(GameObject obj)
        {
            _objects.Remove(obj);
        }
    }
}
