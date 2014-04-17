using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public static class SpriteDrawer
    {
        //private static List<Sprite> _sprites;

        //private static SpriteBatch _spriteBatch;

        //public static void Initialize(SpriteBatch spriteBatch)
        //{
        //    _spriteBatch = spriteBatch;
        //    _sprites = new List<Sprite>();
        //}


        //public static bool AddSprite(Sprite s)
        //{
        //    bool added = false;
        //    if (!_sprites.Contains(s))
        //    {
        //        _sprites.Add(s);
        //        added = true;
        //    }

        //    return added;
        //}

        //public static bool RemoveSprite(Sprite s)
        //{
        //    return _sprites.Remove(s);
        //}

        //public static void Draw()
        //{
        //    //_sprites = _sprites.OrderByDescending(x => x.ZIndex).ToList();
        //    foreach(Sprite s in _sprites)
        //    {
        //        //_spriteBatch.Draw(
        //        //   TextureManager.Get(s.TextureID),
        //        //   new Rectangle((int)s.PixelPosition.X + (int)s.Origin.X, (int)s.PixelPosition.Y + (int)s.Origin.Y, s.Width, s.Height),
        //        //   null,
        //        //   Color.White);


        //        _spriteBatch.Draw(
        //            TextureManager.Get(s.TextureID),
        //            new Rectangle((int)s.PixelPosition.X + (int)s.Origin.X, (int)s.PixelPosition.Y + (int)s.Origin.Y, s.Width, s.Height),
        //            s.TexturePortion,
        //            s.DrawingColor,
        //            s.Rotation,
        //            s.Origin,
        //            SpriteEffects.None,
        //            s.ZIndex);
        //    }
        //    _sprites = new List<Sprite>();
        //}

    }
}
