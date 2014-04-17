using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace TileEngine
{
    public class Tile
    {
        ReaderWriterLockSlim crewLock;

        private bool _isSolid;
        //private List<Tuple<GameObject, bool>> _objects;
        private List<GameObject> _objects;


        public Point Position { get; private set; }
        public int GroundTextureID { get; set; }
        public bool IsSolid
        {
            get
            {
                crewLock.EnterReadLock();
                bool isItSolid = _isSolid;
                foreach (var obj in _objects)
                {
                    //if (obj.Item1.IsSolid)
                    if(obj.IsSolid)
                    {
                        isItSolid = true;
                        break;
                    }
                }
                crewLock.ExitReadLock();
                return isItSolid;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (_objects == null || _objects.Count == 0);
            }

        }


        public Tile(int textureID, int posX, int posY, bool isSolid = false)
        {
            crewLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _isSolid = isSolid;
            //_objects = new List<Tuple<GameObject, bool>>();
            _objects = new List<GameObject>();
            
            GroundTextureID = textureID;
            Position = new Point(posX, posY);
        }
        

        public T GetFirstObject<T>(bool includeChilds = false) where T : GameObject
        {
            crewLock.EnterReadLock();
            T obj = null;
            foreach (var go in _objects)
            {

                //if (go.Item1.GetType() == typeof(T) ||
                //    (includeChilds && go.GetType().IsSubclassOf(typeof(T))))
                //{
                //    obj = (T)go.Item1;
                //    break;
                //}

                if (go.GetType() == typeof(T) ||
                    (includeChilds && go.GetType().IsSubclassOf(typeof(T))))
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
            foreach (var go in _objects)
            {
                //if (go.Item1.GetType() == typeof(T))
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

       


        public void AddObject(GameObject obj)
        {
            AddObject(obj, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isMainTile">If this tile is the main tile of the GameObject added.</param>
        private void AddObject(GameObject obj, bool isMainTile)
        {
            crewLock.EnterWriteLock();

            if (obj.IsBig && isMainTile)
            {
                PlaceBigObjectCentered(obj);
            }
            else
            {
                //_objects.Add(new Tuple<GameObject, bool>(obj, isMainTile));
                _objects.Add(obj);

            }
            obj.Position = Position;

            crewLock.ExitWriteLock();
        }
        
        private bool PlaceBigObjectCentered(GameObject gameObject)
        {
            if (gameObject.ObjectSprite == null) return false;

            bool objectPlaced = false;

            Size size = gameObject.GetTileSize();
            List<Tile> tiles = TileMap.GetTileArea(Position, size.Width, size.Height);
            if (tiles != null)
            {
                foreach (Tile t in tiles)
                {
                    if (t.IsSolid) return false;
                }

                tiles.Remove(this);
                foreach (Tile t in tiles)
                    t.AddObject(gameObject, false);

                AddObject(gameObject, false);

                //Set the offset
                gameObject.XDrawOffset = -((gameObject.ObjectSprite.Width - Engine.TileWidth) / 2);
                gameObject.YDrawOffset = -( gameObject.ObjectSprite.Height - Engine.TileHeight );

                objectPlaced = true;
            }

            return objectPlaced;
        }
        public void RemoveObject(GameObject obj)
        {
            crewLock.EnterWriteLock();
            _objects.Remove(obj);

            //_objects.RemoveAll(x => x.Item1 == obj);

            crewLock.ExitWriteLock();
        }


        public void Update(GameTime gameTime)
        {
            crewLock.EnterWriteLock();
            foreach (var obj in _objects.ToList())
            {
                //if (obj.Item2)
                    //obj.Item1.Update(gameTime);
                obj.Update(gameTime);
            }
            crewLock.ExitWriteLock();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            crewLock.EnterReadLock();
            foreach (var obj in _objects)
            {
                //if (obj.Item2)
                    //obj.Item1.Draw(spriteBatch);
                obj.Draw(spriteBatch);

            }
            crewLock.ExitReadLock();
        }


        public XmlElement GetTileXml(XmlDocument doc)
        {
            XmlElement tile = doc.CreateElement("Tile");

            tile.SetAttribute("IsSolid", _isSolid.ToString());
            tile.SetAttribute("TextureID", GroundTextureID.ToString());
            tile.SetAttribute("Y", Position.Y.ToString());
            tile.SetAttribute("X", Position.X.ToString());

            foreach(var gObj in _objects)
            {
                ////if (!gObj.Item1.IsBig)
                //if (!gObj.IsBig)
                //{
                //    XmlElement obj = doc.CreateElement("GameObject");
                //    obj.SetAttribute("Type", gObj.GetType().AssemblyQualifiedName.ToString());

                //    //if (gObj.Item1.ObjectSprite != null)
                //    if (gObj.ObjectSprite != null)
                //    {
                //        XmlElement sprite = doc.CreateElement("Sprite");
                //        //sprite.SetAttribute("TextureID", gObj.Item1.ObjectSprite.TextureID.ToString());
                //        sprite.SetAttribute("TextureID", gObj.ObjectSprite.TextureID.ToString());
                //        obj.AppendChild(sprite);
                //    }
                //    tile.AppendChild(obj);
                //}
            }


            return tile;

        }

        public static Tile LoadTileFromXml(XmlNode node)
        {

            Tile t;
            int tID = int.Parse(node.Attributes["TextureID"].Value);
            int x = int.Parse(node.Attributes["X"].Value);
            int y = int.Parse(node.Attributes["Y"].Value);
            bool isSolid = bool.Parse(node.Attributes["IsSolid"].Value);


            t = new Tile(tID, x, y, isSolid);

            foreach(XmlNode child in node.ChildNodes)
            {

                Type type = Type.GetType(child.Attributes["Type"].Value);

                GameObject obj = (GameObject)type.GetMethod("LoadFromXml", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy).Invoke(null, new object[] { child });

                t.AddObject(obj);
            }


            return t;
        }
    }
}
