using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TileEngine
{
    public class Tile
    {
        ReaderWriterLockSlim crewLock;

        private bool _isSolid;
        private List<GameObject> _objects;

        public Point Position { get; private set; }
        public int GroundTextureID { get; set; }
        public bool IsSolid
        {
            get
            {
                crewLock.EnterReadLock();
                bool isItSolid = _isSolid;
                foreach (GameObject obj in _objects)
                {
                    if (obj.IsSolid)
                    {
                        isItSolid = true;
                        break;
                    }
                }
                crewLock.ExitReadLock();
                return isItSolid;
            }
        }


        public Tile(int textureID, int posX, int posY, bool isSolid = false)
        {
            crewLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _isSolid = isSolid;
            _objects = new List<GameObject>();

            GroundTextureID = textureID;
            Position = new Point(posX, posY);
        }


        

        public T GetFirstObject<T>() where T : GameObject
        {
            crewLock.EnterReadLock();
            T obj = null;
            foreach (GameObject go in _objects)
            {
                if (go.GetType() == typeof(T))
                {
                    obj = (T)go;
                    break;
                }
            }
            
            crewLock.ExitReadLock();
            return obj;
        }

        public bool ContainsObject<T>(int nb = 1) where T : GameObject
        {
            crewLock.EnterReadLock();
            bool contains = false;
            int count = 0;
            foreach (GameObject go in _objects)
            {
                if (go.GetType() == typeof(T))
                {
                    count++;
                    
                    if (count == nb)
                    {
                        contains = true;
                        break;
                    }
                }
            }
            
            crewLock.ExitReadLock();
            return contains;
        }

        public void Update(GameTime gameTime)
        {
            crewLock.EnterWriteLock();
            foreach (GameObject obj in _objects.ToList())
            {
                obj.Update(gameTime);
            }
            crewLock.ExitWriteLock();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            crewLock.EnterReadLock();
            foreach (GameObject obj in _objects)
            {
                obj.Draw(spriteBatch);
            }
            crewLock.ExitReadLock();
        }

        //public void AddObject(GameObject obj)
        //{
           
           
        //}

        public void AddObject(GameObject obj)
        {
            AddObject(obj, true);
        }


        private void AddObject(GameObject obj, bool isMainTile)
        {
            crewLock.EnterWriteLock();
            if (obj.IsBig && isMainTile)
            {
                PlaceBigObjectCentered(obj);
            }
            else
            {
                _objects.Add(obj);
            }

            obj.Position = Position;
            crewLock.ExitWriteLock();     
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
                gameObject.XDrawOffset = -((gameObject.ObjectSprite.Width - Engine.TileWidth) / 2);
                gameObject.YDrawOffset = -(gameObject.ObjectSprite.Height - Engine.TileHeight);

                objectPlaced = true;
            }

            return objectPlaced;
        }
        public void RemoveObject(GameObject obj)
        {
            crewLock.EnterWriteLock();
            _objects.Remove(obj);
            crewLock.ExitWriteLock();
        }
    }
}
