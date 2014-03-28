﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public class GameObject
    {



        //////////////////////////////////////////
        //Testing

        private TimeSpan waitTime = TimeSpan.FromMilliseconds(100);
        private TimeSpan lastGameTime = new TimeSpan();
        public List<Point> path;

        //////////////////////////////////////////

        //private Vector2 _position;

        public Point Position { get; set; }
        //{ 
        //    get
        //    {
        //        return _position;   
        //    } 
        //    set
        //    {
        //        _position = value;
        //        if (ObjectSprite != null)
        //            ObjectSprite.PixelPosition = new Vector2(_position.X, _position.Y);
        //    } 
        //}

    




        public Sprite ObjectSprite { get; set; }
        public bool IsSolid { get; set; }
        
        public GameObject()
        {
            IsSolid = false;
            Position = new Point(0, 0);
        }

        public void Update(GameTime gameTime)
        {
            //////////////////////////////////
            //Testing

            if (lastGameTime > waitTime)
            {
                MoveToNext();
                lastGameTime = TimeSpan.FromMilliseconds(0);
            }

            ////////////////////////////////////



            if (ObjectSprite != null) 
                ObjectSprite.Update(gameTime);



            //////////////////////////////////
            //Testing
            lastGameTime += gameTime.ElapsedGameTime;

            ////////////////////////////////////

        }

        private void MoveToNext()
        {
            if (path != null && path.Count > 0)
            {
                Position = path[0];
                path.RemoveAt(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ObjectSprite != null)
                ObjectSprite.Draw(spriteBatch, Position.X * Engine.TileWidth, Position.Y * Engine.TileHeight);
        }


    }
}
