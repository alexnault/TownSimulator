using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{

    //TODO remove this, it is not thread safe
    public class DrawingUtils
    {
        private static int _textureID;
        private static SpriteBatch _spriteBatch;
        private static Camera _camera;
        private static bool _initialized = false;

        public static SpriteFont Font {get; set; }
        public static bool Drawing { get; set; }



        public static void Initialize(SpriteBatch spriteBatch, Camera camera)
        {
            Drawing = true;
            _textureID = 10;
            _spriteBatch = spriteBatch;
            _camera = camera;
            _initialized = true;
        }

        /// <summary>
        /// Draw a rectangle on screen.
        /// </summary>
        /// <param name="rect">The rectangle we want to draw.</param>
        /// <param name="couleur">The color of the Rectangle</param>
        public static void DrawRectangle(Rectangle rect, Color color)
        {
            if (Drawing && _initialized)
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
                    0);

                //Left line
                _spriteBatch.Draw(
                    TextureManager.Get(_textureID),
                    new Rectangle(rect.Left, rect.Top, 1, rect.Height),
                    null,
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);

                //Bottom line
                _spriteBatch.Draw(
                    TextureManager.Get(_textureID),
                    new Rectangle(rect.Left, rect.Bottom, rect.Width, 1),
                    null,
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);

                //Right line
                _spriteBatch.Draw(
                    TextureManager.Get(_textureID),
                    new Rectangle(rect.Right, rect.Top, 1, rect.Height),
                    null,
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);
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

                _spriteBatch.DrawString(Font, message, position, color);
            }
        }
    }
}


