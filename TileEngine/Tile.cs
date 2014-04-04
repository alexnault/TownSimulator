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

        public void AddObject(GameObject obj)
        {
            crewLock.EnterWriteLock();
            obj.Position = Position;
            _objects.Add(obj);
            crewLock.ExitWriteLock();
        }

        public void RemoveObject(GameObject obj)
        {
            crewLock.EnterWriteLock();
            _objects.Remove(obj);
            crewLock.ExitWriteLock();
        }
    }
}
