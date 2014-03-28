using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public static class TextureManager
    {

        private static Dictionary<int, Texture2D> _textures;
        private static int _nextID;

        public static void Initialize()
        {
            _nextID = 0;
            _textures = new Dictionary<int, Texture2D>();
        }


        public static int Add(Texture2D texture, int id = -1)
        {

            if (id == -1 || _textures.ContainsKey(id))
            {
                id = _nextID;
                _nextID++;
            }
            _textures.Add(id, texture);
            
            return id;
        }

        //public static bool Remove(int id)
        //{
        //    return _textures.Remove(id);
        //}

        public static Texture2D Get(int id)
        {
            return _textures[id];
        }

    }
}
