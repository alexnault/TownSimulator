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

        [System.Xml.Serialization.XmlAttribute]
        public int GroundTextureID { get; set; }

        [System.Xml.Serialization.XmlAttribute]
        public List<GameObject> Objects { get; private set; }

        [System.Xml.Serialization.XmlAttribute]
        public bool IsSolid
        {
            get
            {
                bool isItSolid = _isSolid;

                foreach(GameObject obj in Objects)
                {
                    if(obj.IsSolid) isItSolid = true;
                }

                return isItSolid;
            }
        }


        public Tile(int textureID, bool isSolid = false)
        {
            _isSolid = isSolid;
            GroundTextureID = textureID;
            Objects = new List<GameObject>();
        }

        public bool Contains(Type typeSearching, int nb = 1)
        {
            int count = 0;
            foreach (GameObject go in Objects)
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
            foreach (GameObject obj in Objects)
            {
                obj.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in Objects)
            {
                obj.Draw(spriteBatch);
            }
        }
    }
}
