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

        public static void Initialize()
        {
            _textures = new Dictionary<int, Texture2D>();
        }


        public static void Add(Texture2D texture, int id)
        {
            _textures.Add(id, texture);
        }

        public static Texture2D Get(int id)
        {
            return _textures[id];
        }

    }
}
