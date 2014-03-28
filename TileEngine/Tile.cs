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
                    if(obj.IsSolid) isItSolid = true;
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

        public bool Contains(Type typeSearching, int nb = 1)
        {
            int count = 0;
            foreach (GameObject go in _objects)
            {
                if (go.GetType() == typeSearching)
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
            obj.Position = Position;
            _objects.Add(obj);
        }

        public void RemoveObject(GameObject obj)
        {
            _objects.Remove(obj);
        }
    }
}
