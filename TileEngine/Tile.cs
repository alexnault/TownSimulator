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
        public int GroundTextureID { get; set; }
        public List<GameObject> Objects { get; private set; }

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
