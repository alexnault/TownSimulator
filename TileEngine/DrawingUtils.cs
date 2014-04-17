using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{

    public static class DrawingUtils
    {
        private static int _textureID;
        private static SpriteBatch _spriteBatch;
        private static Camera _camera;
        private static bool _initialized = false;

        private static Random _rand;

        public static SpriteFont Font {get; set; }
        public static bool DrawingRectangle { get; set; }

        private static float _GUI_Z_TEXT = 0;
        private static float _GUI_Z_RECTANGLE = 0.000000001f;



        public static void Initialize(SpriteBatch spriteBatch, Camera camera)
        {
            DrawingRectangle = true;
            _rand = new Random();
            _textureID = 10;
            _spriteBatch = spriteBatch;
            _camera = camera;
            _initialized = true;
        }


        public static float GetZDelta()
        {
            return (float)_rand.Next(1, 10000) / 1000000.0f;
        }

        /// <summary>
        /// Draw a rectangle on screen.
        /// </summary>
        /// <param name="rect">The rectangle we want to draw.</param>
        /// <param name="couleur">The color of the Rectangle</param>
        public static void DrawRectangle(Rectangle rect, Color color, bool forceDraw = false)
        {
            if ((DrawingRectangle || forceDraw) && _initialized)
            {
                //Top line
                _spriteBatch.Draw(
                    TextureManager.Get(_textureID), 
                    new Rectangle(rect.Left, rect.Top, rect.Width, 1), 
                    null, 
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0 + GetZDelta());

                //Left line
                _spriteBatch.Draw(
                    TextureManager.Get(_textureID),
                    new Rectangle(rect.Left, rect.Top, 1, rect.Height),
                    null,
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0 + GetZDelta());

                //Bottom line
                _spriteBatch.Draw(
                    TextureManager.Get(_textureID),
                    new Rectangle(rect.Left, rect.Bottom, rect.Width, 1),
                    null,
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0 + GetZDelta());

                //Right line
                _spriteBatch.Draw(
                    TextureManager.Get(_textureID),
                    new Rectangle(rect.Right, rect.Top, 1, rect.Height),
                    null,
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0 + GetZDelta());
            }
           
        }




        public static void DrawFullRectangle(Rectangle rect, Color color, bool forceDraw = false)
        {
            if ((DrawingRectangle || forceDraw) && _initialized)
            {
                _spriteBatch.Draw(
                   TextureManager.Get(_textureID),
                   new Rectangle(rect.X + (int)_camera.Position.X, rect.Y + (int)_camera.Position.Y, rect.Width, rect.Height),
                   null,
                   color,
                   0,
                   Vector2.Zero,
                   SpriteEffects.None,
                   _GUI_Z_RECTANGLE);
            }

        }

        public static void DrawDotOnTile(Point tilePosition, Color color)
        {
            DrawOnTile(tilePosition, color, 13);
        }

        public static void DrawOnTile(Point tilePosition, int textureID)
        {
            DrawOnTile(tilePosition, Color.White, textureID);
        }

        public static void DrawOnTile(Point tilePosition, Color color, int textureID)
        {
            if ( _initialized)
            {
                _spriteBatch.Draw(
                    TextureManager.Get(textureID),
                    new Rectangle(tilePosition.X * Engine.TileWidth, tilePosition.Y * Engine.TileHeight, Engine.TileWidth, Engine.TileHeight),
                    null,
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0 + GetZDelta());
            }
        }

        public static void DrawMessage(string message)
        {
            DrawMessage(message, Color.Red);
        }

        public static void DrawMessage(string message, Color color)
        {
            DrawMessage(message, new Vector2(0, 0), color);
        }

        public static void DrawMessage(string message, Vector2 topLeftPos, Color color, bool followCamera = true)
        {
            if (Font != null && _initialized == true)
            {
                Vector2 position = topLeftPos;

                if (followCamera) 
                    position += _camera.Position;

                _spriteBatch.DrawString(Font, message, position, color, 0, Vector2.Zero, 1, SpriteEffects.None, _GUI_Z_TEXT);
            }
        }
    }
}


